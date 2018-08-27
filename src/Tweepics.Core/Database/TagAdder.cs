using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class TagAdder
    {
        private readonly string _connectionString;

        public void Add(List<Tag> tagsKeywords)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var entry in tagsKeywords)
                {
                    Guid guid = Guid.NewGuid();
                    string tagID = guid.ToString();

                    string tagName = entry.Name;
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

        public TagAdder(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}