/*
 * Author:durow
 * Date:2016.09.23
 * Description:Make a number based on milliseconds and a counter
 *                   Used for databse key and so on.
 */

using System;
using System.Text;

namespace Ayx.CSLibrary.Utility
{
    public class IdGenerator
    {
        private static IdGenerator _default;
        public static IdGenerator Default
        {
            get
            {
                if (_default == null)
                    _default = new IdGenerator();
                return _default;
            }
        }

        private int _counter = 0;

        public DateTime Start { get; private set; }

        public IdGenerator():this(new DateTime(2016,1,1,0,0,0))
        { }

        public IdGenerator(DateTime start)
        {
            Start = start;
        }

        /// <summary>
        /// 获得一个长度为12的ID
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return GetId(12);
        }

        /// <summary>
        /// 获得一个长度为指定length的ID，length太短会引发异常
        /// </summary>
        /// <param name="length">ID长度</param>
        /// <returns></returns>
        public string GetId(int length)
        {
            var ms = (long)(DateTime.Now - Start).TotalMilliseconds;
            var result = LongToId(ms, length);
            IncreaseCounter();
            return result;
        }

        private string LongToId(long ms, int length)
        {
            if (_counter > 15) _counter = 0;
            var result = Convert.ToString(ms, 16).ToUpper() + Convert.ToString(_counter, 16).ToUpper();

            var zeroNum = length - result.Length;
            if (zeroNum < 0)
                throw new Exception("length is too short!");

            if (zeroNum == 0) return result;

            var sb = new StringBuilder(length);
            for (int i = 0; i < zeroNum; i++)
            {
                sb.Append("0");
            }
            sb.Append(result);
            return sb.ToString();
        }

        private void IncreaseCounter()
        {
            _counter++;
            if (_counter == 16)
                _counter = 0;
        }
    }
}
