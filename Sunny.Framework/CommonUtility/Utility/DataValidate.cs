using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtility.Utility
{
    public class DataValidate
    {
        #region 常量
        public const string const_Num = @"^[0-9]*$";
        public const string const_Int = @"^[1-9]\d*\.?[0]*$";
        public const string const_Email = @"^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$";
        public const string const_Url = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$";

        #endregion

        #region 数据验证
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="number">数字字符</param>
        /// <returns></returns>
        static public bool IsNum(string number)
        {
            return Regex.IsMatch(number, const_Num, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="number">数字字符</param>
        /// <param name="precision">小数位</param>
        /// <returns></returns>
        static public bool IsNum(string number, int precision)
        {
            string strReg = @"^\d{0," + precision + "}";
            return Regex.IsMatch(number, strReg, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否Int
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static public bool IsInt(string val)
        {
            return Regex.IsMatch(val, const_Int);
        }
        #endregion

        #region 格式验证
        /// <summary>
        /// 是否邮件格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        static public bool IsEmail(string email)
        {
            return Regex.IsMatch(email, const_Email, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否Url格式
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsURL(string url)
        {
            return Regex.IsMatch(url, const_Url);
        }
        #endregion
    }

    public enum EValidType
    {
        String,
        Int,
        Num,
        Cur,
        Date,
        Bool,
    }

    /// <summary>
    /// 验证规则
    /// </summary>
    public enum EValidRule
    {
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 货币
        /// </summary>
        Currency,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 仅字母
        /// </summary>
        OnlyLetter,
        /// <summary>
        /// 邮件
        /// </summary>
        Email,
        /// <summary>
        /// 电话号码
        /// </summary>
        Telephone,
    }

    public abstract class Validator
    {
        private EValidType? _DataType;
        public EValidType? DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        private bool _Required;
        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required
        {
            get { return _Required; }
            set { _Required = value; }
        }
    }

    public class Validator_String : Validator
    {
        private int? _Min;
        public int? Min
        {
            get { return _Min; }
            set { _Min = value; }
        }

        private int? _Max;
        public int? Max
        {
            get { return _Max; }
            set { _Max = value; }
        }

        private EValidRule? _CtrValidRule;
        /// <summary>
        /// 规则验证
        /// </summary>
        public EValidRule? CtrValidRule
        {
            get { return _CtrValidRule; }
            set { _CtrValidRule = value; }
        }
    }

    public class Validator_Number : Validator
    {
        private decimal? _Min;
        public decimal? Min
        {
            get { return _Min; }
            set { _Min = value; }
        }
        private decimal? _Max;
        public decimal? Max
        {
            get { return _Max; }
            set { _Max = value; }
        }
        private Int16? _Precision;
        public Int16? Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }
    }

    public class Validator_Bool : Validator
    {

    }
}
