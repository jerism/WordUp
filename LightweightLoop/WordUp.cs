using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace LightweightLoop
{
    public class WordUp : IAppStart
    {
        private const string _alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly Random _random = new();
        private readonly List<string> _validWords;
        private readonly int _wordCount;
        private int _runningCount;
        private int _currentLoop;
        private Settings _settings;

        public WordUp()
        {
            _validWords = LoadWords();
            _wordCount = _validWords.Count;
        }

        public string RandomWord()
        {
            var length = _random.Next(0, 10);
            var word = string.Empty;

            for (var i = 0; i < length; i++)
            {
                var characterPos = _random.Next(0, _alphabet.Length);
                var character = _alphabet[characterPos];
                word += character;
            }

            return word;
        }

        public string RealWord()
        {
            var length = _random.Next(0, _wordCount);
            return _validWords[length];
        }

        public bool IsARealWord(string word)
        {
            return _validWords.Any(s => s == word);
        }

        private static List<string> LoadWords()
        {
            var lines = File.ReadAllLines($"{Directory.GetCurrentDirectory()}/words.txt");
            return lines.ToList();
        }

        public void Start(Settings settings)
        {
            if (_settings == null) _settings = settings;

            if (_settings.DisplayWords) DisplayWords();

            Interop.KeepAlive();
            Thread.Sleep(500);
        }

        private void DisplayWords()
        {
            var word = GetWord();

            _currentLoop++;
            _runningCount++;

            PrintToConsole(word);
            ClearConsole();
        }

        private string GetWord()
        {
            return _settings.UseRealWords ? RealWord() : RandomWord();
        }

        private void ClearConsole()
        {
            if (_settings.ClearConsole() || _currentLoop != _settings.ResetConsoleCounter) return;
            _currentLoop = 0;
            Console.Clear();
        }

        private void PrintToConsole(string word)
        {
            if (!_settings.UseRealWords && IsARealWord(word))
            {
                Console.WriteLine($"Congrats '{word}' is a real word. It took you {_runningCount} attempts");
                return;
            }

            Console.WriteLine(word);
        }
    }
}
