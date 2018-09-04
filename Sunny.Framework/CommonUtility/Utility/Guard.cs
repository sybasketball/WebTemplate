using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtility.Utility
{
    public static class Guard
    {
        // Methods
        public static void Check(bool result, string message)
        {
            if (!result)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, TextResource.CannotBeNull.FormatWith(new object[] { parameterName }));
            }
        }

        public static void IsNotNullOrEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty(parameter ?? string.Empty))
            {
                throw new ArgumentException(TextResource.CannotBeNullOrEmpty.FormatWith(new object[] { parameterName }));
            }
        }
    }


}
