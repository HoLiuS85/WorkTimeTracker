using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WorkTimeTracker
{
    internal class WorkdayHandler
	{
		private static int iWorkDuration;
        private static DateTime DayStartTime;
        private static List<Break> lBreaks;
        private static Thread tWorkTimeCalculation;
        
		private static TimeSpan CalcDayElapsedTime()
		{
			DateTime now = DateTime.Now;
			foreach (Break lBreak in lBreaks)
			{
				if ((!lBreak.isEnabled || !(DateTime.Now.TimeOfDay > lBreak.dtStartTime.TimeOfDay) || !(Config.dtStartTime.TimeOfDay < lBreak.dtStartTime.TimeOfDay) ? false : Config.dtEndTime.TimeOfDay > lBreak.dtStartTime.TimeOfDay))
				{
					now = now.Subtract(lBreak.tsDuration);
				}
			}
			return now - Config.dtStartTime;
		}

		private static DateTime CalcDayEndTime()
		{
			DateTime dateTime = DayStartTime.AddMinutes((double)iWorkDuration);
			foreach (Break lBreak in lBreaks)
			{
				if ((!lBreak.isEnabled || !(DayStartTime.TimeOfDay < lBreak.dtStartTime.TimeOfDay) ? false : dateTime.TimeOfDay >= lBreak.dtStartTime.TimeOfDay))
				{
					dateTime = dateTime.Add(lBreak.tsDuration);
				}
			}
			return dateTime;
		}

		private static void CalcDayWorktime()
		{
			while (true)
			{
				Config.dtStartTime = DayStartTime;
				Config.dtEndTime = CalcDayEndTime();
				Config.tsElapsed = CalcDayElapsedTime();
				Config.tsRemaining = Config.dtEndTime - DateTime.Now;
				Thread.Sleep(Config.iCalcInterval);
			}
		}

		public static bool isWorkdayStarted()
		{
			bool flag;
			foreach (Day lDay in Config.lDays)
			{
				if (lDay.dtEndTime == DateTime.MinValue)
				{
					flag = true;
					return flag;
				}
			}
			flag = false;
			return flag;
		}

		public static void WorkdayCalculationStop()
		{
			if (tWorkTimeCalculation != null)
                tWorkTimeCalculation.Abort();
		}

		public static void WorkdayEnd(DateTime EndTime)
		{
			Config.lDays.LastOrDefault().dtEndTime = EndTime;
			Config.dtStartTime = DateTime.MinValue;
			Config.dtEndTime = DateTime.MinValue;
			Config.tsRemaining = TimeSpan.Zero;
			Config.tsElapsed = TimeSpan.Zero;
            tWorkTimeCalculation.Abort();
			Config.Export();
		}

		public static int WorkdayPercent()
		{
			int num;
			try
			{
				double totalMinutes = Config.tsElapsed.TotalMinutes;
				TimeSpan timeSpan = Config.tsElapsed.Add(Config.tsRemaining);
				int num1 = Convert.ToInt32(totalMinutes / timeSpan.TotalMinutes * 100);
				num = (num1 < 100 ? num1 : 100);
			}
			catch
			{
				num = 0;
			}
			return num;
		}

		public static void WorkdayStart(int WorkingMinutes, List<Break> Breaks, DateTime StartTime)
		{
			iWorkDuration = WorkingMinutes;
			lBreaks = Breaks;
			DayStartTime = StartTime;
			if (!isWorkdayStarted())
			{
				Config.lDays.Add(new Day(Config.lDays.Count + 1, StartTime, DateTime.MinValue));
			}
			if ((tWorkTimeCalculation == null ? true : tWorkTimeCalculation.ThreadState != ThreadState.Running))
			{
				tWorkTimeCalculation = new Thread(() => CalcDayWorktime());
				tWorkTimeCalculation.Start();
			}
			Config.Export();
		}
	}
}