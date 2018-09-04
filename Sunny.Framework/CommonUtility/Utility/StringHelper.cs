using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Collections;

namespace CommonUtility.Utility
{
    public static class StringHelper
    {
        #region 静态方法
        /// <summary>
        /// 对字符串进行base64编码
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>base64编码串</returns>
        public static string Base64StringEncode(string input)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// 对字符串进行反编码
        /// </summary>
        /// <param name="input">base64编码串</param>
        /// <returns>字符串</returns>
        public static string Base64StringDecode(string input)
        {
            byte[] decbuff = Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        /// <summary>
        /// 替换字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string CaseInsensitiveReplace(string input, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, newValue);
        }

        /// <summary>
        /// 替换首次出现的字符串
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceFirst(string input, string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue, RegexOptions.Multiline);
            return regEx.Replace(input, newValue, 1);
        }

        /// <summary>
        /// 替换最后一次出现的字符串
        /// </summary>
        /// <param name="input">要进行替换的内容</param>
        /// <param name="oldValue">旧字符串</param>
        /// <param name="newValue">新字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceLast(string input, string oldValue, string newValue)
        {
            int index = input.LastIndexOf(oldValue);
            if (index < 0)
            {
                return input;
            }
            else
            {
                StringBuilder sb = new StringBuilder(input.Length - oldValue.Length + newValue.Length);
                sb.Append(input.Substring(0, index));
                sb.Append(newValue);
                sb.Append(input.Substring(index + oldValue.Length, input.Length - index - oldValue.Length));
                return sb.ToString();
            }
        }

        /// <summary>
        /// 根据词组过虑字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行过虑的内容</param>
        /// <param name="filterWords">要过虑的词组</param>
        /// <returns>过虑后的字符串</returns>
        public static string FilterWords(string input, params string[] filterWords)
        {
            return StringHelper.FilterWords(input, char.MinValue, filterWords);
        }

        /// <summary>
        /// 根据词组过虑字符串(忽略大小写)
        /// </summary>
        /// <param name="input">要进行过虑的内容</param>
        /// <param name="mask">字符掩码</param>
        /// <param name="filterWords">要过虑的词组</param>
        /// <returns>过虑后的字符串</returns>
        public static string FilterWords(string input, char mask, params string[] filterWords)
        {
            string stringMask = mask == char.MinValue ? string.Empty : mask.ToString();
            string totalMask = stringMask;

            foreach (string s in filterWords)
            {
                Regex regEx = new Regex(s, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (stringMask.Length > 0)
                {
                    for (int i = 1; i < s.Length; i++)
                        totalMask += stringMask;
                }

                input = regEx.Replace(input, totalMask);

                totalMask = stringMask;
            }

            return input;
        }

        public static MatchCollection HasWords(string input, params string[] hasWords)
        {
            StringBuilder sb = new StringBuilder(hasWords.Length + 50);
            //sb.Append("[");

            foreach (string s in hasWords)
            {
                sb.AppendFormat("({0})|", StringHelper.HtmlSpecialEntitiesEncode(s.Trim()));
            }

            string pattern = sb.ToString();
            pattern = pattern.TrimEnd('|'); // +"]";

            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Matches(input);
        }

        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="input">要进行编辑的字符串</param>
        /// <returns>Html编码后的字符串</returns>
        public static string HtmlSpecialEntitiesEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="input">要进行解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlSpecialEntitiesDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5String(string input)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// 对字符串进行MD5较验
        /// </summary>
        /// <param name="input">要进行较验的字符串</param>
        /// <param name="hash">散列串</param>
        /// <returns>是否匹配</returns>
        public static bool MD5VerifyString(string input, string hash)
        {
            string hashOfInput = StringHelper.MD5String(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string PadLeftHtmlSpaces(string input, int totalSpaces)
        {
            string space = "&nbsp;";
            return PadLeft(input, space, totalSpaces * space.Length);
        }

        public static string PadLeft(string input, string pad, int totalWidth)
        {
            return StringHelper.PadLeft(input, pad, totalWidth, false);
        }

        public static string PadLeft(string input, string pad, int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            int padCount = pad.Length;
            string paddedString = input;

            while (paddedString.Length < totalWidth)
            {
                paddedString += pad;
            }

            // trim the excess.
            if (cutOff)
                paddedString = paddedString.Substring(0, totalWidth);

            return paddedString;
        }

        public static string PadRightHtmlSpaces(string input, int totalSpaces)
        {
            string space = "&nbsp;";
            return PadRight(input, space, totalSpaces * space.Length);
        }

        public static string PadRight(string input, string pad, int totalWidth)
        {
            return StringHelper.PadRight(input, pad, totalWidth, false);
        }

        public static string PadRight(string input, string pad, int totalWidth, bool cutOff)
        {
            if (input.Length >= totalWidth)
                return input;

            string paddedString = string.Empty;

            while (paddedString.Length < totalWidth - input.Length)
            {
                paddedString += pad;
            }

            // trim the excess.
            if (cutOff)
                paddedString = paddedString.Substring(0, totalWidth - input.Length);

            paddedString += input;

            return paddedString;
        }

        /// <summary>
        /// 去除新行
        /// </summary>
        /// <param name="input">要去除新行的字符串</param>
        /// <returns>已经去除新行的字符串</returns>
        public static string RemoveNewLines(string input)
        {
            return StringHelper.RemoveNewLines(input, false);
        }

        /// <summary>
        /// 去除新行
        /// </summary>
        /// <param name="input">要去除新行的字符串</param>
        /// <param name="addSpace">是否添加空格</param>
        /// <returns>已经去除新行的字符串</returns>
        public static string RemoveNewLines(string input, bool addSpace)
        {
            string replace = string.Empty;
            if (addSpace)
                replace = " ";

            string pattern = @"[/r|/n]";
            Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="input">要进行反转的字符串</param>
        /// <returns>反转后的字符串</returns>
        public static string Reverse(string input)
        {
            char[] reverse = new char[input.Length];
            for (int i = 0, k = input.Length - 1; i < input.Length; i++, k--)
            {
                if (char.IsSurrogate(input[k]))
                {
                    reverse[i + 1] = input[k--];
                    reverse[i++] = input[k];
                }
                else
                {
                    reverse[i] = input[k];
                }
            }
            return new System.String(reverse);
        }

        /// <summary>
        /// 转成首字母大字形式
        /// </summary>
        /// <param name="input">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string SentenceCase(string input)
        {
            if (input.Length < 1)
                return input;

            string sentence = input.ToLower();
            return sentence[0].ToString().ToUpper() + sentence.Substring(1);
        }

        /// <summary>
        /// 空格转换成&nbsp;
        /// </summary>
        /// <param name="input">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string SpaceToNbsp(string input)
        {
            string space = "&nbsp;";
            return input.Replace(" ", space);
        }

        /// <summary>
        /// 去除"<" 和 ">" 符号之间的内容
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string StripTags(string input)
        {
            Regex stripTags = new Regex("<(.|/n)+?>");
            return stripTags.Replace(input, "");
        }

        public static string TitleCase(string input)
        {
            return TitleCase(input, true);
        }

        public static string TitleCase(string input, bool ignoreShortWords)
        {
            List<string> ignoreWords = null;
            if (ignoreShortWords)
            {
                //TODO: Add more ignore words?
                ignoreWords = new List<string>();
                ignoreWords.Add("a");
                ignoreWords.Add("is");
                ignoreWords.Add("was");
                ignoreWords.Add("the");
            }

            string[] tokens = input.Split(' ');
            StringBuilder sb = new StringBuilder(input.Length);
            foreach (string s in tokens)
            {
                if (ignoreShortWords == true
                    && s != tokens[0]
                    && ignoreWords.Contains(s.ToLower()))
                {
                    sb.Append(s + " ");
                }
                else
                {
                    sb.Append(s[0].ToString().ToUpper());
                    sb.Append(s.Substring(1).ToLower());
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// 去除字符串内的空白字符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string TrimIntraWords(string input)
        {
            Regex regEx = new Regex(@"[/s]+");
            return regEx.Replace(input, " ");
        }

        /// <summary>
        /// 换行符转换成Html标签的换行符<br />
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        public static string NewLineToBreak(string input)
        {
            Regex regEx = new Regex(@"[/n|/r]+");
            return regEx.Replace(input, "<br />");
        }

        /// <summary>
        /// 插入换行符(不中断单词)
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount)
        {
            return StringHelper.WordWrap(input, charCount, false, Environment.NewLine);
        }

        /// <summary>
        /// 插入换行符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <param name="cutOff">如果为真，将在单词的中部断开</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount, bool cutOff)
        {
            return StringHelper.WordWrap(input, charCount, cutOff, Environment.NewLine);
        }

        /// <summary>
        /// 插入换行符
        /// </summary>
        /// <param name="input">要进行处理的字符串</param>
        /// <param name="charCount">每行字符数</param>
        /// <param name="cutOff">如果为真，将在单词的中部断开</param>
        /// <param name="breakText">插入的换行符号</param>
        /// <returns>处理后的字符串</returns>
        public static string WordWrap(string input, int charCount, bool cutOff, string breakText)
        {
            StringBuilder sb = new StringBuilder(input.Length + 100);
            int counter = 0;

            if (cutOff)
            {
                while (counter < input.Length)
                {
                    if (input.Length > counter + charCount)
                    {
                        sb.Append(input.Substring(counter, charCount));
                        sb.Append(breakText);
                    }
                    else
                    {
                        sb.Append(input.Substring(counter));
                    }
                    counter += charCount;
                }
            }
            else
            {
                string[] strings = input.Split(' ');
                for (int i = 0; i < strings.Length; i++)
                {
                    counter += strings[i].Length + 1; // the added one is to represent the inclusion of the space.
                    if (i != 0 && counter > charCount)
                    {
                        sb.Append(breakText);
                        counter = 0;
                    }

                    sb.Append(strings[i] + ' ');
                }
            }
            return sb.ToString().TrimEnd(); // to get rid of the extra space at the end.
        }
        #endregion
        
        /// <summary>
        /// 倒序加1加密
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string EncryptStr(string rs) //倒序加1加密 
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] + 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
        /// <summary>
        /// 顺序减1解码 
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string DecryptStr(string rs) //顺序减1解码 
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] - 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
        /// <summary>
        /// Escape加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            if (str == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            int len = str.Length;

            for (int i = 0; i < len; i++)
            {
                char c = str[i];

                //everything other than the optionally escaped chars _must_ be escaped 
                if (Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\\' || c == '.')
                    sb.Append(c);
                else
                    sb.Append(Uri.HexEscape(c));
            }

            return sb.ToString();
        }
        /// <summary>
        /// UnEscape解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 左截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(0, len);
        }
        /// <summary>
        /// 右截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(inputString.Length - len, len);
        }
        /// <summary>
        /// 截取指定长度字符串,汉字为2个字符
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] >= 63 && (int)s[i] <= 90)
                    tempLen += 2;
                else
                    tempLen += 1;
                if (tempLen > len)
                    break;
                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }
            }
            return tempString;
        }
        #region 去除HTML标记
        ///<summary>   
        ///去除HTML标记   
        ///</summary>   
        ///<param name="NoHTML">包括HTML的源码</param>   
        ///<returns>已经去除后的文字</returns>   
        public static string NoHTML(string Htmlstring)
        {
            //Regex myReg = new Regex(@"(\<.[^\<]*\>)", RegexOptions.IgnoreCase);
            //Htmlstring = myReg.Replace(Htmlstring, "");
            //myReg = new Regex(@"(\<\/[^\<]*\>)", RegexOptions.IgnoreCase);
            //Htmlstring = myReg.Replace(Htmlstring, "");
            //return Htmlstring;

            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "”", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "&lt;");
            Htmlstring = Htmlstring.Replace(">", "&gt;");
            return Htmlstring;
        }
        #endregion
        /// <summary>
        /// 不区分大小写的替换
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="pattern">需替换字符</param>
        /// <param name="replacement">被替换内容</param>
        /// <returns></returns>
        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i) chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i) chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i) chars[count++] = original[i];
            return new string(chars, 0, count);
        }
        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("'", "&#39;");
            theString = theString.Replace("\r\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "'");
            theString = theString.Replace("<br/> ", "\r\n");
            return theString;
        }
        /// <summary>
        /// 输出单行简介
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string SimpleLineSummary(string theString)
        {
            theString = theString.Replace("&gt;", "");
            theString = theString.Replace("&lt;", "");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "'");
            theString = theString.Replace("<br/> ", "\r\n");
            theString = theString.Replace("\"", "");
            theString = theString.Replace("\t", " ");
            theString = theString.Replace("\r", " ");
            theString = theString.Replace("\n", " ");
            theString = Regex.Replace(theString, "\\s{2,}", " ");
            return theString;
        }
        /// <summary> 
        /// UBB代码处理函数 
        /// </summary> 
        /// <param name="content">输入字符串</param> 
        /// <returns>输出字符串</returns> 
        public static string UBB2HTML(string content)  //ubb转html
        {
            content = Regex.Replace(content, @"\[b\](.+?)\[/b\]", "<b>$1</b>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[i\](.+?)\[/i\]", "<i>$1</i>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[u\](.+?)\[/u\]", "<u>$1</u>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[p\](.+?)\[/p\]", "<p>$1</p>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=left\](.+?)\[/align\]", "<align='left'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=center\](.+?)\[/align\]", "<align='center'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=right\](.+?)\[/align\]", "<align='right'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[url=(?<url>.+?)]\[/url]", "<a href='${url}' target=_blank>${url}</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[url=(?<url>.+?)](?<name>.+?)\[/url]", "<a href='${url}' target=_blank>${name}</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[quote](?<text>.+?)\[/quote]", "<div class=\"quote\">${text}</div>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[img](?<img>.+?)\[/img]", "<a href='${img}' target=_blank><img src='${img}' alt=''/></a>", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 将html转成js代码,不完全和原始数据一致
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Html2Js(string source)
        {
            return String.Format("document.write(\"{0}\");",
                String.Join("\");\r\ndocument.write(\"", source.Replace("\\", "\\\\")
                                        .Replace("/", "\\/")
                                        .Replace("'", "\\'")
                                        .Replace("\"", "\\\"")
                                        .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            ));
        }
        /// <summary>
        /// 将html转成可输出的js字符串,不完全和原始数据一致
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Html2JsStr(string source)
        {
            return String.Format("{0}",
                String.Join(" ", source.Replace("\\", "\\\\")
                                        .Replace("/", "\\/")
                                        .Replace("'", "\\'")
                                        .Replace("\"", "\\\"")
                                        .Replace("\t", "")
                                        .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            ));
        }
        /// <summary>
        /// 过滤所有特殊特号
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string FilterSymbol(string theString)
        {
            string[] aryReg = { "'", "\"", "\r", "\n", "<", ">", "%", "?", ",", ".", "=", "-", "_", ";", "|", "[", "]", "&", "/" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 过滤一般特殊特号,如单引、双引和回车和分号等
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string SafetyStr(string theString)
        {
            string[] aryReg = { "'", ";", "\"", "\r", "\n" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 正则表达式取值
        /// </summary>
        /// <param name="HtmlCode">HTML代码</param>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="GroupKey">正则表达式分组关键字</param>
        /// <param name="RightToLeft">是否从右到左</param>
        /// <returns></returns>
        public static string[] GetRegValue(string HtmlCode, string RegexString, string GroupKey, bool RightToLeft)
        {
            MatchCollection m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            m = r.Matches(HtmlCode);
            string[] MatchValue = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                MatchValue[i] = m[i].Groups[GroupKey].Value;
            }
            return MatchValue;
        }
    }
}
