using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class TweetReader
    {
        private readonly string _connectionString;

        public List<Tweet> ReadAll()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM tweet_data"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();

                List<Tweet> tweets = new List<Tweet>();

                while (dataReader.Read())
                {
                    string fullName = dataReader[0].ToString();
                    string screenName = dataReader[1].ToString();
                    long userID = Convert.ToInt64(dataReader[2]);
                    DateTime createdAt = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string text = dataReader[5].ToString();
                    string url = dataReader[6].ToString();
                    string html = dataReader[7].ToString();
                    DateTime addedToDatabaseAt = Convert.ToDateTime(dataReader[8]);

                    tweets.Add(new Tweet(fullName, screenName, userID, createdAt, tweetID,
                                         text, url, html, addedToDatabaseAt));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }

        public long? FindMostRecentTweetId(long userID)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

                long mostRecentTweetId = Convert.ToInt64(dataReader[0]);

                dataReader.Close();
                connection.Close();

                return mostRecentTweetId;
            }
        }

        public long? FindOldestTweetId(long userID)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT MIN(tweet_id) FROM tweet_data WHERE user_id = @user_id"
                };
                cmd.Parameters.AddWithValue("@user_id", userID);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();

                if (dataReader[0] == DBNull.Value)
                    return null;

                long oldestTweetId = Convert.ToInt64(dataReader[0]);

                dataReader.Close();
                connection.Close();

                return oldestTweetId;
            }
        }

        public List<Tweet> FindTweetsByTag(string tag)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<Tweet> tweets = new List<Tweet>();

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
                    DateTime createdAt = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string text = dataReader[5].ToString();
                    string url = dataReader[6].ToString();
                    string html = dataReader[7].ToString();

                    tweets.Add(new Tweet(fullName, screenName, userID, createdAt, tweetID,
                                         text, url, html));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }

        public Dictionary<long, int> CountTweetFrequencyByUser()
        {

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                Dictionary<long, int> tweetFrequency = new Dictionary<long, int>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT user_id, COUNT(*) FROM tweet_data GROUP BY user_id"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.HasRows)
                    return null;

                while (dataReader.Read())
                {
                    long userId = Convert.ToInt64(dataReader[0]);
                    int tweetCount = Convert.ToInt32(dataReader[1]);

                    tweetFrequency.Add(userId, tweetCount);
                }
                dataReader.Close();
                connection.Close();
                return tweetFrequency;
            }
        }

        public List<Tweet> GetUserTweets(long userId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<Tweet> tweets = new List<Tweet>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM tweet_data WHERE user_id = @user_id"
                };
                cmd.Parameters.AddWithValue("@user_id", userId);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.HasRows)
                    return null;

                while (dataReader.Read())
                {
                    string fullName = dataReader[0].ToString();
                    string screenName = dataReader[1].ToString();
                    long userID = Convert.ToInt64(dataReader[2]);
                    DateTime createdAt = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string text = dataReader[5].ToString();
                    string url = dataReader[6].ToString();
                    string html = dataReader[7].ToString();

                    tweets.Add(new Tweet(fullName, screenName, userID, createdAt, tweetID,
                                         text, url, html));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }

        public List<Tweet> SearchUsersAndTweetContent(string searchQuery)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<Tweet> tweets = new List<Tweet>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM tweet_data WHERE MATCH (full_name, tweet_text) 
                                    AGAINST (@searchQuery IN NATURAL LANGUAGE MODE)"
                };
                cmd.Parameters.AddWithValue("@searchQuery", searchQuery);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (!dataReader.HasRows)
                    return null;

                while (dataReader.Read())
                {
                    string fullName = dataReader[0].ToString();
                    string screenName = dataReader[1].ToString();
                    long userID = Convert.ToInt64(dataReader[2]);
                    DateTime createdAt = Convert.ToDateTime(dataReader[3]);
                    long tweetID = Convert.ToInt64(dataReader[4]);
                    string text = dataReader[5].ToString();
                    string url = dataReader[6].ToString();
                    string html = dataReader[7].ToString();

                    tweets.Add(new Tweet(fullName, screenName, userID, createdAt, tweetID,
                                         text, url, html));
                }
                dataReader.Close();
                connection.Close();
                return tweets;
            }
        }

        public TweetReader(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}