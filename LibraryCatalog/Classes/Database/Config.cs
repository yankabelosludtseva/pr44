using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace LibraryCatalog.Classes.Database
{
    public class Config
    {
        public static string Connection = "server=127.0.0.1;uid=root;pwd=;database=LibraryCatalog;";
        public static MySqlServerVersion Version = new MySqlServerVersion(new Version(8, 0, 30));
    }
}