using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace WorkTimeTracker
{
    internal class WorkdayHandler
	{
        private static Timer tWorkTime;

		private static TimeSpan CalcDayElapsedTime()
		{
			DateTime now = DateTime.Now;
			foreach (Break lBreak in UserData.getBreaks())
			{
				if ((!lBreak.isEnabled || !(DateTime.Now.TimeOfDay > lBreak.dtStartTime.TimeOfDay) || !(UserData.getWorkTimeStart().TimeOfDay < lBreak.dtStartTime.TimeOfDay) ? false : UserData.getWorkTimeEnd().TimeOfDay > lBreak.dtStartTime.TimeOfDay))
					now = now.Subtract(lBreak.tsDuration);
			}
			return now - UserData.getWorkTimeStart();
		}

		private static DateTime CalcDayEndTime()
		{
			DateTime dateTime = UserData.getWorkTimeStart().AddMinutes((double)UserData.getWorkDuration());
			foreach (Break lBreak in UserData.getBreaks())
			{
				if ((!lBreak.isEnabled || !(UserData.getWorkTimeStart().TimeOfDay < lBreak.dtStartTime.TimeOfDay) ? false : dateTime.TimeOfDay >= lBreak.dtStartTime.TimeOfDay))
					dateTime = dateTime.Add(lBreak.tsDuration);
			}
			return dateTime;
		}

        private static void TWorkTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            UserData.setWorkTimeEnd(CalcDayEndTime());
            UserData.setWorkTimeElapsed(CalcDayElapsedTime());
            UserData.setWorkTimeRemaining(UserData.getWorkTimeEnd() - DateTime.Now);
        }

		public static bool getIsStarted()
		{
			foreach (Day lDay in UserData.getDays())
			{
				if (lDay.dtEndTime == DateTime.MinValue)
					return true;
			}
			return false;
		}

		public static void WorkdayCalculationStop()
		{
            if (!tWorkTime.Equals(null) && tWorkTime.Enabled.Equals(true))
                tWorkTime.Stop();
		}

		public static void WorkdayEnd(DateTime EndTime)
		{
            List<Day> lTemp = UserData.getDays();
            lTemp.LastOrDefault().dtEndTime = EndTime;
            UserData.setDays(lTemp);
            UserData.setWorkTimeStart(DateTime.MinValue);
            UserData.setWorkTimeEnd(DateTime.MinValue);
            UserData.setWorkTimeRemaining(TimeSpan.Zero);
            UserData.setWorkTimeElapsed(TimeSpan.Zero);
            WorkdayCalculationStop();
		}

        public static int getPercent()
        {
            try
            {
                Double totalMinutes = UserData.getWorkTimeElapsed().TotalMinutes;
                TimeSpan timeSpan = UserData.getWorkTimeElapsed().Add(UserData.getWorkTimeRemaining());

                int num1 = Convert.ToInt32(totalMinutes / timeSpan.TotalMinutes * 100);
                return (num1 < 100 ? num1 : 100);
            }
            catch { return 0; }
        }

        public static void WorkdayStart(int WorkingMinutes, DateTime StartTime)
        {
            UserData.setWorkDuration(WorkingMinutes);
            UserData.setWorkTimeStart(StartTime);

            if (!getIsStarted())
            {
                List<Day> lTemp = UserData.getDays();
                lTemp.Add(new Day(UserData.getDays().Count, StartTime, DateTime.MinValue));
                UserData.setDays(lTemp);
            }

            if (tWorkTime == null)
            {
                tWorkTime = new Timer(UserData.getInterval());
                tWorkTime.Elapsed += TWorkTime_Elapsed;
                tWorkTime.Start();
            }
            else if (tWorkTime.Enabled.Equals(false))
            {
                tWorkTime = new Timer(UserData.getInterval());
                tWorkTime.Elapsed += TWorkTime_Elapsed;
                tWorkTime.Start();
            }
        }
    }
}