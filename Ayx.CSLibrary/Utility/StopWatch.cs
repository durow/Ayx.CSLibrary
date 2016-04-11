using System;

namespace Ayx.CSLibrary.Utility
{
    public class StopWatch
    {
        /// <summary>
        /// 获取当前秒表的起始时间
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// 创建一个秒表实例并将起始时间设置为当前时间
        /// </summary>
        public StopWatch()
        {
            StartDateTime = DateTime.Now;
        }

        /// <summary>
        /// 创建一个秒表实例并将参数设置为起始时间
        /// </summary>
        /// <param name="startDateTime">起始时间</param>
        public StopWatch(DateTime startDateTime)
        {
            StartDateTime = startDateTime;
        }

        /// <summary>
        /// 获取当前与起始时间的时间差
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeSpan()
        {
            return DateTime.Now - StartDateTime;
        }

        /// <summary>
        /// 重置秒表起始时间为当前时间
        /// </summary>
        public void Reset()
        {
            StartDateTime = DateTime.Now;
        }

        /// <summary>
        /// 重置秒表起始时间为startDateTime
        /// </summary>
        /// <param name="startDateTime">重置的时间</param>
        public void Reset(DateTime startDateTime)
        {
            StartDateTime = startDateTime;
        }
    }
}
