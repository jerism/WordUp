using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading;

namespace LightweightLoop
{
    internal class Program
    {
        private static TimeSpan _totalRunTime;
        private static TimeSpan _delayRunTime;
        private static TimeSpan _beforeDelayTime;
        private static Stopwatch _stopwatch;
        private static Settings _settings;

        static void Main(string[] args)
        {
            LoadConfiguration();
            RunApplication();
        }

        private static void RunApplication()
        {
            IAppStart appStart = _settings.ApplicationType switch
            {
                "Clicker" => new ExperimentalTouch(),
                "Words" => new WordUp(),
                _ => throw new NotImplementedException($"Application type of {_settings.ApplicationType} has not been created"),
            };

            RunLoop(appStart);
        }

        private static void RunLoop(IAppStart appStart)
        {
            // Keep Alive
            if (_settings.BeforeDelayMins > 0)
            {
                while (_stopwatch.Elapsed < _beforeDelayTime)
                {
                    appStart.Start(_settings);
                }

                _stopwatch.Restart();
            }

            // Away but on
            if (_settings.DelayMins > 0)
            {
                while (_stopwatch.Elapsed < _delayRunTime)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    Interop.KeepAlive();
                }

                _stopwatch.Restart();
            }

            // Keep Alive
            while (_stopwatch.Elapsed < _totalRunTime)
            {
                appStart.Start(_settings);
            }

            if (_settings.LockMachineOnCompletion) Interop.LockWorkStation();
        }

        private static void LoadConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _settings = config.GetSection("settings").Get<Settings>();
            _totalRunTime = _settings.TotalRunTime();
            _delayRunTime = _settings.DelayRunTime();
            _beforeDelayTime = _settings.BeforeDelayTime();
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            if (!_settings.ShowConsole)
            {
                Console.SetWindowSize(1, 1);
                Console.SetWindowPosition(0, 0);
            }
        }
    }
}
