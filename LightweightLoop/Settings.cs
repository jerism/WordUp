using System;

namespace LightweightLoop
{
    public class Settings
    {
        public string ApplicationType { get; set; }
        public bool UseRealWords { get; set; }
        public bool DisplayWords { get; set; }
        public bool ShowConsole { get; set; }
        public bool LockMachineOnCompletion { get; set; }
        public int ResetConsoleCounter { get; set; }
        public int FinalRunTimeMins { get; set; }
        public int DelayMins { get; set; }
        public int BeforeDelayMins { get; set; }

        public bool ClearConsole()
        {
            return ResetConsoleCounter > 0;
        }

        public TimeSpan TotalRunTime()
        {
            return TimeSpan.FromMinutes(FinalRunTimeMins);
        }

        public TimeSpan DelayRunTime()
        {
            return TimeSpan.FromMinutes(DelayMins);
        }

        public TimeSpan BeforeDelayTime()
        {
            return TimeSpan.FromMinutes(BeforeDelayMins);
        }
    }
}
