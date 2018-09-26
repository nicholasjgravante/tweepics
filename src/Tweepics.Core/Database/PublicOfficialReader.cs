using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweepics.Core.Models;

namespace Tweepics.Core.Database
{
    public class PublicOfficialReader
    {
        private readonly string _connectionString;

        public List<PublicOfficial> ReadFromDb()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                List<PublicOfficial> officials = new List<PublicOfficial>();

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT * from rep_info"
                };

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string tweepicsId = dataReader[0].ToString();
                    string firstName = dataReader[1].ToString();
                    string middleName = dataReader[2].ToString();
                    string lastName = dataReader[3].ToString();
                    string suffix = dataReader[4].ToString();
                    string officeGovernment = dataReader[5].ToString();
                    string officeBranch = dataReader[6].ToString();
                    string officeState = dataReader[7].ToString();
                    string officeTitle = dataReader[8].ToString();
                    string party = dataReader[9].ToString();
                    long twitterId = Convert.ToInt64(dataReader[10]);
                    string twitterScreenName = dataReader[11].ToString();

                    Name name = new Name(firstName, middleName, lastName, suffix);
                    Office office = new Office(officeGovernment, officeBranch, officeState, officeTitle);

                    officials.Add(new PublicOfficial(tweepicsId, name, office, party, twitterId, twitterScreenName));
                }
                dataReader.Close();
                connection.Close();

                return officials;
            }
        }

        public List<PublicOfficial> ReadFromFile(string filePath)
        {
            string fileText = File.ReadAllText($"{filePath}");

            List<string> rawDataStrings = new List<string>();
            rawDataStrings = fileText.Split("\n").ToList();

            List<PublicOfficial> officials = new List<PublicOfficial>();

            foreach (string line in rawDataStrings)
            {
                List<string> oneLineOfData = new List<string>();
                oneLineOfData = line.Split(",").ToList();

                if (oneLineOfData.Any())
                {
                    string firstName = oneLineOfData[0];
                    string middleName = oneLineOfData[1];
                    string lastName = oneLineOfData[2];
                    string suffix = oneLineOfData[3];
                    string officeGovernment = oneLineOfData[4];
                    string officeBranch = oneLineOfData[5];
                    string officeState = oneLineOfData[6];
                    string officeTitle = oneLineOfData[7];
                    string party = oneLineOfData[8];
                    long twitterId = 0;

                    if (oneLineOfData[9] != string.Empty)
                    {
                        twitterId = Convert.ToInt64(oneLineOfData[9]);
                    }

                    string twitterScreenName = oneLineOfData[10];

                    Name name = new Name(firstName, middleName, lastName, suffix);
                    Office office = new Office(officeGovernment, officeBranch, officeState, officeTitle);

                    officials.Add(new PublicOfficial(name, office, party, twitterId, twitterScreenName));
                }
                else
                    break;
            }
            return officials;
        }

            public PublicOfficialReader(string mySqlConnectionString)
        {
            _connectionString = mySqlConnectionString;
        }
    }
}
