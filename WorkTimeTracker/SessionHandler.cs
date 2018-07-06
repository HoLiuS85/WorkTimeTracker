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
        public delegate void EventDelegate();
        public event EventDelegate MyWorkdayEvent;


        private void OnWorkdayEvent()
        {
            MyWorkdayEvent?.Invoke();
        }

        public SessionHandler()
        {
            //Call Unlock on Applciation Start to check if a workday is running
            VerifySessionState();

            //Register Events for session End (Logoff/Shutdown)
            SystemEvents.SessionEnding += (e1, e2) =>
            {
                //Call Session Change manually to store current time as last lock time
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
                VerifySessionState();
            }
        }

        public void VerifySessionState()
        {
            //If LastLockTime is not today
            if (!UserData.getLastLockTime().Date.Equals(DateTime.Now.Date))
            {
                //Raise the event
                OnWorkdayEvent();
            }
        }
    }
}
