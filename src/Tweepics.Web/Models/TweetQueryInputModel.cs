using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tweepics.Web.Models
{
    public class TweetQueryInputModel
    {
        public string QueryType { get; set; }

        [MinLength(2), MaxLength(50)]
        public string OriginalQuery { get; set; }

        public int? Page { get; set; }
        public List<string> Officials { get; set; }
        public List<string> States { get; set; }
        public List<string> Parties { get; set; }
    }
}
