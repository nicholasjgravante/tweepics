using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Core.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string KeywordString { get; set; }
        public List<string> KeywordList { get; set; }

        public Tag() { }

        public Tag(string name, string keywordsString)
            : this ("", name, keywordsString) { }

        public Tag(string id, string name, string keywordString)
        {
            Id = id;
            Name = name;
            KeywordString = keywordString.ToLower();
            KeywordList = keywordString.ToLower().Split(", ").ToList();
        }
    }
}