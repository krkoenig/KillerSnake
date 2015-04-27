using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
// TODO add sqlite
using scMessage;

namespace LearnToDev01
{
    class User
    {
        public static message login(message inc)
        {
            string username = inc.getSCObject(0).getString("username");
            string password = inc.getSCObject(0).getString("password");

            string hash = getHashed(password);

            // Build the message
            scBool mLogged = isInDatabase;
            return new message("temp");
        }

        public static message register(message inc)
        {
            return new message("temp");
        }

        private static string getHashed(string pass)
        {
            MD5 encrypt = new MD5CryptoServiceProvider();
            encrypt.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            byte[] hash = encrypt.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                strBuilder.Append(hash[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private static bool isInDatabase(string username, string hash)
        {
            SqliteConnection myConnection = new SqliteConnection();
            myConnection.ConnectionString = "URI=file:" + Application.dataPath + "/killer_snake.db";
            myConnection.Open();

            string query = "SELECT * FROM users WHERE username = '" + username.ToUpper() + "';";
            SqliteCommand cmd = new SqliteCommand(query, myConnection);
            SqliteDataReader rdr = cmd.ExecuteReader();

            string dbHash = "";
            if (rdr.Read())
            {
                dbHash = rdr.GetString(1);
            }

            return hash.Equals(dbHash);
        }

    }

    
}
