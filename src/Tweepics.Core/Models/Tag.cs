using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Core.Models
{
    public class Tag
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string KeywordString { get; set; }
        public List<string> KeywordList { get; set; }

        public Tag() { }

        public Tag(string tagName, string keywordsString)
            : this ("", tagName, keywordsString) { }

        public Tag(string tagID, string tagName, string keywordString)
        {
            ID = tagID;
            Name = tagName;
            KeywordString = keywordString.ToLower();
            KeywordList = keywordString.ToLower().Split(", ").ToList();
        }
    }
}