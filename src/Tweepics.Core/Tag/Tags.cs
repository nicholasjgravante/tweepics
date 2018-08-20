using System.Linq;
using System.Collections.Generic;

namespace Tweepics.Core.Tag
{
    public class Tags
    {
        public string ID { get; set; }
        public string Tag { get; set; }
        public string KeywordString { get; set; }
        public List<string> KeywordList { get; set; }

        public Tags() { }

        public Tags(string tagCategory, string keywordsString)
            : this ("", tagCategory, keywordsString) { }

        public Tags(string tagID, string tagCategory, string keywordString)
        {
            ID = tagID;
            Tag = tagCategory;
            KeywordString = keywordString.ToLower();
            KeywordList = keywordString.ToLower().Split(", ").ToList();
        }
    }
}