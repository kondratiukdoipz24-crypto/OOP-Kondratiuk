
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class TextProcessor
{
    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new List<string>();

        return Regex.Matches(
                text.ToLowerInvariant(),
                @"[\p{L}\p{N}]+(?:[+#]+)?")
            .Cast<Match>()
            .Select(match => match.Value)
            .ToList();
    }

    public static IEnumerable<string> RemoveStopWords(
        IEnumerable<string> words)
    {
        HashSet<string> stopWords = new HashSet<string>
        {
            "a", "an", "the", "is", "are", "was", "were",
            "in", "on", "at", "to", "for", "of", "and", "or"
        };

        return words.Where(word => !stopWords.Contains(word));
    }
}
