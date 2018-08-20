using System.Collections.Generic;
using Tweepics.Core.Parse;

namespace Tweepics.Web.ViewModels.Display
{
    public class DisplayTweetsViewModel
    {
        public List<TweetData> Tweets { get; set; }
        public string Topic { get; set; }
    }
}
