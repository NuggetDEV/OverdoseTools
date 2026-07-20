import tkinter as tk
from tkinter import ttk, filedialog, messagebox
import os
import sys
import base64
import tempfile

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from extract_texture_pc import extract_texture_pc
from repack_texture_pc import repack

ICON_B64 = 'AAABAAEAEBACAAEABQCAAAAgAABFgAAAIAAABAAAAAEAAQAEAAQAQIAAAABAAgAAAIABAAADAAAAAAAgAAAIAAAABAAD4PwAA+D8AAOA/AAD4PwAA8D8AAOA/AADwPwAA8D8AAPg/AADwPwAA4D8AAOA/AADwPwAA8D8AAPg/AAD4PwAA'  # fallback if file missing


def get_icon_path():
    if getattr(sys, 'frozen', False):
        base = sys._MEIPASS
    else:
        base = os.path.dirname(os.path.abspath(__file__))
    path = os.path.join(base, 'Icon2.ico')
    if os.path.exists(path):
        return path
    path2 = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'Icon2.ico')
    if os.path.exists(path2):
        return path2
    return None


class LogRedirect:
    def __init__(self, log_func):
        self.log_func = log_func

    def write(self, text):
        text = text.strip()
        if text:
            self.log_func(text)

    def flush(self):
        pass


class TexturePCApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title('Total Overdose .texture_pc Tool')
        self.resizable(False, False)

        icon_path = get_icon_path()
        if icon_path:
            try:
                self.iconbitmap(icon_path)
            except Exception:
                pass

        self._setup_style()
        self.notebook = ttk.Notebook(self)
        self.notebook.pack(fill='both', expand=True, padx=8, pady=(8,0))
        self._build_extract_tab()
        self._build_repack_tab()
        self.protocol('WM_DELETE_WINDOW', self._on_close)

    def _setup_style(self):
        self.configure(bg='#0a0a0a')
        style = ttk.Style(self)
        style.theme_use('clam')
        style.configure('.',
            background='#0a0a0a',
            foreground='#e0e0e0',
            fieldbackground='#1a1a1a',
            troughcolor='#0a0a0a',
            selectbackground='#2a6a9a',
            selectforeground='#ffffff',
            font=('Consolas', 10))
        style.configure('TLabel', background='#0a0a0a', foreground='#e0e0e0')
        style.configure('TFrame', background='#0a0a0a')
        style.configure('TNotebook', background='#0a0a0a', tabmargins=0)
        style.configure('TNotebook.Tab',
            background='#1a1a1a',
            foreground='#e0e0e0',
            padding=(10, 4),
            borderwidth=0,
            focuscolor='none')
        style.map('TNotebook.Tab',
            background=[('selected', '#2a2a2a'), ('active', '#222222')],
            foreground=[('selected', '#ffffff')])
        style.configure('TButton',
            background='#1a1a1a',
            foreground='#e0e0e0',
            borderwidth=1,
            focuscolor='none',
            padding=(8, 3))
        style.map('TButton',
            background=[('active', '#2a2a2a'), ('pressed', '#333333')],
            foreground=[('active', '#ffffff')])
        style.configure('TEntry',
            fieldbackground='#111111',
            foreground='#e0e0e0',
            borderwidth=1)
        style.configure('Horizontal.TScrollbar',
            background='#1a1a1a',
            troughcolor='#0a0a0a',
            arrowcolor='#e0e0e0')
        style.configure('Vertical.TScrollbar',
            background='#1a1a1a',
            troughcolor='#0a0a0a',
            arrowcolor='#e0e0e0')
        style.configure('TCheckbutton', background='#0a0a0a', foreground='#e0e0e0')
        style.configure('TLabelframe', background='#0a0a0a', foreground='#e0e0e0')
        style.configure('TLabelframe.Label', background='#0a0a0a', foreground='#e0e0e0')

    def _log(self, msg):
        self.log_text.insert('end', msg + '\n')
        self.log_text.see('end')
        self.update_idletasks()

    def _build_extract_tab(self):
        frame = ttk.Frame(self.notebook, padding=12)
        self.notebook.add(frame, text='  Extract  ')

        ttk.Label(frame, text='Source .texture_pc file:').grid(row=0, column=0, sticky='w', pady=(0,4))
        row1 = ttk.Frame(frame)
        row1.grid(row=1, column=0, sticky='ew', pady=(0,8))
        self.extract_src_var = tk.StringVar()
        ttk.Entry(row1, textvariable=self.extract_src_var, width=50).pack(side='left', padx=(0,6))
        ttk.Button(row1, text='Browse', command=self._browse_extract_src).pack(side='left')

        ttk.Label(frame, text='Output directory:').grid(row=2, column=0, sticky='w', pady=(0,4))
        row2 = ttk.Frame(frame)
        row2.grid(row=3, column=0, sticky='ew', pady=(0,12))
        self.extract_out_var = tk.StringVar()
        ttk.Entry(row2, textvariable=self.extract_out_var, width=50).pack(side='left', padx=(0,6))
        ttk.Button(row2, text='Browse', command=self._browse_extract_out).pack(side='left')

        ttk.Button(frame, text='Extract to DDS', command=self._do_extract).grid(row=4, column=0, pady=(0,4))

    def _build_repack_tab(self):
        frame = ttk.Frame(self.notebook, padding=12)
        self.notebook.add(frame, text='  Repack  ')

        ttk.Label(frame, text='Edited .dds file:').grid(row=0, column=0, sticky='w', pady=(0,4))
        row1 = ttk.Frame(frame)
        row1.grid(row=1, column=0, sticky='ew', pady=(0,8))
        self.dds_var = tk.StringVar()
        ttk.Entry(row1, textvariable=self.dds_var, width=50).pack(side='left', padx=(0,6))
        ttk.Button(row1, text='Browse', command=self._browse_dds).pack(side='left')

        ttk.Label(frame, text='Original .texture_pc template:').grid(row=2, column=0, sticky='w', pady=(0,4))
        row2 = ttk.Frame(frame)
        row2.grid(row=3, column=0, sticky='ew', pady=(0,8))
        self.tmpl_var = tk.StringVar()
        ttk.Entry(row2, textvariable=self.tmpl_var, width=50).pack(side='left', padx=(0,6))
        ttk.Button(row2, text='Browse', command=self._browse_template).pack(side='left')

        ttk.Label(frame, text='Output .texture_pc file:').grid(row=4, column=0, sticky='w', pady=(0,4))
        row3 = ttk.Frame(frame)
        row3.grid(row=5, column=0, sticky='ew', pady=(0,12))
        self.out_var = tk.StringVar()
        ttk.Entry(row3, textvariable=self.out_var, width=50).pack(side='left', padx=(0,6))
        ttk.Button(row3, text='Browse', command=self._browse_output).pack(side='left')

        ttk.Button(frame, text='Repack to .texture_pc', command=self._do_repack).grid(row=6, column=0, pady=(0,4))

        tip = ttk.Label(frame,
            text='The original .texture_pc is required as a template.\nAll original fields are preserved; only pixel data is replaced.',
            foreground='#888888', wraplength=450)
        tip.grid(row=7, column=0, sticky='w', pady=(8,0))

        self._log_frame = ttk.LabelFrame(self, text='  Log  ', padding=6)
        self._log_frame.pack(fill='both', expand=True, padx=8, pady=(6,8))
        self.log_text = tk.Text(self._log_frame, height=10, width=72, state='normal', wrap='word',
            bg='#0d0d0d', fg='#00ff41', insertbackground='#00ff41',
            relief='flat', borderwidth=0, font=('Consolas', 10))
        scroll = ttk.Scrollbar(self._log_frame, orient='vertical', command=self.log_text.yview)
        self.log_text.configure(yscrollcommand=scroll.set)
        self.log_text.pack(side='left', fill='both', expand=True)
        scroll.pack(side='right', fill='y')

    def _on_close(self):
        self.destroy()

    def _browse_extract_src(self):
        path = filedialog.askopenfilename(title='Select .texture_pc file',
            filetypes=[('Texture PC', '*.texture_pc'), ('All files', '*.*')])
        if path:
            self.extract_src_var.set(path)
            self.extract_out_var.set(os.path.dirname(path))

    def _browse_extract_out(self):
        path = filedialog.askdirectory(title='Output directory')
        if path:
            self.extract_out_var.set(path)

    def _browse_dds(self):
        path = filedialog.askopenfilename(title='Select edited .dds file',
            filetypes=[('DDS', '*.dds'), ('All files', '*.*')])
        if path:
            self.dds_var.set(path)
            base = os.path.splitext(path)[0]
            self.out_var.set(base + '.texture_pc')
            tmpl = base + '.texture_pc'
            if os.path.exists(tmpl):
                self.tmpl_var.set(tmpl)

    def _browse_template(self):
        path = filedialog.askopenfilename(title='Select original .texture_pc template',
            filetypes=[('Texture PC', '*.texture_pc'), ('All files', '*.*')])
        if path:
            self.tmpl_var.set(path)

    def _browse_output(self):
        path = filedialog.asksaveasfilename(title='Save .texture_pc as',
            defaultextension='.texture_pc',
            filetypes=[('Texture PC', '*.texture_pc'), ('All files', '*.*')])
        if path:
            self.out_var.set(path)

    def _do_extract(self):
        src = self.extract_src_var.get()
        out = self.extract_out_var.get()
        if not src or not os.path.isfile(src):
            messagebox.showerror('Error', 'Select a valid .texture_pc file')
            return
        if not out:
            out = os.path.dirname(src)
        try:
            self._log(f'Extracting: {src}')
            sys.stdout = LogRedirect(self._log)
            extract_texture_pc(src, out)
            sys.stdout = sys.__stdout__
            self._log('Done.')
        except Exception as e:
            sys.stdout = sys.__stdout__
            self._log(f'ERROR: {e}')
            messagebox.showerror('Error', str(e))

    def _do_repack(self):
        dds_path = self.dds_var.get()
        tmpl_path = self.tmpl_var.get()
        out_path = self.out_var.get()
        if not dds_path or not os.path.isfile(dds_path):
            messagebox.showerror('Error', 'Select a valid .dds file')
            return
        if not tmpl_path or not os.path.isfile(tmpl_path):
            messagebox.showerror('Error', 'Select the original .texture_pc template')
            return
        if not out_path:
            out_path = os.path.splitext(dds_path)[0] + '.texture_pc'
        try:
            self._log(f'Repacking: {dds_path}')
            self._log(f'Template:  {tmpl_path}')
            sys.stdout = LogRedirect(self._log)
            repack(dds_path, tmpl_path, out_path)
            sys.stdout = sys.__stdout__
            self._log('Done.')
        except Exception as e:
            sys.stdout = sys.__stdout__
            self._log(f'ERROR: {e}')
            messagebox.showerror('Error', str(e))


if __name__ == '__main__':
    app = TexturePCApp()
    app.mainloop()
