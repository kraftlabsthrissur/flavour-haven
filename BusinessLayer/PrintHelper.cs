using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PrintHelper
    {
        public static int TotalLines { get; set; }
        public static int BodyLines { get; set; }
        public static int TotalPages { get; set; }
        public static int CurrentPages { get; set; }

        public enum CharAlignment
        {
            AlignLeft,
            AlignCenter,
            AlignRight,
        }

        public void SetPageVariables()
        {
            if (PrintHelper.BodyLines == 0)
                throw new Exception("Body length for the Print Invoice is not Set.\nPlease set its value in Parameters");
            PrintHelper.TotalPages = PrintHelper.TotalLines / PrintHelper.BodyLines;
            PrintHelper.CurrentPages = 1;
            if (PrintHelper.TotalLines % PrintHelper.BodyLines == 0)
                return;
            ++PrintHelper.TotalPages;
        }

        public string AlignText(string vText, CharAlignment vAlign, int vCharLength)
        {
            string str = "";
            try
            {
                int totalWidth1 = vCharLength >= 0 ? vCharLength : 0;
                switch (vAlign)
                {
                    case CharAlignment.AlignLeft:
                        str = vText.PadRight(totalWidth1, ' ');
                        str = str.Substring(0, vCharLength);
                        break;
                    case CharAlignment.AlignCenter:
                        int totalWidth2 = (vCharLength - vText.Length) / 2;
                        if (totalWidth2 <= 0)
                            totalWidth2 = 0;
                        str = str.PadLeft(totalWidth2, ' ') + vText + str.PadLeft(vCharLength, ' ');
                        str = str.Substring(0, vCharLength);
                        break;
                    case CharAlignment.AlignRight:
                        str = vText.PadLeft(totalWidth1, ' ');
                        str = str.Substring(str.Length - vCharLength);
                        break;
                }
                return str;
            }
            catch
            {
                return str;
            }
        }

        public string Condense(string vText)
        {
            string CondencedOn = Convert.ToChar(15).ToString();
            string CondencedOff = Convert.ToChar(18).ToString();
            return CondencedOn + vText + CondencedOff;
        }

        public string Bold(string vText)
        {
            char ch6 = Convert.ToChar(27);
            string str13 = ch6.ToString();
            ch6 = Convert.ToChar(69);
            string str14 = ch6.ToString();
            string BoldOn = str13 + str14;
            char ch7 = Convert.ToChar(27);
            string str15 = ch7.ToString();
            ch7 = Convert.ToChar(70);
            string str16 = ch7.ToString();
            string BoldOff = str15 + str16;
            return BoldOn + vText + BoldOff;
        }

        public string FillChar(char vText, int vCount)
        {
            string str = string.Empty;
            for (int index = 1; index <= vCount; ++index)
                str += vText;
            return str;
        }

        public string Pica(string vText)
        {
            string Pica = Convert.ToChar(27).ToString() + Convert.ToChar(80).ToString();
            return Pica + vText;
        }

        public string SplitString(string vText, int limit)
        {
            string sentence = vText;
            string[] words = sentence.Split(' ');
            StringBuilder newSentence = new StringBuilder();
            string line = "";
            foreach (string word in words)
            {
                if ((line + word).Length > limit)
                {
                    newSentence.AppendLine(line);
                    line = "";
                }
                line += string.Format("{0} ", word);
            }
            if (line.Length > 0)
                newSentence.AppendLine(line);
            return newSentence.Replace('\n', ' ').ToString();
        }

        public string FormFeed()
        {
            string str = string.Empty;
            char ch12 = Convert.ToChar(12);
            string str12 = ch12.ToString();
            str = str12;
            return str;
        }

        public string ForwardLineFeed()
        {
            string str = string.Empty;
            char ch10 = Convert.ToChar(10);
            string str10 = ch10.ToString();
            char ch27 = Convert.ToChar(27);
            string str27 = ch27.ToString();
            char ch50 = Convert.ToChar(50);
            string str50 = ch50.ToString();
            string LineSpacing1_6 = str27 + str50;
            string FLineFeed = str10;
            str = LineSpacing1_6 + " " + FLineFeed + " ";
            return str;
        }

        public string ReverseLineFeed()
        {
            string str = string.Empty;
            char ch27 = Convert.ToChar(27);
            string str27 = ch27.ToString();
            char ch106 = Convert.ToChar(106);
            string str106 = ch106.ToString();
            char ch50 = Convert.ToChar(50);
            string str50 = ch50.ToString();
            string LineSpacing1_6 = str27 + str50;
            string RLineFeed = str27 + str106;
            str = LineSpacing1_6 + " " + RLineFeed + " ";
            return str;
        }

        public string LineSpacing1_6()
        {
            string str = string.Empty;
            char ch27 = Convert.ToChar(27);
            string str27 = ch27.ToString();
            char ch50 = Convert.ToChar(50);
            string str50 = ch50.ToString();
            string LineSpacing1_6 = str27 + str50;
            str = LineSpacing1_6 + " ";
            return str;
        }
    }
}
