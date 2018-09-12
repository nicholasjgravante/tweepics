using System.Collections.Generic;
using Tweepics.Core.Models;
using X.PagedList;

namespace Tweepics.Web.ViewModels.Display
{
    public class DisplayTweetsViewModel
    {
        public string Topic { get; set; }
        public IPagedList<Tweet> Tweets { get; set; }
        public int ThirtyDayCount { get; set; }
    }
}
