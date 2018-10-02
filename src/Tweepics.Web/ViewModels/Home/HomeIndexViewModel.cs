using System.Collections.Generic;
using Tweepics.Web.Models;

namespace Tweepics.Web.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<string> Tags { get; set; }
        public TweetQueryInputModel TweetQueryInputModel { get; set; }
    }
}
