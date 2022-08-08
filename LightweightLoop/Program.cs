using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace LightweightLoop
{
    internal class Program
    {
        private static int _runningCount;
        private static int _currentLoop;
        private static bool _continueProgram = true;
        private static TimeSpan _totalRunTime;
        private static Stopwatch _stopwatch;
        private static readonly WordUp _wordManager = new();
        private static Settings _settings;

        static void Main(string[] args)
        {
            LoadConfiguration();

            if (!_settings.ShowConsole)
            {
                Console.SetWindowSize(1, 1);
                Console.SetWindowPosition(0, 0);
            }

            if (_settings.UseCountdown())
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }

            Loop();
        }

        private static void LoadConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _settings = config.GetSection("settings").Get<Settings>();
            _totalRunTime = _settings.TotalRunTime();
        }

        private static void Loop()
        {
            while (_continueProgram)
            {
                if (_settings.DisplayWords) DisplayWords();
                if (_settings.UseCountdown()) DetermineIfEnded();

                KeepAlive();
                Thread.Sleep(500);
            }

            if (_settings.LockMachineOnCompletion) LockWorkStation();
        }

        private static void DisplayWords()
        {
            var word = GetWord();

            _currentLoop++;
            _runningCount++;

            PrintToConsole(word);
            ClearConsole();
        }

        private static void DetermineIfEnded()
        {
            _continueProgram = _stopwatch.Elapsed < _totalRunTime;
        }

        private static string GetWord()
        {
            return _settings.UseRealWords ? _wordManager.RealWord() : _wordManager.RandomWord();
        }

        private static void ClearConsole()
        {
            if (_settings.ClearConsole() || _currentLoop != _settings.ResetConsoleCounter) return;

            _currentLoop = 0;
            Console.Clear();
        }

        private static void PrintToConsole(string word)
        {
            if (!_settings.UseRealWords && _wordManager.IsARealWord(word))
            {
                Console.WriteLine($"Congrats '{word}' is a real word. It took you {_runningCount} attempts");
                return;
            }

            Console.WriteLine(word);
        }

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);

        private static void KeepAlive()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }
    }
}
