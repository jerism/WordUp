using System;
using System.Threading;

namespace LightweightLoop
{
    public class ExperimentalTouch : IAppStart
    {
        private static void Click(int x, int y)
        {
            Interop.SetCursorPos(x, y);
            Interop.mouse_event(0x02 | 0x04, x, y, 0, 0);
        }

        public void Start(Settings settings)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Click(250, 150);

            Interop.KeepAlive();

            Thread.Sleep(TimeSpan.FromSeconds(10));
            Click(250, 200);
        }
    }
}
