using MySql.Data.MySqlClient;

namespace Kursach.Classes.Common
{
    public class DBConnection
    {
        public static readonly string path = "Host=localhost;Username=root;Database=kursach";
        public static MySqlConnection Connection() 
        {
            MySqlConnection connection = new MySqlConnection(path);
            connection.Open();
            return connection;
        }
        public static MySqlDataReader Query(string query, MySqlConnection connection, params MySqlParameter[] parameters) 
        {
            var command = new MySqlCommand(query, connection);
            command.Parameters.Add(parameters);
            return command.ExecuteReader();
        }
        public static void CloseConnection(MySqlConnection connection) 
        {
            connection.Close();
        }
    }
}
