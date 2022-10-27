﻿using System;

namespace PersonalWebsite.Helper
{
    /// <summary>
    /// Double类型处理扩展类
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// 数字简化，把10000简化成1w 格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Simple(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            double temp = Convert.ToDouble(str);
            string result = temp > 10000 ? $"{(temp / 10000).ToString2()}w" : temp.ToString();
            return result;
        }


        /// <summary>
        /// 返回两位小数格式，如果是0 返回空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString2(this double value)
        {
            if (value == 0d) return "";
            string s = value.ToDouble2().ToString("0.00");
            if (s == "0.00") return "";
            else return s;
        }

        /// <summary>
        /// 返回两位小数格式 ,四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble2(this double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 变成万元
        /// </summary>
        /// <param name="value"></param>
        /// <returns>两位小数格式 ,四舍五入</returns>
        public static double ToTenThousand(this double value)
        {
            return (value / 10000d).ToDouble2();
        }
        /// <summary>
        /// 由万元变成元
        /// </summary>
        /// <param name="value"></param>
        /// <returns>两位小数格式 ,四舍五入</returns>
        public static double FromTenThousand(this double value)
        {
            return (value * 10000d).ToDouble2();
        }

        /// <summary>
        /// 尝试将Object类型转换为Double类型
        /// </summary>
        /// <param name="value">Object格式的数值</param>
        /// <returns></returns>
        public static double ParseToDouble(this object value)
        {
            double result;

            if (!double.TryParse(value.ToString(), out result))
                return 0;
            else
                return result;
        }


        /// <summary>
        /// 将Double型转换为中文金额数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertToChineseNumber(this double input)
        {
            string str = input.ToString();

            if (!IsPositveDecimal(str))
                return "输入的不是正数字！";
            if (input > 999999999999.99)
                return "数字太大，请输入一万亿元以下的金额";

            char[] ch = new char[1];
            ch[0] = '.'; //小数点
            string[] splitstr = null; //定义按小数点分割后的字符串数组
            splitstr = str.Split(ch[0]);//按小数点分割字符串

            if (splitstr.Length == 1) //只有整数部分
                return ConvertData(str) + "圆整";
            else //有小数部分
            {
                string rstr;
                rstr = ConvertData(splitstr[0]) + "圆";//转换整数部分
                rstr += ConvertXiaoShu(splitstr[1]);//转换小数部分
                return rstr;
            }

        }


        /// 判断是否是正数字字符串
        /// 如果是数字，返回true，否则返回false
        private static bool IsPositveDecimal(string str)
        {
            Decimal d;
            try
            {
                d = Decimal.Parse(str);

            }
            catch (Exception)
            {
                return false;
            }
            if (d > 0)
                return true;
            else
                return false;
        }


        /// 转换数字（整数）
        /// 需要转换的整数数字字符串
        /// 转换成中文大写后的字符串
        private static string ConvertData(string str)
        {
            string tmpstr = "";
            string rstr = "";
            int strlen = str.Length;
            if (strlen <= 4)//数字长度小于四位
            {
                rstr = ConvertDigit(str);

            }
            else
            {

                if (strlen <= 8)//数字长度大于四位，小于八位
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字
                    tmpstr = str.Substring(0, strlen - 4);//截取其余数字
                                                          //将两次转换的数字加上萬后相连接
                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                    rstr = rstr.Replace("零萬", "萬");
                    rstr = rstr.Replace("零零", "零");

                }
                else
                if (strlen <= 12)//数字长度大于八位，小于十二位
                {
                    tmpstr = str.Substring(strlen - 4, 4);//先截取最后四位数字
                    rstr = ConvertDigit(tmpstr);//转换最后四位数字
                    tmpstr = str.Substring(strlen - 8, 4);//再截取四位数字
                    rstr = String.Concat(ConvertDigit(tmpstr) + "萬", rstr);
                    tmpstr = str.Substring(0, strlen - 8);
                    rstr = String.Concat(ConvertDigit(tmpstr) + "億", rstr);
                    rstr = rstr.Replace("零億", "億");
                    rstr = rstr.Replace("零萬", "零");
                    rstr = rstr.Replace("零零", "零");
                    rstr = rstr.Replace("零零", "零");
                }
            }
            strlen = rstr.Length;
            if (strlen >= 2)
            {
                switch (rstr.Substring(strlen - 2, 2))
                {
                    case "佰零": rstr = rstr.Substring(0, strlen - 2) + "佰"; break;
                    case "仟零": rstr = rstr.Substring(0, strlen - 2) + "仟"; break;
                    case "萬零": rstr = rstr.Substring(0, strlen - 2) + "萬"; break;
                    case "億零": rstr = rstr.Substring(0, strlen - 2) + "億"; break;

                }
            }

            return rstr;
        }

        /// 转换数字（小数部分）
        /// 需要转换的小数部分数字字符串
        /// 转换成中文大写后的字符串
        private static string ConvertXiaoShu(string str)
        {
            int strlen = str.Length;
            string rstr;
            if (strlen == 1)
            {
                rstr = ConvertChinese(str) + "角";
                return rstr;
            }
            else
            {
                string tmpstr = str.Substring(0, 1);
                rstr = ConvertChinese(tmpstr) + "角";
                tmpstr = str.Substring(1, 1);
                rstr += ConvertChinese(tmpstr) + "分";
                rstr = rstr.Replace("零分", "");
                rstr = rstr.Replace("零角", "");
                return rstr;
            }


        }

        /// 转换数字
        /// 转换的字符串（四位以内）
        private static string ConvertDigit(string str)
        {
            int strlen = str.Length;
            string rstr = "";
            switch (strlen)
            {
                case 1: rstr = ConvertChinese(str); break;
                case 2: rstr = Convert2Digit(str); break;
                case 3: rstr = Convert3Digit(str); break;
                case 4: rstr = Convert4Digit(str); break;
            }
            rstr = rstr.Replace("拾零", "拾");
            strlen = rstr.Length;

            return rstr;
        }


        /// 转换四位数字
        private static string Convert4Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string str4 = str.Substring(3, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "仟";
            rstring += ConvertChinese(str2) + "佰";
            rstring += ConvertChinese(str3) + "拾";
            rstring += ConvertChinese(str4);
            rstring = rstring.Replace("零仟", "零");
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }

        /// 转换三位数字
        private static string Convert3Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string str3 = str.Substring(2, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "佰";
            rstring += ConvertChinese(str2) + "拾";
            rstring += ConvertChinese(str3);
            rstring = rstring.Replace("零佰", "零");
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }

        /// 转换二位数字 
        private static string Convert2Digit(string str)
        {
            string str1 = str.Substring(0, 1);
            string str2 = str.Substring(1, 1);
            string rstring = "";
            rstring += ConvertChinese(str1) + "拾";
            rstring += ConvertChinese(str2);
            rstring = rstring.Replace("零拾", "零");
            rstring = rstring.Replace("零零", "零");
            return rstring;
        }


        /// 将一位数字转换成中文大写数字
        private static string ConvertChinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分"
            string cstr = "";
            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "壹"; break;
                case "2": cstr = "贰"; break;
                case "3": cstr = "叁"; break;
                case "4": cstr = "肆"; break;
                case "5": cstr = "伍"; break;
                case "6": cstr = "陆"; break;
                case "7": cstr = "柒"; break;
                case "8": cstr = "捌"; break;
                case "9": cstr = "玖"; break;
            }
            return (cstr);
        }
    }
}