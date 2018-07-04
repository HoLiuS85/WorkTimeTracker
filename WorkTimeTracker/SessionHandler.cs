using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WorkTimeTracker
{
    class SessionHandler
    {
        private WindowInteropHelper _helper;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static bool ShutdownBlockReasonCreate([In]IntPtr hWnd, [In] string pwszReason);

        [DllImport("user32.dll", SetLastError = true)]
        public extern static bool ShutdownBlockReasonDestroy([In]IntPtr hWnd);

        public SessionHandler()
        {
            //Call Unlock on Applciation Start
            LogonUnlock();

            //Register Events for session End (Logoff/Shutdown)
            SystemEvents.SessionEnding += (e1, e2) =>
            {
                SessionSwitch(null, new SessionSwitchEventArgs(SessionSwitchReason.SessionLogoff));
            };

            //Register Event for session Switch (Un-/Lock Workstation)
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SessionSwitch);
        }

        private void SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            ////logoff or shutdown or locked
            if (e.Reason == SessionSwitchReason.SessionLogoff || e.Reason == SessionSwitchReason.SessionLock)
            {
                //Save the Lock timestamp
                UserData.setLastLockTime(DateTime.Now);
            }
            ////startup or logon or unlock
            else if (e.Reason == SessionSwitchReason.SessionLogon || e.Reason == SessionSwitchReason.SessionUnlock)
            {
                ////startup or logon or unlock
                LogonUnlock();
            }
        }

        private void LogonUnlock()
        {
            //If LastLockTime is not today
            if (!UserData.getLastLockTime().Date.Equals(DateTime.Now.Date))
            {
                DateTime dtTempNow = DateTime.Now;

                //If the Workday is started, offer to end it
                if (WorkdayHandler.getIsStarted())
                {
                    if (WorkdayEnd(UserData.getLastLockTime()))
                        WorkdayStart(UserData.getWorkDuration(), dtTempNow);
                }
                else
                {
                    WorkdayStart(UserData.getWorkDuration(), dtTempNow);
                }
            }
        }

        private bool WorkdayEnd(DateTime LastLockTime)
        {
            string strCaption = "First daily login detected";
            string strText = "Your last workday on '" + LastLockTime.ToShortDateString() + "' is still active. Do you want to end it using last logoff time: " + LastLockTime.ToShortTimeString();

            if (MessageBoxResult.Yes == MessageBox.Show(strText, strCaption, MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No))
            {
                WorkdayHandler.WorkdayEnd(LastLockTime);
                return true;
            }
            return false;
        }

        private void WorkdayStart(int WorkDuration, DateTime WorkStartTime)
        {
            string strCaption = "First daily login detected";
            string strText = "Do you want to start a new workday now: " + WorkStartTime.ToShortTimeString() + "?";

            if (MessageBoxResult.Yes == MessageBox.Show(strText, strCaption, MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No))
                WorkdayHandler.WorkdayStart(WorkDuration, WorkStartTime);
        }


    }
}
