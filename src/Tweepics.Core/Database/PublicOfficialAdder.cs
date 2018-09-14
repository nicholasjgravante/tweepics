using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class PublicOfficialAdder
    {
        private readonly string _connectionString;

        public void Add(List<PublicOfficial> officials)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var official in officials)
                {
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = @"INSERT INTO representative_information (tweepics_id, first_name, middle_name, 
                                        last_name, full_name, state, party, twitter_id, twitter_screen_name)
                                        VALUES (@tweepics_id, @first_name, @middle_name, @last_name, @full_name, 
                                        @state, @party, @twitter_id, @twitter_screen_name)
                                        ON DUPLICATE KEY UPDATE middle_name=@middle_name"
                    };
                    cmd.Parameters.Add("@tweepics_id", MySqlDbType.VarChar).Value = official.TweepicsId;
                    cmd.Parameters.Add("@first_name", MySqlDbType.VarChar).Value = official.FirstName;
                    cmd.Parameters.Add("@middle_name", MySqlDbType.VarChar).Value = official.MiddleName;
                    cmd.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = official.LastName;
                    cmd.Parameters.Add("@full_name", MySqlDbType.VarChar).Value = official.FullName;
                    cmd.Parameters.Add("@state", MySqlDbType.VarChar).Value = official.State;
                    cmd.Parameters.Add("@party", MySqlDbType.VarChar).Value = official.Party;
                    cmd.Parameters.Add("@twitter_id", MySqlDbType.Int64).Value = official.TwitterId;
                    cmd.Parameters.Add("@twitter_screen_name", MySqlDbType.VarChar).Value = official.TwitterScreenName;

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public PublicOfficialAdder(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}
