using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Web.ViewModels.Display
{
    public class DisplayTweetsViewModel
    {
        public List<Tweet> Tweets { get; set; }
        public string Topic { get; set; }
    }
}
