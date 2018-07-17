using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Tag;
using Tweepics.Config;

namespace Tweepics.Database.Operations
{
    public class UpdateTagsKeywords
    {
        public void Add(List<Tags> tagsKeywords)
        {
            string connectionString = Keys.mySqlConnectionString;

            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                foreach (var entry in tagsKeywords)
                {
                    Guid guid = Guid.NewGuid();
                    string tagID = guid.ToString();

                    string tagName = entry.Tag;
                    string tagKeywords = entry.KeywordString;

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = @"INSERT INTO tweet_tags (id, tag, keywords) 
                                        VALUES (@id, @tag, @keywords)
                                        ON DUPLICATE KEY UPDATE keywords=@keywords"
                    };
                    cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = tagID;
                    cmd.Parameters.Add("@tag", MySqlDbType.VarChar).Value = tagName;
                    cmd.Parameters.Add("@keywords", MySqlDbType.VarChar).Value = tagKeywords;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}