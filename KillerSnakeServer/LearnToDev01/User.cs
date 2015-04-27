using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using scMessage;

namespace LearnToDev01
{
    class User
    {
        public static message login(message inc)
        {
            // Build the message
            scBool mLogged = isLogged;
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


    }

    
}
