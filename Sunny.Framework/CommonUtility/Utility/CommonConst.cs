using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtility.Utility
{
    public struct CommonConst
    {
        public struct SysFormat
        {
            public const string Format_Date = "{0:yyyy-MM-dd}";
            public const string Format_DateTime = "{0:yyyy-MM-dd hh:mm:ss}";
            public const string Format_Float = "{0:F2}";
            public const string Format_Currency = "{0:C2}";
            public const string Format_Percent = "{0:P}";
        }
    }

    public struct CommonEnum
    {
        public enum EBool
        {
            False,
            True,
        }
    }
}
