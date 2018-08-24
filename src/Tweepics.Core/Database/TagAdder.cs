using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Tag;
using Tweepics.Core.Config;

namespace Tweepics.Core.Database
{
    public class TagAdder
    {
        public void Add(List<Tags> tagsKeywords)
        {
            using (MySqlConnection connection = new MySqlConnection(Keys.mySqlConnectionString))
            {
                connection.Open();

                foreach (var entry in tagsKeywords)
                {
                    Guid guid = Guid.NewGuid();
                    string tagID = guid.ToString();

                    string tagName = entry.Tag;
                    string tagKeywords = entry.KeywordString;

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = @"INSERT INTO tweet_tags (id, tag, keywords) 
                                    VALUES (@id, @tag, @keywords)
                                    ON DUPLICATE KEY UPDATE keywords=@keywords"
                    };
                    cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = tagID;
                    cmd.Parameters.Add("@tag", MySqlDbType.VarChar).Value = tagName;
                    cmd.Parameters.Add("@keywords", MySqlDbType.VarChar).Value = tagKeywords;

                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}