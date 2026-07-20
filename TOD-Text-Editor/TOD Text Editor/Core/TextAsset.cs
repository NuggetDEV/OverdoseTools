using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text;


namespace TOD_Localization_Tool
{
    public class TextAsset : AssetHeader
    {

        static readonly Dictionary<char, string> TransliterationMap = new Dictionary<char, string>
        {
            {'á',"a"}, {'à',"a"}, {'â',"a"}, {'ã',"a"}, {'ä',"a"}, {'å',"a"}, {'ā',"a"}, {'ă',"a"}, {'ą',"a"},
            {'é',"e"}, {'è',"e"}, {'ê',"e"}, {'ë',"e"}, {'ē',"e"}, {'ĕ',"e"}, {'ę',"e"}, {'ė',"e"},
            {'í',"i"}, {'ì',"i"}, {'î',"i"}, {'ï',"i"}, {'ī',"i"}, {'ĭ',"i"}, {'į',"i"},
            {'ó',"o"}, {'ò',"o"}, {'ô',"o"}, {'õ',"o"}, {'ö',"o"}, {'ō',"o"}, {'ŏ',"o"}, {'ő',"o"},
            {'ú',"u"}, {'ù',"u"}, {'û',"u"}, {'ü',"u"}, {'ū',"u"}, {'ŭ',"u"}, {'ů',"u"},
            {'ý',"y"}, {'ÿ',"y"},
            {'ñ',"n"}, {'ń',"n"}, {'ň',"n"},
            {'ç',"c"}, {'ĉ',"c"}, {'ć',"c"}, {'č',"c"},
            {'ğ',"g"}, {'ĝ',"g"}, {'ģ',"g"},
            {'š',"s"}, {'ş',"s"}, {'ś',"s"},
            {'ž',"z"}, {'ź',"z"}, {'ż',"z"},
            {'đ',"d"}, {'Ð',"D"}, {'Đ',"D"},
            {'ł',"l"}, {'Ł',"L"},
            {'Č',"C"}, {'Š',"S"}, {'Ž',"Z"},
            {'Á',"A"}, {'À',"A"}, {'Â',"A"}, {'Ã',"A"}, {'Ä',"A"}, {'Å',"A"},
            {'É',"E"}, {'È',"E"}, {'Ê',"E"}, {'Ë',"E"},
            {'Í',"I"}, {'Ì',"I"}, {'Î',"I"}, {'Ï',"I"},
            {'Ó',"O"}, {'Ò',"O"}, {'Ô',"O"}, {'Õ',"O"}, {'Ö',"O"},
            {'Ú',"U"}, {'Ù',"U"}, {'Û',"U"}, {'Ü',"U"},
            {'Ý',"Y"},
            {'Ñ',"N"},
            {'Ç',"C"},
            {'Ğ',"G"}, {'Ģ',"G"},
            {'Ř',"R"}, {'ř',"r"},
            {'œ',"oe"}, {'Œ',"OE"},
            {'æ',"ae"}, {'Æ',"AE"},
            {'ß',"ss"},
            {'þ',"th"}, {'Þ',"TH"},
            {'ð',"d"},
            {'ı',"i"},
            {'>',""},
            {'‑',"-"},
            {'А',"A"}, {'Б',"B"}, {'В',"V"}, {'Г',"G"}, {'Д',"D"},
            {'Е',"E"}, {'Ё',"Yo"}, {'Ж',"Zh"}, {'З',"Z"}, {'И',"I"},
            {'Й',"Y"}, {'К',"K"}, {'Л',"L"}, {'М',"M"}, {'Н',"N"},
            {'О',"O"}, {'П',"P"}, {'Р',"R"}, {'С',"S"}, {'Т',"T"},
            {'У',"U"}, {'Ф',"F"}, {'Х',"Kh"}, {'Ц',"Ts"}, {'Ч',"Ch"},
            {'Ш',"Sh"}, {'Щ',"Shch"}, {'Ъ',""}, {'Ы',"Y"}, {'Ь',""},
            {'Э',"E"}, {'Ю',"Yu"}, {'Я',"Ya"},
            {'а',"a"}, {'б',"b"}, {'в',"v"}, {'г',"g"}, {'д',"d"},
            {'е',"e"}, {'ё',"yo"}, {'ж',"zh"}, {'з',"z"}, {'и',"i"},
            {'й',"y"}, {'к',"k"}, {'л',"l"}, {'м',"m"}, {'н',"n"},
            {'о',"o"}, {'п',"p"}, {'р',"r"}, {'с',"s"}, {'т',"t"},
            {'у',"u"}, {'ф',"f"}, {'х',"kh"}, {'ц',"ts"}, {'ч',"ch"},
            {'ш',"sh"}, {'щ',"shch"}, {'ъ',""}, {'ы',"y"}, {'ь',""},
            {'э',"e"}, {'ю',"yu"}, {'я',"ya"},
            {'Є',"Ye"}, {'І',"I"}, {'Ї',"Yi"}, {'Ґ',"G"},
            {'є',"ye"}, {'і',"i"}, {'ї',"yi"}, {'ґ',"g"},
        };

        static public Dictionary<char, string> CodesDictionary = new Dictionary<char, string>()
        {
           {'Θ', "[TOTAL_OVERDOSE_LOGO]"},
           {'α', "[XBOX_A_BUTTON]"},
           {'β', "[XBOX_B_BUTTON]"},
           {'λ', "[XBOX_X_BUTTON]"},
           {'π', "[XBOX_Y_BUTTON]"},
           {'†', "[XBOX_START_BUTTON]"},
           {'‡', "[XBOX_BACK_BUTTON]"},
           {'⅓', "[PS2_START_BUTTON]"},
           {'⅔', "[PS2_SELECT_BUTTON]"},
           {'⅛', "[PS2_L1_BUTTON]"},
           {'⅜', "[PS2_R1_BUTTON]"},
           {'⅝', "[PS2_R2_BUTTON]"},
           {'⅞', "[PS2_L2_BUTTON]"},
           {'←', "[XBOX_LEFT_BUTTON]"},
           {'↑', "[XBOX_UP_BUTTON]"},
           {'→', "[XBOX_RIGHT_BUTTON]"},
           {'↓', "[XBOX_BOTTOM_BUTTON]"},
           {'┌', "[PS2_X_BUTTON]"},
           {'┐', "[PS2_O_BUTTON]"},
           {'└', "[PS2_TRIANGLE_BUTTON]"},
           {'┘', "[PS2_SQUARE_BUTTON]"},
           {'├', "[PS2_RIGHT_BUTTON]"},
           {'┤', "[PS2_LEFT_BUTTON]"},
           {'┬', "[PS2_BOTTOM_BUTTON]"},
           {'┴', "[PS2_UP_BUTTON]"},
           {'╟', "[XBOX_DX_BUTTON]"},
           {'╠', "[XBOX_D_BUTTON]"},
           {'╡', "[XBOX_G_BUTTON]"},
           {'╢', "[XBOX_SX_BUTTON]"},
           {'╣', "[XBOX_I_BUTTON]"},
           {'░', "[TWO_GUNS_ICON]"},
           {'▒', "[RED_DIAMETER_ICON]"},
           {'▓', "[WHITE_DIAMETER_ICON]"},
           {'■', "[MAN_ICON]"},
           {'□', "[STAR_ICON]"},
           {'▪', "[RED_SQURE_ICON]"},
           {'▫', "[RED_CIRCLE_AND_WHITE_ICON]"},
           {'►', "[XBOX_L_BUTTON]"},
           {'◄', "[XBOX_R_BUTTON]"},
           {'○', "[RED_DOT_ICON]"},
           {'●', "[YELLOW_DOT_ICON"},
           {'◘', "[XBOX_WHITE_BUTTON]"},
           {'◙', "[XBOX_BLACK_BUTTON]"},
           {'☺', "[CITY_TOWERS_ICON]"},
           {'☻', "[CITY_CASTLE_ICON]"},
           {'☼', "[CITY_CHURCH_ICON]"},
           {'♀', "[CITY_TOWERS2_ICON]"},
           {'♂', "[CITY_FACTORES_ICON]"},
           {'♠', "[CITY_US_ICON]"},
           {'♣', "[CITY_SHIPPORT_ICON]"},
           {'♥', "[CITY_WRESTRING_ICON]"},
           {'♦', "[CITY_DESERT_ICON]"},
           {'♪', "[CITY_TOWERS3_ICON]"},
           {'♫', "[CITY_FACE_ICON]"}
        };





        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct TextHeader
        {
            public int Unkown;

            //CharMap Block
            //From this offset + CharsMapOffset
            public int CharsMapOffset;
            public int Unkown_1;
            public int Unkown_2;
            public int Unkown_3;

            //CharInfo Block
            //From this offset + CharInfoOffset
            public int CharInfoOffset;
            public int TextCount;
            public int TextCount_1;
            public int Unkown_4;

            //GenralInfo Block
            public int Val1;
            public int Val2;
            public int Val3;
            public int Unkown_5;

            //From this offset + StartOffset
            public int StartOffset;

            //AssetHeader.Header.Unkown_1
            public int Unkown_6;
        }



        class CharNode
        {

            public CharNode Right;
            public CharNode Left;
            public int CharNodeOffset;

            public ushort Item;
            public ushort Item2;

            public long Offset;
            public MStream Stream;

            public CharNode PreviousNode;


            public CharNode ReadNodeValue(long offset)
            {
                Stream.SetPosition(offset);
                var charnode = new CharNode();
                charnode.Offset = offset;
                charnode.Right = new CharNode { CharNodeOffset = Stream.GetIntValue() };
                charnode.Left = new CharNode { CharNodeOffset = Stream.GetIntValue() };
                charnode.Item = Stream.GetUShortValue();
                charnode.Item2 = Stream.GetUShortValue();
                charnode.Stream = Stream;
                return charnode;
            }

            public CharNode GetRightCharNode()
            {
                CharNode temp = null;
                try
                {
                    temp = ReadNodeValue(Offset + Right.CharNodeOffset);
                    temp.PreviousNode = this;
                }
                catch
                {
                    return temp;
                }
                return temp;
            }

            public CharNode GetLeftCharNode()
            {
                CharNode temp = null;
                try
                {
                    temp = ReadNodeValue(Offset + Left.CharNodeOffset + 4);
                    temp.PreviousNode = this;
                }
                catch
                {
                    return temp;
                }
                return temp;
            }
        }



        TextHeader textheader;
        public long StartOffset;
        public long EndOffset;
        public long ExtraSize;


        CharNode MainCharNode;
        byte[] CharInfoBlock;
        ushort[] allTextStartoffset;

        public IStream TextStreamFile;
        public Entry AssetEntry;

        long _textHeaderPosition;
        int _charsMapRelOffset;
        int _charInfoRelOffset;

        public static List<string> EditWarnings = new List<string>();

        byte[] _originalCharMap;
        Dictionary<char, string> _originalCharCode;
        long _originalTextBlockSize;

        public TextAsset(IStream stream, Entry assetentry) : base(stream)
        {
            AssetEntry = assetentry;


            TextStreamFile = stream;

            _textHeaderPosition = stream.GetPosition();
            textheader = stream.GetStructureValues<TextHeader>();

            StartOffset = (stream.GetPosition() - 8) + textheader.StartOffset;//block Start

            _charsMapRelOffset = (int)(StartOffset - (BlockStartOffset + 32));
            _charInfoRelOffset = (int)(StartOffset - (BlockStartOffset + 48));

            stream.Seek(StartOffset);

            CharNode charNode = new CharNode();
            charNode.Stream = new MStream(stream.GetBytes((int)(textheader.CharsMapOffset - (StartOffset - (BlockStartOffset + 32)))));
            _originalCharMap = ((MStream)charNode.Stream).ToArray();
            MainCharNode = charNode.ReadNodeValue(0);

            _originalCharCode = new Dictionary<char, string>();
            ExtractCharCode(MainCharNode, "", _originalCharCode);


            CharInfoBlock = stream.GetBytes((int)(textheader.CharInfoOffset - ((StartOffset - (BlockStartOffset + 48)) + charNode.Stream.GetSize())));
            allTextStartoffset = stream.GetArray<ushort>(textheader.TextCount);

            SkipPadding();

            EndOffset = stream.GetPosition();
            _originalTextBlockSize = EndOffset - StartOffset;

        }


        public string[] GetStringFormFile()
        {
            byte[] temp = CharInfoBlock;
            int sheft = 0;

            Func<CharNode> getCharacterInfo = () =>
            {
                CharNode node = MainCharNode;

                if (node.Item != 0xA74)
                {
                    return node;
                }

                do
                {
                    if (sheft >= 8)
                    {
                        Array.Copy(temp, 1, temp, 0, temp.Length - 1);
                        sheft -= 8;
                    }

                    byte currchar = (byte)(temp[0] >> sheft++);

                    node = (currchar & 1) != 0 ? node.GetLeftCharNode() : node.GetRightCharNode();

                } while (node.Item == 0xA74);

                return node;
            };

            List<string> strings = new List<string>();

            for (int i = 0; i < textheader.TextCount; i++)
            {
                sheft = 0;

                StringBuilder id = new StringBuilder(50);
                CharNode charinfo;

                do
                {
                    charinfo = getCharacterInfo();

                    char charItem = (char)charinfo.Item;
                    string textcode = CodesDictionary.TryGetValue(charItem, out var code) ? code : charItem.ToString();
                    id.Append(textcode);

                } while (charinfo.Item != 0);

                StringBuilder text = new StringBuilder(50);
                charinfo = null;

                do
                {
                    charinfo = getCharacterInfo();

                    char charItem = (char)charinfo.Item;
                    string textcode = CodesDictionary.TryGetValue(charItem, out var code) ? code : charItem.ToString();
                    text.Append(textcode);

                } while (charinfo.Item != 0);

                strings.Add(string.Concat(id.ToString().TrimEnd('\0'), "=", text.ToString().TrimEnd('\0')));

                if (sheft > 0)
                {
                    sheft = 0;
                    Array.Copy(temp, 1, temp, 0, temp.Length - 1);
                }
            }

            return strings.ToArray();
        }



        //repack


        Dictionary<char, string> CharCode;
        int Id = 0;
        string GetPath(ushort id)
        {
            Id += 1;
            return new string(Convert.ToString(Id, 2).Reverse().ToArray());
        }


        private CharNode CreateMap(string Text)
        {
            CharCode = new Dictionary<char, string>();

            HashSet<char> chars = new HashSet<char>();
            foreach (char c in Text)
            {
                chars.Add(c);

            }
            chars.Add('\0');

            //علي النعمه ما عارف ايه لزمة ام الخريطة دي
            Id = chars.Count - 1;


            chars = chars.OrderBy(x => x).ToHashSet();

            const ushort PointerValue = 0xA74;
            CharNode MainCharNode = new CharNode
            {
                Item = PointerValue
            };

            foreach (char _char in chars)
            {
            GetCode:
                string BinNum = GetPath(_char);






                //char[] strbytes = BinNum.ToCharArray();
                //Array.Reverse(strbytes);
                //BinNum = new string(strbytes);

                //  Console.WriteLine(_char + " -> " + (int)_char + " ->" + BinNum);

                //  CharacterMap;
                CharNode Temp = MainCharNode;
                foreach (char num in BinNum)
                {

                    if (Temp.Item != PointerValue)
                    {
                        goto GetCode;
                       // throw new Exception("INVALID MAP |" + Temp.Item + "->" + CharCode[(char)Temp.Item] + "| -> " + _char + " -> " + (int)_char + " ->" + BinNum);
                    }


                    if (num == '0')
                    {

                        if (Temp.Right == null)
                        {
                            Temp.Right = new CharNode
                            {
                                Item = PointerValue
                            };
                        }
                        Temp = Temp.Right;


                    }
                    else
                    {
                        if (Temp.Left == null)
                        {
                            Temp.Left = new CharNode
                            {
                                Item = PointerValue
                            };

                        }
                        Temp = Temp.Left;
                    }

                }

                Temp.Item = _char;
                CharCode.Add(_char, BinNum);
            }

            return MainCharNode;
        }


        private byte[] CreateCharMap(string text)
        {
            CharNode root = CreateMap(text);
            if (root == null) return new byte[0];

            Action<CharNode> fillTree = null;
            fillTree = (n) =>
            {
                if (n == null || n.Item != 0xA74) return;
                if (n.Right == null) n.Right = new CharNode { Item = 0 };
                else fillTree(n.Right);
                if (n.Left == null) n.Left = new CharNode { Item = 0 };
                else fillTree(n.Left);
            };
            fillTree(root);

            List<CharNode> nodes = new List<CharNode>();
            Action<CharNode> collect = null;
            collect = (n) =>
            {
                if (n == null) return;
                nodes.Add(n);
                collect(n.Right);
                collect(n.Left);
            };
            collect(root);

            Dictionary<CharNode, int> pos = new Dictionary<CharNode, int>();
            for (int i = 0; i < nodes.Count; i++)
                pos[nodes[i]] = i * 12;

            MStream ms = new MStream();
            foreach (CharNode n in nodes)
            {
                if (n.Item != 0xA74)
                {
                    ms.SetIntValue(0);
                    ms.SetIntValue(0);
                    ms.SetUShortValue(n.Item);
                    ms.SetUShortValue(0xBAAC);
                }
                else
                {
                    ms.SetIntValue(pos[n.Right] - pos[n]);
                    ms.SetIntValue(pos[n.Left] - pos[n] - 4);
                    ms.SetUShortValue(n.Item);
                    ms.SetUShortValue(0xBAAC);
                }
            }
            return ms.ToArray();
        }
        ///////////////
        public (byte[], List<ushort>) ConvertTextToInfoTable(string Strings)
        {
            List<ushort> Offsets = new List<ushort>();
            StringBuilder tempBuilder = new StringBuilder();

            foreach (string str1 in Strings.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(x => x.Contains("=")))
            {
                if ((tempBuilder.Length / 8) > ushort.MaxValue)
                {
                    throw new Exception("Text is too long");
                }

                Offsets.Add((ushort)(tempBuilder.Length / 8));


                string[] Line = str1.Split(new[] { '=' }, 2);
                Line[0] += '\0';
                Line[1] += '\0';

                if (!string.IsNullOrEmpty(Line[0]))
                {
                    foreach (char ch in Line[0])
                    {
                        tempBuilder.Append(CharCode[ch]);
                    }
                }

                if (!string.IsNullOrEmpty(Line[1]))
                {
                    foreach (char ch in Line[1])
                    {
                        tempBuilder.Append(CharCode[ch]);
                    }
                }

                int padding = (8 - (tempBuilder.Length % 8));
                if (padding != 8)
                {
                    tempBuilder.Append('0', padding);
                }
            }

            string temp = tempBuilder.ToString();
            byte[] byteArray = new byte[temp.Length / 8];

            for (int i = 0; i < byteArray.Length; i++)
            {
                int sheft = 0;
                for (int j = 0; j < 8; j++)
                {
                    byteArray[i] |= (byte)(((byte)(temp[i * 8 + j] == '1' ? 1 : 0)) << sheft++);
                }
            }


            tempBuilder.Clear();
            tempBuilder = null;
            temp = null;

            return (byteArray, Offsets);
        }







        private void ExtractCharCode(CharNode node, string prefix, Dictionary<char, string> dict)
        {
            if (node.Item != 0xA74)
            {
                dict[(char)node.Item] = prefix;
                return;
            }
            var right = node.GetRightCharNode();
            if (right != null)
                ExtractCharCode(right, prefix + "0", dict);
            var left = node.GetLeftCharNode();
            if (left != null)
                ExtractCharCode(left, prefix + "1", dict);
        }

        private string TransliterateText(string text)
        {
            var sb = new StringBuilder(text.Length);
            bool changed = false;
            foreach (char c in text)
            {
                if (_originalCharCode.ContainsKey(c))
                {
                    sb.Append(c);
                    continue;
                }

                if (TransliterationMap.TryGetValue(c, out var mapped))
                {
                    sb.Append(mapped);
                    changed = true;
                    continue;
                }

                string kd = c.ToString().Normalize(NormalizationForm.FormKD);
                if (kd[0] != c)
                {
                    var decompSb = new StringBuilder(kd.Length);
                    foreach (char d in kd)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(d) != UnicodeCategory.NonSpacingMark)
                            decompSb.Append(d);
                    }
                    string result = decompSb.ToString();
                    if (result.Length > 0)
                    {
                        sb.Append(result);
                        changed = true;
                        continue;
                    }
                }

                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                {
                    changed = true;
                    continue;
                }

                if (char.IsLetter(c))
                {
                    char otherCase = char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c);
                    if (_originalCharCode.ContainsKey(otherCase))
                    {
                        sb.Append(otherCase);
                        changed = true;
                        continue;
                    }
                }

                sb.Append(c);
            }
            return changed ? sb.ToString() : text;
        }

        private void ExtendTree(char[] newChars)
        {
            var ms = new MStream(_originalCharMap);
            var nodes = new List<(int rightOff, int leftOff, ushort item, ushort magic)>();
            while (ms.GetPosition() < ms.GetSize())
            {
                nodes.Add((ms.GetIntValue(), ms.GetIntValue(), ms.GetUShortValue(), ms.GetUShortValue()));
            }

            foreach (char newChar in newChars)
            {
                if (_originalCharCode.ContainsKey(newChar)) continue;
                int leafIdx = -1;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].item != 0xA74) { leafIdx = i; break; }
                }
                if (leafIdx == -1) break;

                ushort originalChar = nodes[leafIdx].item;

                nodes.Add((0, 0, originalChar, (ushort)0xBAAC));
                nodes.Add((0, 0, (ushort)newChar, (ushort)0xBAAC));

                int rightOff = ((nodes.Count - 2) - leafIdx) * 12;
                int leftOff = ((nodes.Count - 1) - leafIdx) * 12 - 4;

                nodes[leafIdx] = (rightOff, leftOff, 0xA74, 0xBAAC);
            }

            var outMs = new MStream();
            foreach (var n in nodes)
            {
                outMs.SetIntValue(n.rightOff);
                outMs.SetIntValue(n.leftOff);
                outMs.SetUShortValue(n.item);
                outMs.SetUShortValue(n.magic);
            }

            _originalCharMap = outMs.ToArray();

            _originalCharCode.Clear();
            var rootNode = new CharNode();
            rootNode.Stream = new MStream(_originalCharMap);
            var root = rootNode.ReadNodeValue(0);
            ExtractCharCode(root, "", _originalCharCode);
        }

        public void EditFile(string Strings)
        {
            EditWarnings.Clear();
            foreach (var charinfo in CodesDictionary)
            {
                Strings = Strings.Replace(charinfo.Value, charinfo.Key + "");
            }

            Strings = TransliterateText(Strings);

            var lines = Strings.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(x => x.Contains("="));
            var allEncodedChars = string.Concat(lines.SelectMany(l =>
            {
                var parts = l.Split(new[] { '=' }, 2);
                return (parts[0] ?? "") + "\0" + (parts[1] ?? "") + "\0";
            }));
            var missingChars = allEncodedChars.Where(c => !_originalCharCode.ContainsKey(c)).Distinct().ToList();
            if (missingChars.Any())
            {
                ExtendTree(missingChars.ToArray());
                var stillMissing = allEncodedChars.Where(c => !_originalCharCode.ContainsKey(c)).Distinct().ToList();
                if (stillMissing.Any())
                    throw new Exception("Unsupported characters in text: " + string.Join(", ", stillMissing.Select(c => $"'{c}' (0x{(int)c:X4})")));
            }

            CharCode = _originalCharCode;
            (var TextInfo, var TextOffsetMap) = ConvertTextToInfoTable(Strings);

            int newBlockSize = _originalCharMap.Length + TextInfo.Length + TextOffsetMap.Count * 2 + 15;
            newBlockSize -= newBlockSize % 16;
            if (newBlockSize > _originalTextBlockSize)
            {
                long pct = (newBlockSize - _originalTextBlockSize) * 100 / _originalTextBlockSize;
                EditWarnings.Add("Encoded text block grew from " + _originalTextBlockSize + " to " + newBlockSize + " bytes (+" + pct + "%). Game might crash if text exceeds engine buffer.");
            }

            textheader.CharsMapOffset = _charsMapRelOffset + _originalCharMap.Length;
            textheader.CharInfoOffset = _charInfoRelOffset + _originalCharMap.Length + TextInfo.Length;
            textheader.TextCount = TextOffsetMap.Count;
            textheader.TextCount_1 = TextOffsetMap.Count;
            textheader.Val1 = textheader.CharsMapOffset - 32;
            textheader.Val2 = TextInfo.Length;
            textheader.Val3 = TextInfo.Length;

            MStream mStream = new MStream();

            mStream.SetBytes(_originalCharMap);
            mStream.SetBytes(TextInfo);
            foreach (ushort s in TextOffsetMap)
                mStream.SetUShortValue(s);

            if ((mStream.GetPosition() % 16) != 0)
                mStream.SetBytes(new byte[16 - (mStream.GetPosition() % 16)]);

            TextStreamFile.Seek(StartOffset);
            TextStreamFile.DeleteBytes((int)(EndOffset - StartOffset));
            TextStreamFile.InsertBytes(mStream.ToArray());

            long End = TextStreamFile.GetPosition();

            TextStreamFile.Seek(_textHeaderPosition);
            TextStreamFile.SetStructureValus(textheader);
            TextStreamFile.Seek(End);

            ExtraSize = End - EndOffset;

            AssetEntry.UpdateOffsets(ExtraSize);
        }









    }
}
