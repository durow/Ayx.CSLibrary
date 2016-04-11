using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

namespace Ayx.CSLibrary.Utility
{
    public class ScreenShot
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 获取当前所有屏幕
        /// </summary>
        public IEnumerable<Screen> Screens => Screen.AllScreens;

        /// <summary>
        /// 将屏幕截图保存为图像
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="filename"></param>
        public void SaveScreenToFile(Screen screen, string filename)
        {
            var bmp = GetScreenBitmap(screen);
            bmp.Save(filename);
        }

        /// <summary>
        /// 获取屏幕为Bitmap
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public static Bitmap GetScreenBitmap(Screen screen)
        {
            if (screen == null) return null;
            var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
            try
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(
                        screen.Bounds.Left,
                        screen.Bounds.Top,
                        0, 0,
                        new Size(screen.Bounds.Width, screen.Bounds.Height));
                }
                return bmp;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Bitmap转为BitmapSource
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static BitmapSource BitmapToBitmapSource(Bitmap bmp)
        {
            BitmapSource result;
            IntPtr hBmp = bmp.GetHbitmap();
            try
            {
                result = Imaging.
                    CreateBitmapSourceFromHBitmap(
                        hBmp,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                result = null;
            }
            finally
            {
                DeleteObject(hBmp);
            }
            return result;
        }
    }
}
