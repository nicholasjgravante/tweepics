using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class TagReader
    {
        private readonly string _connectionString;

        public List<Tag> ReadAll()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<Tag> tags = new List<Tag>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * from tags"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string id = dataReader[0].ToString();
                    string tagNotCapitalizedFirst = dataReader[1].ToString();
                    string tagCapitalizedFirst = tagNotCapitalizedFirst.First().ToString().ToUpper() +
                                                 tagNotCapitalizedFirst.Substring(1);

                    string keywordString = dataReader[2].ToString();

                    tags.Add(new Tag(id, tagCapitalizedFirst, keywordString));
                }
                dataReader.Close();
                connection.Close();

                return tags;
            }
        }

        public TagReader(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}
