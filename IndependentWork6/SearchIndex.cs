
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IndependentWork6
{
    public class SearchIndex
    {
        private readonly Dictionary<string, HashSet<int>> _invertedIndex;

        public SearchIndex()
        {
            _invertedIndex = new Dictionary<string, HashSet<int>>(
                StringComparer.OrdinalIgnoreCase);
        }

        private string NormalizeWord(string word)
        {
            return Regex.Replace(
                word.ToLowerInvariant(),
                @"[^\p{L}\p{N}]+",
                "");
        }

        public void AddDocument(int docId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            string[] words = Regex.Split(
                content.ToLowerInvariant(),
                @"[^\p{L}\p{N}]+");

            foreach (string word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (!_invertedIndex.ContainsKey(word))
                {
                    _invertedIndex[word] = new HashSet<int>();
                }

                _invertedIndex[word].Add(docId);
            }
        }

        public HashSet<int> Search(string word)
        {
            string normalizedWord = NormalizeWord(word);

            if (_invertedIndex.TryGetValue(
                normalizedWord, out HashSet<int> documents))
            {
                return new HashSet<int>(documents);
            }

            return new HashSet<int>();
        }

        public HashSet<int> SearchMultiple(params string[] words)
        {
            HashSet<int> result = new HashSet<int>();

            if (words == null || words.Length == 0)
                return result;

            result = Search(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                result.IntersectWith(Search(words[i]));
            }

            return result;
        }
    }
}
