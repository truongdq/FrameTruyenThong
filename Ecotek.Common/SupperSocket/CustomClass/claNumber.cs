using System;

namespace Ecotek.Common.SupperSocket
{
    public class claNumber
    {
        /// <summary>
        /// Determines whether the input string is a number or not.
        /// </summary>
        public static bool IsNumeric(string value)
        {
            if ((null == value) || (string.IsNullOrEmpty(value)) || ("" == value))
            {
                //It is the board
                return false;
            }

            int nIndex = 0;

            foreach (char cData in value)
            {
                if (false == Char.IsNumber(cData))
                {
                    //When the index is 0, '-' is a sign, so it is judged as a number.
                    if ((0 == nIndex)
                        && ('-' != cData))
                    {
                        return false;
                    }
                }

                ++nIndex;
            }

            return true;
        }

        /// <summary>
        /// Replace string with a number.
        /// </summary>
        public static int StringToInt(string sData)
        {
            if (!IsNumeric(sData))
            {
                return 0;
            }
  
            return Convert.ToInt32(sData);
        }
    }
}
