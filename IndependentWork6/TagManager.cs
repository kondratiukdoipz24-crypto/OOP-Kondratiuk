
using System;
using System.Collections.Generic;

namespace IndependentWork6
{
    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags;

        public TagManager()
        {
            _articleTags = new Dictionary<string, HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);
        }

        public void AddTag(string articleId, string tag)
        {
            if (string.IsNullOrWhiteSpace(articleId) ||
                string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("Ідентифікатор і тег не можуть бути порожніми.");
            }

            if (!_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId] = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);
            }

            _articleTags[articleId].Add(tag.Trim());
        }

        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.ContainsKey(articleId))
            {
                _articleTags[articleId].Remove(tag);

                if (_articleTags[articleId].Count == 0)
                {
                    _articleTags.Remove(articleId);
                }
            }
        }

        public HashSet<string> GetAllUniqueTags()
        {
            HashSet<string> result = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (HashSet<string> tags in _articleTags.Values)
            {
                result.UnionWith(tags);
            }

            return result;
        }

        public HashSet<string> FindCommonTags(
            string articleId1, string articleId2)
        {
            HashSet<string> result = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            if (!_articleTags.ContainsKey(articleId1) ||
                !_articleTags.ContainsKey(articleId2))
            {
                return result;
            }

            result.UnionWith(_articleTags[articleId1]);
            result.IntersectWith(_articleTags[articleId2]);

            return result;
        }

        public HashSet<string> FindTagsOnlyInArticle(
            string articleId1, string articleId2)
        {
            HashSet<string> result = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            if (!_articleTags.ContainsKey(articleId1))
                return result;

            result.UnionWith(_articleTags[articleId1]);

            if (_articleTags.ContainsKey(articleId2))
            {
                result.ExceptWith(_articleTags[articleId2]);
            }

            return result;
        }

        public HashSet<string> GetArticleTags(string articleId)
        {
            HashSet<string> result = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            if (_articleTags.ContainsKey(articleId))
            {
                result.UnionWith(_articleTags[articleId]);
            }

            return result;
        }
    }
}
