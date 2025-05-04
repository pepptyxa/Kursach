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
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command.ExecuteReader();
        }
        public static void CloseConnection(MySqlConnection connection) 
        {
            connection.Close();
        }
    }
}
