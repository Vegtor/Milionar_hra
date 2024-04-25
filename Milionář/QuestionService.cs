using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Milionář
{
    internal class QuestionService
    {
        private MySqlConnection connection;
        public QuestionService()
        {
            connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=test;");
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        public string[] GetQuestion(int level)
        {
            try
            {
                string query = "SELECT * FROM test.otazky WHERE obtiznost = " + level + " ORDER BY RAND() LIMIT 1;";
                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    string[] result = { (string)reader["otazka"], (string)reader["odpoved_spravna"], (string)reader["odpoved_spatna_1"], (string)reader["odpoved_spatna_2"], (string)reader["odpoved_spatna_3"], reader["kategorie"] + "" };
                    reader.Close();
                    return result;
                } 
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine("Error: " + ex.Message);
            }
            return new string[1];
        }

        public void closeConnection()
        {
            connection.Close();
        }
    }
}
