# OverdoseTools
The first modding tools for TOD The Game

Total Overdose Modding Tools Collection

After spending countless hours digging through Total Overdose's files and figuring out how the game stores its data, I finally put together a collection of tools to make modding the game a whole lot easier. Whether you're making simple tweaks or planning a full overhaul, these utilities should save you plenty of time and frustration.

Included Tools
NAZTool

The first step into modding Total Overdose. This utility lets you extract and rebuild the game's
.naz
archives, giving you access to the files hidden inside. Once you've finished editing, simply repack everything back into a working archive.

Important: When repacking, make sure your folder structure matches the original archive. For example, if a file originally belongs directly inside
data_pc
, don't repack it as
blocks\data_pc\...
or add any extra folders. The game expects the original directory layout, and packing files into the wrong folder structure will usually cause it to crash. If you keep the original layout intact, everything should work as expected.

TexturePC Tool

Customize Total Overdose's loading screens with ease. This tool lets you open and edit the
.texture_pc
files found inside
blocks.naz
, allowing you to replace the game's loading screen artwork with your own images or restore the originals whenever you want.

TOD Audio Editor

A complete audio editing utility for Total Overdose. Extract and replace WAV sound effects, or edit the game's STREAM entries by changing their pitch, playback speed, volume, and OGG references. Whether you're replacing sound effects, tweaking the audio mix, or creating your own custom soundtrack, this tool gives you full control over the game's audio. NOTE: all the wav files and stream files are in the language files. for example overdose_uk.main, overdose_de.main and etc. don't open overdose.main because it doesn't have any sort of wav or stream files.

TOD Text Editor

Total Overdose stores most of its text using Huffman compression, making it difficult to edit by hand. This tool takes care of the decoding and encoding for you, allowing you to edit via notepad. Mission dialogue, subtitles, menu text, and more can all be modified without worrying about the compression format.

These tools were created with one goal in mind: making Total Overdose easier to mod. For years, the game's file formats were largely unexplored, making even simple edits a challenge. Hopefully this toolkit helps lower that barrier and encourages more people to create new mods for one of the most underrated action games of the PS2/Xbox era.

If you end up making something with these tools, I'd love to see what you come up with. Have fun modding, and don't be afraid to experiment!

If you encountered some bug, or anything just make sure to contact me on discord. User: nuggetrespawn

Big thanks to CTPAX, Micheal0ne, Amr shaheen and seifmagdi for making this possible!!!
