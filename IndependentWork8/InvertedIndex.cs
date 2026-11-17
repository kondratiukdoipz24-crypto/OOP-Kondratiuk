
using System;
using System.Collections.Generic;
using System.Linq;

public class InvertedIndex
{
    private Dictionary<string, HashSet<int>> _index;
    private Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public void AddDocument(Document doc)
    {
        if (doc == null)
            throw new ArgumentNullException(nameof(doc));

        _documents[doc.Id] = doc;

        var words = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(doc.Content));

        foreach (string word in words)
        {
            if (!_index.ContainsKey(word))
            {
                _index[word] = new HashSet<int>();
            }

            _index[word].Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        string normalizedWord = TextProcessor.Tokenize(word)
            .FirstOrDefault() ?? "";

        if (string.IsNullOrEmpty(normalizedWord))
            return new List<Document>();

        if (!_index.TryGetValue(normalizedWord, out HashSet<int> ids))
            return new List<Document>();

        return ids
            .Where(id => _documents.ContainsKey(id))
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var searchWords = TextProcessor.RemoveStopWords(
            words.SelectMany(word => TextProcessor.Tokenize(word)))
            .Distinct()
            .ToList();

        if (searchWords.Count == 0)
            return new List<Document>();

        HashSet<int> result = null;

        foreach (string word in searchWords)
        {
            if (!_index.TryGetValue(word, out HashSet<int> ids))
                return new List<Document>();

            if (result == null)
                result = new HashSet<int>(ids);
            else
                result.IntersectWith(ids);
        }

        return result
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var searchWords = TextProcessor.RemoveStopWords(
            words.SelectMany(word => TextProcessor.Tokenize(word)))
            .Distinct()
            .ToList();

        HashSet<int> result = new HashSet<int>();

        foreach (string word in searchWords)
        {
            if (_index.TryGetValue(word, out HashSet<int> ids))
            {
                result.UnionWith(ids);
            }
        }

        return result
            .Select(id => _documents[id])
            .OrderBy(doc => doc.Id)
            .ToList();
    }

    public int DocumentCount => _documents.Count;

    public int UniqueWords => _index.Keys.Count;
}
