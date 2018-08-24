using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Parse;
using Tweepics.Core.Config;

namespace Tweepics.Core.Database
{
    public class TweetReader
    {
        public List<TweetData> ReadAll()
        {
            using (MySqlConnection connection = new MySqlConnection(Keys.mySqlConnectionString))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM tweet_data"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();

                List<TweetData> tweets = new List<TweetData>();

                while (dataReader.Read())
                {
                    string fullName = dataReader[0].ToString();
                    string screenName = dataReader[1].ToString();
                    long userID = Convert.ToInt64(dataReader[2]);
                    DateTime tweetDateTime = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string text = dataReader[5].ToString();
                    DateTime addedDateTime = Convert.ToDateTime(dataReader[6]);

                    tweets.Add(new TweetData(fullName, screenName, userID, tweetDateTime,
                                             tweetID, text, addedDateTime));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }

        public long? FindMostRecentTweetID(long userID)
        {
            using (MySqlConnection connection = new MySqlConnection(Keys.mySqlConnectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT MAX(tweet_id) FROM tweet_data WHERE user_id = @user_id"
                };
                cmd.Parameters.AddWithValue("@user_id", userID);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();

                if (dataReader[0] == DBNull.Value)
                    return null;

                long mostRecentTweetID = Convert.ToInt64(dataReader[0]);

                dataReader.Close();
                connection.Close();

                return mostRecentTweetID;
            }
        }

        public List<TweetData> FindTweetsByTag(string tag)
        {
            using (MySqlConnection connection = new MySqlConnection(Keys.mySqlConnectionString))
            {
                connection.Open();

                List<TweetData> tweets = new List<TweetData>();

                // (1) Find tag id from argument in tweet_tags, (2) find tweet ids from 
                // tag id in tagmap, and (3) find tweets from tweet ids in tweet_data

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT td.* 
                                FROM tweet_data td, tweet_tags tt, tagmap tm
                                WHERE td.tweet_id = tm.tweet_id
                                AND tm.tag_id = tt.id
                                AND tt.tag = @tag"
                };
                cmd.Parameters.AddWithValue("@tag", tag.ToLower());

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.HasRows)
                    return null;

                while (dataReader.Read())
                {
                    string fullName = dataReader[0].ToString();
                    string screenName = dataReader[1].ToString();
                    long userID = Convert.ToInt64(dataReader[2]);
                    DateTime tweetDateTime = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string tweetText = dataReader[5].ToString();

                    tweets.Add(new TweetData(fullName, screenName, userID, tweetDateTime, tweetID, tweetText));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }
    }
}