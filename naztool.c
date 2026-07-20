#include <windows.h>
#include <commctrl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <zlib.h>

#define ID_EXTRACT     1001
#define ID_REPACK      1002
#define ID_OUTPUT      1003
#define WM_RUN_DONE    0x4000

#define NAZ_HEAD 0x16ED5B50
#define NAZ_LIST 0x0406F370
#define ZIP_HEAD 0x06054B50
#define ZIP_LIST 0x02014B50
#define ZIP_ITEM 0x04034B50

HINSTANCE hInst;
HWND hOutput, hExtract, hRepack;
HFONT hMonoFont;
HBRUSH hBlackBrush;

#pragma pack(push, 1)
typedef struct {
  DWORD magic; WORD dknum; WORD dkcds; WORD ditem; WORD titem;
  DWORD dsize; DWORD doffs; WORD csize;
} zip_head;

typedef struct {
  DWORD magic; WORD cvers; WORD nvers; WORD flags; WORD compr;
  WORD mtime; WORD mdate; DWORD crc32; DWORD psize; DWORD usize;
  WORD nsize; WORD isize; WORD csize; WORD dstrt; WORD iattr;
  DWORD eattr; DWORD hoffs;
} zip_list;

typedef struct {
  DWORD magic; WORD nvers; WORD flags; WORD compr; WORD mtime; WORD mdate;
  DWORD crc32; DWORD psize; DWORD usize; WORD nsize; WORD isize;
} zip_item;

typedef struct {
  DWORD magic; WORD mtime; WORD mdate; DWORD crc32; DWORD psize; DWORD usize;
  WORD nsize; WORD isize; WORD csize; WORD dstrt; WORD iattr; DWORD eattr;
  DWORD hoffs; WORD cvers; WORD nvers; WORD flags; WORD compr;
} naz_list;
#pragma pack(pop)

void AppendOutput(const char *text) {
    int len = GetWindowTextLengthA(hOutput);
    SendMessageA(hOutput, EM_SETSEL, len, len);
    SendMessageA(hOutput, EM_REPLACESEL, FALSE, (LPARAM)text);
    SendMessageA(hOutput, EM_SCROLLCARET, 0, 0);
}

static char *basename(char *s) {
    if (!s) return s;
    char *r;
    for (r = s; *r; r++)
        if (*r == '/' || *r == '\\') s = &r[1];
    return s;
}

static void newext(char *name, char *ext) {
    if (!name || !ext) return;
    for (; *name == '.'; name++);
    char *p = NULL;
    for (char *q = name; *q; q++)
        if (*q == '.') p = q;
    strcpy(p ? p : name, ext);
}

/* === EXTRACTOR (NAZ → ZIP) === */

#define MAX_BLOCK 0x800

static void CRC32BuildTable(DWORD *r) {
    DWORD i, j;
    for (i = 0; i < 256; i++) {
        r[i] = i;
        for (j = 0; j < 8; j++)
            r[i] = (r[i] >> 1) ^ (0xEDB88320 * (r[i] & 1));
    }
}

static DWORD CRC32Calc(BYTE *buf, DWORD sz) {
    DWORD crctab[256], crc = (DWORD)-1;
    CRC32BuildTable(crctab);
    for (DWORD i = 0; i < sz; i++)
        crc = crctab[LOBYTE(crc ^ buf[i])] ^ (crc >> 8);
    return crc ^ (DWORD)-1;
}

static void NAZNameDecryptInPlace(FILE *fl, DWORD sz) {
    BYTE b[MAX_BLOCK];
    DWORD o = ftell(fl);
    while (sz) {
        DWORD l = (sz < MAX_BLOCK) ? sz : MAX_BLOCK;
        fseek(fl, o, SEEK_SET);
        fread(b, l, 1, fl);
        for (DWORD i = 0; i < l; i++)
            b[i] = (b[i] << 2) | (b[i] >> 6);
        fseek(fl, o, SEEK_SET);
        fwrite(b, l, 1, fl);
        o += l; sz -= l;
    }
}

static void NAZCopyBlocks(FILE *fl, DWORD sz, DWORD o1, DWORD o2) {
    BYTE b[MAX_BLOCK];
    while (sz) {
        DWORD l = (sz < MAX_BLOCK) ? sz : MAX_BLOCK;
        fseek(fl, o1, SEEK_SET); fread(b, l, 1, fl);
        fseek(fl, o2, SEEK_SET); fwrite(b, l, 1, fl);
        o1 += l; o2 += l; sz -= l;
    }
}

static int DoExtract(const char *input, HWND hwnd) {
    char output[MAX_PATH], s[MAX_PATH];
    strcpy(s, basename((char*)input));
    AppendOutput(s); AppendOutput(" => ");
    strcpy(output, s);
    newext(output, ".zip");
    AppendOutput(output); AppendOutput("\r\n\r\n");

    AppendOutput("Copying file...\r\n");
    if (!CopyFileA(input, output, FALSE)) {
        AppendOutput("Error: can't create output file.\r\n");
        return 5;
    }
    SetFileAttributesA(output, FILE_ATTRIBUTE_NORMAL);

    FILE *fl = fopen(output, "rb+");
    if (!fl) { AppendOutput("Error: can't open output file.\r\n"); return 6; }

    fseek(fl, 0, SEEK_END);
    DWORD fsize = ftell(fl);
    DWORD j = fsize - sizeof(zip_head);

    zip_head zh;
    memset(&zh, 0, sizeof(zh));
    fseek(fl, j, SEEK_SET);
    fread(&zh, sizeof(zh), 1, fl);

    if ((zh.magic ^ NAZ_HEAD) || (zh.ditem ^ zh.titem)) {
        fclose(fl);
        AppendOutput("Error: not a NAZ archive.\r\n");
        return 4;
    }

    zh.magic = ZIP_HEAD;
    fseek(fl, j, SEEK_SET);
    fwrite(&zh, sizeof(zh), 1, fl);

    zip_list zl;
    zip_item zi;
    naz_list nl;
    zl.magic = ZIP_LIST;
    zi.magic = ZIP_ITEM;

    AppendOutput("Decrypting entries...\r\n");
    DWORD i;
    for (i = 0; i < zh.titem; i++) {
        fseek(fl, zh.doffs, SEEK_SET);
        if (fread(&nl, sizeof(nl), 1, fl) != 1 || nl.magic != NAZ_LIST) break;

        zl.cvers = nl.cvers; zl.nvers = nl.nvers; zl.flags = nl.flags;
        zl.compr = nl.compr; zl.mtime = nl.mtime; zl.mdate = nl.mdate;
        zl.crc32 = nl.crc32; zl.psize = nl.psize; zl.usize = nl.usize;
        zl.nsize = nl.nsize; zl.isize = nl.isize; zl.csize = nl.csize;
        zl.dstrt = nl.dstrt; zl.iattr = nl.iattr; zl.eattr = nl.eattr;
        zl.hoffs = nl.hoffs;

        fseek(fl, zl.hoffs + sizeof(zi) + zl.nsize + zl.isize, SEEK_SET);
        BYTE *buf = (BYTE*)malloc(zl.usize);
        if (buf) {
            fread(buf, 1, zl.usize, fl);
            nl.crc32 = CRC32Calc(buf, zl.usize);
            free(buf);
        }
        fwrite(&nl.crc32, 4, 1, fl);

        zl.crc32 = nl.crc32;
        fseek(fl, zh.doffs, SEEK_SET);
        fwrite(&zl, sizeof(zl), 1, fl);
        zh.doffs += sizeof(zl);

        NAZNameDecryptInPlace(fl, zl.nsize);

        zi.nvers = nl.nvers; zi.flags = nl.flags; zi.compr = nl.compr;
        zi.mtime = nl.mtime; zi.mdate = nl.mdate; zi.crc32 = nl.crc32;
        zi.psize = nl.psize; zi.usize = nl.usize;
        zi.nsize = nl.nsize; zi.isize = nl.isize;

        fseek(fl, zl.hoffs, SEEK_SET);
        fwrite(&zi, sizeof(zi), 1, fl);
        NAZCopyBlocks(fl, zl.nsize + zl.isize, zh.doffs, ftell(fl));
        zh.doffs += zl.nsize + zl.isize + zl.csize;

        if ((i + 1) % 50 == 0) { char d[16]; sprintf(d, "%u\r\n", i+1); AppendOutput(d); }
    }

    fclose(fl);
    char done[64];
    sprintf(done, "\r\n%d entries processed.\r\ndone\r\n", i);
    AppendOutput(done);
    return (i >= zh.titem) ? 0 : 7;
}


/* === REPACKER (ZIP → NAZ) === */

static void NameEncrypt(BYTE *buf, DWORD sz) {
    for (DWORD i = 0; i < sz; i++)
        buf[i] = (buf[i] >> 2) | (buf[i] << 6);
}

static int DoRepack(const char *input, HWND hwnd) {
    char output[MAX_PATH], s[MAX_PATH];
    strcpy(s, basename((char*)input));
    AppendOutput(s); AppendOutput(" => ");
    strcpy(output, s);
    newext(output, ".naz");
    AppendOutput(output); AppendOutput("\r\n\r\n");

    FILE *fi = fopen(input, "rb");
    if (!fi) { AppendOutput("Error: can't open input file.\r\n"); return 2; }

    fseek(fi, 0, SEEK_END);
    DWORD fsize = ftell(fi);
    if (fsize <= sizeof(zip_head)) { fclose(fi); AppendOutput("Error: file too small.\r\n"); return 3; }

    zip_head zh;
    memset(&zh, 0, sizeof(zh));
    fseek(fi, fsize - sizeof(zh), SEEK_SET);
    fread(&zh, sizeof(zh), 1, fi);
    if (zh.magic != ZIP_HEAD) { fclose(fi); AppendOutput("Error: not a ZIP archive.\r\n"); return 4; }

    char count[32];
    sprintf(count, "Entries: %u\r\n\r\n", zh.titem);
    AppendOutput(count);

    zip_list *zl = (zip_list*)malloc(zh.titem * sizeof(zip_list));
    BYTE **names = (BYTE**)calloc(zh.titem, sizeof(BYTE*));
    BYTE **extras = (BYTE**)calloc(zh.titem, sizeof(BYTE*));
    BYTE **comments = (BYTE**)calloc(zh.titem, sizeof(BYTE*));
    DWORD *new_hoffs = (DWORD*)malloc(zh.titem * sizeof(DWORD));
    if (!zl || !names || !extras || !comments || !new_hoffs) {
        if (zl) free(zl); if (names) free(names); if (extras) free(extras);
        if (comments) free(comments); if (new_hoffs) free(new_hoffs);
        fclose(fi); AppendOutput("Error: memory allocation failed.\r\n"); return 5;
    }

    fseek(fi, zh.doffs, SEEK_SET);
    DWORD i;
    for (i = 0; i < zh.titem; i++) {
        if (fread(&zl[i], sizeof(zip_list), 1, fi) != 1 || zl[i].magic != ZIP_LIST) break;
        names[i] = (BYTE*)malloc(zl[i].nsize);
        extras[i] = (BYTE*)malloc(zl[i].isize);
        comments[i] = (BYTE*)malloc(zl[i].csize);
        if (zl[i].nsize && names[i]) fread(names[i], 1, zl[i].nsize, fi);
        if (zl[i].isize && extras[i]) fread(extras[i], 1, zl[i].isize, fi);
        if (zl[i].csize && comments[i]) fread(comments[i], 1, zl[i].csize, fi);
    }

    if (i < zh.titem) {
        AppendOutput("Error: invalid CD.\r\n");
        for (DWORD j = 0; j < zh.titem; j++) {
            if (names[j]) free(names[j]); if (extras[j]) free(extras[j]); if (comments[j]) free(comments[j]);
        }
        free(zl); free(names); free(extras); free(comments); free(new_hoffs);
        fclose(fi); return 7;
    }

    DWORD decomp_count = 0;
    for (i = 0; i < zh.titem; i++)
        if (zl[i].compr != 0) decomp_count++;

    if (decomp_count) {
        char note[64];
        sprintf(note, "Note: %u entries use compression, will decompress.\r\n\r\n", decomp_count);
        AppendOutput(note);
    }

    FILE *fo = fopen(output, "wb");
    if (!fo) {
        AppendOutput("Error: can't create output.\r\n");
        for (DWORD j = 0; j < zh.titem; j++) {
            if (names[j]) free(names[j]); if (extras[j]) free(extras[j]); if (comments[j]) free(comments[j]);
        }
        free(zl); free(names); free(extras); free(comments); free(new_hoffs);
        fclose(fi); return 6;
    }

    AppendOutput("Writing file data...\r\n");
    for (i = 0; i < zh.titem; i++) {
        new_hoffs[i] = ftell(fo);

        fseek(fi, zl[i].hoffs, SEEK_SET);
        zip_item zi;
        fread(&zi, sizeof(zi), 1, fi);

        BYTE *local_name = (BYTE*)malloc(zl[i].nsize);
        BYTE *local_extra = (BYTE*)malloc(zl[i].isize);
        BYTE *data = (BYTE*)malloc(zl[i].psize);
        if (zl[i].nsize && local_name) fread(local_name, 1, zl[i].nsize, fi);
        if (zl[i].isize && local_extra) fread(local_extra, 1, zl[i].isize, fi);
        fseek(fi, zl[i].hoffs + 30 + zi.nsize + zi.isize, SEEK_SET);
        if (zl[i].psize && data) fread(data, 1, zl[i].psize, fi);

        BYTE *stored = data;
        z_stream strm;
        if (zl[i].compr != 0) {
            stored = (BYTE*)malloc(zl[i].usize);
            if (stored) {
                memset(&strm, 0, sizeof(strm));
                strm.next_in = data;
                strm.avail_in = zl[i].psize;
                strm.next_out = stored;
                strm.avail_out = zl[i].usize;
                if (inflateInit2(&strm, -MAX_WBITS) == Z_OK &&
                    inflate(&strm, Z_FINISH) == Z_STREAM_END) {
                    inflateEnd(&strm);
                    zl[i].compr = 0;
                    zl[i].psize = zl[i].usize;
                    zi.compr = 0;
                    zi.psize = zl[i].usize;
                } else {
                    inflateEnd(&strm);
                    free(stored); stored = data;
                }
            }
        }

        zi.nsize = zl[i].nsize;
        zi.isize = zl[i].isize;
        fwrite(&zi, sizeof(zi), 1, fo);

        if (local_name && zl[i].nsize) {
            NameEncrypt(local_name, zl[i].nsize);
            fwrite(local_name, 1, zl[i].nsize, fo);
        }
        if (local_extra && zl[i].isize) fwrite(local_extra, 1, zl[i].isize, fo);
        if (stored && zl[i].psize) fwrite(stored, 1, zl[i].psize, fo);
        fwrite(&zl[i].crc32, 4, 1, fo);
        fwrite(&zl[i].psize, 4, 1, fo);
        fwrite(&zl[i].usize, 4, 1, fo);

        if (local_name) free(local_name);
        if (local_extra) free(local_extra);
        if (stored && stored != data) free(stored);
        if (data) free(data);
    }

    AppendOutput("Writing central directory...\r\n");
    DWORD cd_start = ftell(fo);
    for (i = 0; i < zh.titem; i++) {
        naz_list nl;
        memset(&nl, 0, sizeof(nl));
        nl.magic = NAZ_LIST;
        nl.mtime = zl[i].mtime; nl.mdate = zl[i].mdate;
        nl.crc32 = zl[i].crc32; nl.psize = zl[i].psize; nl.usize = zl[i].usize;
        nl.nsize = zl[i].nsize; nl.isize = zl[i].isize; nl.csize = zl[i].csize;
        nl.dstrt = zl[i].dstrt; nl.iattr = zl[i].iattr; nl.eattr = zl[i].eattr;
        nl.hoffs = new_hoffs[i];
        nl.cvers = zl[i].cvers; nl.nvers = zl[i].nvers;
        nl.flags = zl[i].flags; nl.compr = zl[i].compr;
        fwrite(&nl, sizeof(nl), 1, fo);

        if (names[i] && zl[i].nsize) {
            NameEncrypt(names[i], zl[i].nsize);
            fwrite(names[i], 1, zl[i].nsize, fo);
        }
        if (extras[i] && zl[i].isize) fwrite(extras[i], 1, zl[i].isize, fo);
        if (comments[i] && zl[i].csize) fwrite(comments[i], 1, zl[i].csize, fo);
    }

    DWORD cd_end = ftell(fo);
    zh.magic = NAZ_HEAD;
    zh.doffs = cd_start;
    zh.dsize = cd_end - cd_start;
    fwrite(&zh, sizeof(zh), 1, fo);

    fclose(fo);
    fclose(fi);
    for (DWORD j = 0; j < zh.titem; j++) {
        if (names[j]) free(names[j]);
        if (extras[j]) free(extras[j]);
        if (comments[j]) free(comments[j]);
    }
    free(zl); free(names); free(extras); free(comments); free(new_hoffs);

    AppendOutput("\r\ndone\r\n");
    return 0;
}


/* === THREADING === */

typedef struct {
    int mode; // 0 = extract, 1 = repack
    char path[MAX_PATH];
} JobParams;

DWORD WINAPI WorkThread(LPVOID lpParam) {
    JobParams *jp = (JobParams*)lpParam;
    int rc;
    if (jp->mode == 0)
        rc = DoExtract(jp->path, NULL);
    else
        rc = DoRepack(jp->path, NULL);
    char buf[32];
    sprintf(buf, "\r\n=== Exit code: %d ===\r\n", rc);
    AppendOutput(buf);
    PostMessage(GetParent(hOutput), WM_RUN_DONE, rc, 0);
    free(jp);
    return 0;
}

void StartJob(int mode) {
    JobParams *jp = malloc(sizeof(JobParams));
    jp->mode = mode;

    char path[1024] = {0};
    OPENFILENAMEA ofn = {0};
    ofn.lStructSize = sizeof(ofn);
    ofn.lpstrFile = path;
    ofn.nMaxFile = sizeof(path);
    ofn.lpstrDefExt = (mode == 0) ? "naz" : "zip";
    ofn.lpstrFilter = (mode == 0)
        ? "NAZ Files\0*.NAZ\0All Files\0*.*\0"
        : "ZIP Files\0*.ZIP\0All Files\0*.*\0";
    ofn.Flags = OFN_FILEMUSTEXIST | OFN_HIDEREADONLY;

    if (!GetOpenFileNameA(&ofn)) {
        free(jp);
        return;
    }

    strcpy(jp->path, path);

    AppendOutput("\r\n");
    EnableWindow(hExtract, FALSE);
    EnableWindow(hRepack, FALSE);
    CreateThread(NULL, 0, WorkThread, jp, 0, NULL);
}


/* === WINDOW PROC === */

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam) {
    switch (msg) {
    case WM_CREATE:
        hMonoFont = CreateFontA(16, 0, 0, 0, FW_NORMAL, FALSE, FALSE, FALSE,
            ANSI_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
            DEFAULT_QUALITY, FIXED_PITCH | FF_MODERN, "Consolas");
        hBlackBrush = CreateSolidBrush(RGB(0, 0, 0));

        CreateWindowA("STATIC", "NAZ Tool v2 - Total Overdose", WS_CHILD | WS_VISIBLE,
            10, 8, 300, 24, hwnd, NULL, hInst, NULL);

        hExtract = CreateWindowA("BUTTON", "Extract NAZ file...",
            WS_CHILD | WS_VISIBLE, 10, 40, 220, 36,
            hwnd, (HMENU)ID_EXTRACT, hInst, NULL);
        hRepack = CreateWindowA("BUTTON", "Repack ZIP file...",
            WS_CHILD | WS_VISIBLE, 250, 40, 220, 36,
            hwnd, (HMENU)ID_REPACK, hInst, NULL);

        hOutput = CreateWindowA("EDIT", "", WS_CHILD | WS_VISIBLE | WS_VSCROLL |
            ES_MULTILINE | ES_READONLY | ES_AUTOVSCROLL | ES_NOHIDESEL,
            10, 90, 460, 330, hwnd, (HMENU)ID_OUTPUT, hInst, NULL);
        SendMessageA(hOutput, WM_SETFONT, (WPARAM)hMonoFont, 0);
        break;

    case WM_CTLCOLORSTATIC: {
        HDC hdc = (HDC)wParam;
        SetTextColor(hdc, RGB(0, 255, 0));
        SetBkColor(hdc, RGB(0, 0, 0));
        return (LRESULT)hBlackBrush;
    }

    case WM_CTLCOLOREDIT: {
        HDC hdc = (HDC)wParam;
        SetTextColor(hdc, RGB(255, 255, 255));
        SetBkColor(hdc, RGB(10, 10, 10));
        return (LRESULT)hBlackBrush;
    }

    case WM_COMMAND:
        switch (LOWORD(wParam)) {
        case ID_EXTRACT: StartJob(0); break;
        case ID_REPACK:  StartJob(1); break;
        }
        break;

    case WM_RUN_DONE:
        EnableWindow(hExtract, TRUE);
        EnableWindow(hRepack, TRUE);
        AppendOutput("\r\n");
        break;

    case WM_DESTROY:
        DeleteObject(hMonoFont);
        DeleteObject(hBlackBrush);
        PostQuitMessage(0);
        break;

    default:
        return DefWindowProcA(hwnd, msg, wParam, lParam);
    }
    return 0;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
    LPSTR lpCmdLine, int nCmdShow)
{
    WNDCLASSEXA wc = {0};
    HWND hwnd;
    MSG msg;

    hInst = hInstance;
    InitCommonControls();

    wc.cbSize = sizeof(WNDCLASSEXA);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.hIcon = LoadIconA(hInstance, "IDI_ICON1");
    wc.hCursor = LoadCursor(NULL, IDC_ARROW);
    wc.hbrBackground = CreateSolidBrush(RGB(0, 0, 0));
    wc.lpszClassName = "NazToolClass";

    RegisterClassExA(&wc);

    hwnd = CreateWindowExA(0, "NazToolClass", "NAZ Tool - Total Overdose",
        WS_OVERLAPPEDWINDOW & ~WS_THICKFRAME & ~WS_MAXIMIZEBOX,
        CW_USEDEFAULT, CW_USEDEFAULT, 500, 470,
        NULL, NULL, hInstance, NULL);

    if (!hwnd) return 0;

    HICON hIcon = LoadIconA(hInstance, "IDI_ICON1");
    if (hIcon) {
        SendMessage(hwnd, WM_SETICON, ICON_SMALL, (LPARAM)hIcon);
        SendMessage(hwnd, WM_SETICON, ICON_BIG, (LPARAM)hIcon);
    }

    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    while (GetMessage(&msg, NULL, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
    return msg.wParam;
}
