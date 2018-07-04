using System;
using System.Collections.Generic;

namespace WorkTimeTracker
{
    internal class WorkdayHandler
	{
        public static void WorkdayStart(int WorkingMinutes, DateTime StartTime)
        {
            //Store parameters of the newly started workday
            UserData.setWorkDuration(WorkingMinutes);
            UserData.setWorkTimeStart(StartTime);
        }

        public static void WorkdayEnd(DateTime EndTime)
        {
            //Store the ended workday in the workday history
            List<Day> lTemp = UserData.getDays();
            lTemp.Add(new Day(UserData.getWorkTimeStart(), EndTime));
            UserData.setDays(lTemp);

            //Set all workday related parameters to Zero
            UserData.setWorkTimeStart(DateTime.MinValue);
        }
        
        public static bool getIsStarted()
        {
            if (UserData.getWorkTimeStart().Equals(DateTime.MinValue))
                return false;
            else
                return true;
        }

        public static DateTime getWorkTimeEnd(DateTime dtStartTime)
        {
            if (!getIsStarted())
                return DateTime.MinValue;

            DateTime dtEndTime = dtStartTime.AddMinutes(UserData.getWorkDuration());

            foreach (Break lBreak in UserData.getBreaks())
            {
                if ((!lBreak.enabled || !(dtStartTime.TimeOfDay < lBreak.starttime.TimeOfDay) ? false : dtEndTime.TimeOfDay >= lBreak.starttime.TimeOfDay))
                    dtEndTime = dtEndTime.Add(lBreak.duration);
            }

            return dtEndTime;
        }

        public static TimeSpan getWorkTimeElapsed(DateTime dtStartTime)
        {
            if (!getIsStarted())
                return TimeSpan.Zero;

            DateTime dtEndTime = DateTime.Now;

            foreach (Break lBreak in UserData.getBreaks())
            {
                if ((!lBreak.enabled || !(DateTime.Now.TimeOfDay > lBreak.starttime.TimeOfDay) || !(dtStartTime.TimeOfDay < lBreak.starttime.TimeOfDay) ? false : getWorkTimeEnd(dtStartTime).TimeOfDay > lBreak.starttime.TimeOfDay))
                    dtEndTime = dtEndTime.Subtract(lBreak.duration);
            }

            return dtEndTime - dtStartTime;
        }

        public static TimeSpan getWorkTimeRemaining(DateTime dtStartTime)
        {
            if (!getIsStarted())
                return TimeSpan.Zero;

            return getWorkTimeEnd(dtStartTime) - DateTime.Now;
        }

        public static int getPercent(DateTime dtStartTime)
        {
            if (!getIsStarted())
                return 0;

            try
            {
                Double totalMinutes = getWorkTimeElapsed(UserData.getWorkTimeStart()).TotalMinutes;
                TimeSpan timeSpan = getWorkTimeElapsed(UserData.getWorkTimeStart()).Add(getWorkTimeRemaining(dtStartTime));

                int num1 = Convert.ToInt32(totalMinutes / timeSpan.TotalMinutes * 100);
                return (num1 < 100 ? num1 : 100);
            }
            catch { return 0; }
        }

    }
}