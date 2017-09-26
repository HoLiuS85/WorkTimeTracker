using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace WorkTimeTracker
{
	public static class Aero
	{
		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Aero.MARGINS margins);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern bool DwmIsCompositionEnabled();

        public static void enable(int margins, Window window)
		{
            IntPtr asd = new WindowInteropHelper(window).Handle;
            if (Aero.DwmIsCompositionEnabled())
			{
				MARGINS margin = new MARGINS()
				{
					Top = margins,
					Left = margins,
					Bottom = margins,
					Right = margins
                };
				Aero.DwmExtendFrameIntoClientArea(asd, ref margin);
			}
		}

		public struct MARGINS
		{
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}
	}
}