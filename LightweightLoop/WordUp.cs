using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LightweightLoop
{
    public class WordUp
    {
        private const string _alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly Random _random = new();
        private readonly List<string> _validWords;
        private readonly int _wordCount;

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
    }
}
