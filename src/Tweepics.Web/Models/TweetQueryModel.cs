using System.ComponentModel.DataAnnotations;
using Tweepics.Web.Services;

namespace Tweepics.Web.Models
{
    public class TweetQueryModel
    {
        // "Search" (user searched from home page) or "Tag" (user selected one of the tag options)
        public string QueryType { get; set; } 

        // Search query or tag name
        [MinLength(2), MaxLength(50)]
        public string OriginalQuery { get; set; }

        public int? Page { get; set; }
        public FilterOptions FilterOptions { get; set; }
    }
}
