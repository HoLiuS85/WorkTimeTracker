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
				if ((!lBreak.enabled || !(DateTime.Now.TimeOfDay > lBreak.starttime.TimeOfDay) || !(UserData.getWorkTimeStart().TimeOfDay < lBreak.starttime.TimeOfDay) ? false : UserData.getWorkTimeEnd().TimeOfDay > lBreak.starttime.TimeOfDay))
					now = now.Subtract(lBreak.duration);
			}
			return now - UserData.getWorkTimeStart();
		}

		private static DateTime CalcDayEndTime()
		{
			DateTime dateTime = UserData.getWorkTimeStart().AddMinutes((double)UserData.getWorkDuration());
			foreach (Break lBreak in UserData.getBreaks())
			{
				if ((!lBreak.enabled || !(UserData.getWorkTimeStart().TimeOfDay < lBreak.starttime.TimeOfDay) ? false : dateTime.TimeOfDay >= lBreak.starttime.TimeOfDay))
					dateTime = dateTime.Add(lBreak.duration);
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
            if (UserData.getWorkTimeStart().Equals(DateTime.MinValue))
                return false;
            else
                return true;
        }

        public static void WorkdayCalculationStop()
        {
            if (tWorkTime != null)
            {
                if (tWorkTime.Enabled.Equals(true))
                    tWorkTime.Stop();
            }
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
            //Store parameters of the newly started workday
            UserData.setWorkDuration(WorkingMinutes);
            UserData.setWorkTimeStart(StartTime);

            //Calculate the workday parameters intially to avoid delays
            TWorkTime_Elapsed(new object(), new EventArgs() as ElapsedEventArgs );
            
            //Initialize and start a new timer for workday calulation
            if (tWorkTime == null || tWorkTime?.Enabled == false)
            {
                tWorkTime = new Timer(10000);
                tWorkTime.Elapsed += TWorkTime_Elapsed;
                tWorkTime.Start();
            }
        }

        public static void WorkdayEnd(DateTime EndTime)
        {
            //Stop the calculation of Worktime Data
            WorkdayCalculationStop();

            //Store the ended workday in the workday history
            List<Day> lTemp = UserData.getDays();
            lTemp.Add(new Day(UserData.getWorkTimeStart(), EndTime));
            UserData.setDays(lTemp);

            //Set all workday related parameters to Zero
            UserData.setWorkTimeStart(DateTime.MinValue);
            UserData.setWorkTimeEnd(DateTime.MinValue);
            UserData.setWorkTimeRemaining(TimeSpan.Zero);
            UserData.setWorkTimeElapsed(TimeSpan.Zero);
        }

    }
}