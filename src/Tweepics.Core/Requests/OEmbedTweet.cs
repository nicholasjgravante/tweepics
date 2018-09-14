using Tweetinvi.Models.DTO;

namespace Tweepics.Core.Requests
{
    public static class OEmbedTweet
    {
        public static string GetHtml(string tweetUrl)
        {
            var oembed = Tweetinvi.TwitterAccessor.ExecuteGETQuery<IOEmbedTweetDTO>
                        ($"https://publish.twitter.com/oembed?url={tweetUrl}&omit_script=true&hide_thread=true");

            return oembed.HTML;
        }
    }
}
