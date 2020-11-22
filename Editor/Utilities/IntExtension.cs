using UnityEngine;

namespace MuffinDev.MultipleEditors.Utilities
{

    ///<summary>
    /// Extensions for int objects.
    ///</summary>
    public static class IntExtension
	{

        /// <summary>
        /// Adds leading 0 before the given number.
        /// NOTE: It works only with positive numbers. If you give a negative number, its absolute value will be used.
        /// </summary>
        /// <param name="_Number">Number to process.</param>
        /// <param name="_Pow">Number of 0 to eventually add.</param>
        /// <returns>Returns the given number as a string, with added leading 0s.</returns>
		public static string AddLeading0(this int _Number, int _Pow)
        {
            _Number = Mathf.Abs(_Number);

            string valueString = "";
            _Pow--;
            int compNumber = (int)Mathf.Pow(10f, _Pow);
            while(compNumber > 1)
            {
                if(_Number < compNumber)
                {
                    valueString += "0";
                }
                _Pow--;
                compNumber = (int)Mathf.Pow(10f, _Pow);
            }
            return valueString + _Number;
        }

        /// <summary>
        /// Compares a number to another.
        /// </summary>
        /// <param name="_Number">The number to compare.</param>
        /// <param name="_Other">The number to compare with.</param>
        /// <param name="_Desc">If true, compares the number in a descending order.</param>
        /// <returns>Returns -1 if the number should be placed before the other, 1 if it should be placed after, or 0 if the numbers are
        /// even.</returns>
        public static int CompareTo(this int _Number, int _Other, bool _Desc)
        {
            if(_Desc)
            {
                if (_Number == _Other)
                    return 0;

                return (_Number < _Other) ? 1 : -1;
            }
            else
            {
                return _Number.CompareTo(_Other);
            }
        }

	}

}