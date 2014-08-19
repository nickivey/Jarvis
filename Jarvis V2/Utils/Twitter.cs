using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace Jarvis.Utils
{
    class Twitter
    {
       public static string LatestTweet = "";

        public void TwitterLoad()
        {
            var twitterApp = new TwitterService("", "");
            twitterApp.AuthenticateWith("", "");

            IEnumerable<TwitterStatus> tweets = twitterApp.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = "ACCOUNTNAME", Count = 1, });
           if (tweets != null){
                foreach (var tweet in tweets)
                {
                    LatestTweet = tweet.Text;
                }
            }
        }
    }
}
        