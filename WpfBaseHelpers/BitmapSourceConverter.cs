using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AR.WPF.Graphics
{
    public static class BitmapSourceConvert
    {
        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert Bitmap to a WPF BitmapSource. 
        /// </summary>
        /// <param name="image">Source bitmap</param>
        /// <returns>BitmapSource</returns>
        public static BitmapSource ToBitmapSource(Bitmap image)
        {
            using (var source = image)
            {
                IntPtr ptr = source.GetHbitmap(); 
                BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ptr); 
                return bs;
            }
        }
    }
}
