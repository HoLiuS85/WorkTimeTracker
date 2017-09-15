using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WorkTimeTracker
{
	public static class Aero
	{
		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Aero.MARGINS margins);

		[DllImport("dwmapi.dll", CharSet=CharSet.None, ExactSpelling=false, PreserveSig=false)]
		public static extern bool DwmIsCompositionEnabled();

		public static void enable(int top, int left, int bottom, int right, IntPtr handle)
		{
			if (Aero.DwmIsCompositionEnabled())
			{
				MARGINS margin = new MARGINS()
				{
					Top =top,
					Left = left,
					Bottom =bottom,
					Right = right
				};
				Aero.DwmExtendFrameIntoClientArea(handle, ref margin);
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