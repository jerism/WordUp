using System;

namespace LightweightLoop
{
    public class Settings
    {
        public bool UseRealWords { get; set; }
        public bool DisplayWords { get; set; }
        public bool ShowConsole { get; set; }
        public bool LockMachineOnCompletion { get; set; }
        public int ResetConsoleCounter { get; set; }
        public int TotalRunTimeMins { get; set; }

        public bool UseCountdown()
        {
            return TotalRunTimeMins > 0;
        }

        public bool ClearConsole()
        {
            return ResetConsoleCounter > 0;
        }
        public TimeSpan TotalRunTime()
        {
            return TimeSpan.FromMinutes(TotalRunTimeMins);
        }
    }
}
