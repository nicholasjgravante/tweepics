using Tweepics.Core.Models;
using Tweepics.Web.Services;
using X.PagedList;

namespace Tweepics.Web.ViewModels.Display
{
    public class DisplayTweetsViewModel
    {
        public string QueryType { get; set; }
        public string OriginalQuery { get; set; }
        public FilterOptions FiltersSelected { get; set; }
        public IPagedList<Tweet> Tweets { get; set; }
        public ResultInfo ResultInfo { get; set; }
    }
}