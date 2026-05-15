using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCatalog.Classes.Database
{
    public class Config
    {
        public static string connection = "server=localhost; uid=root; pwd=; database=LibraryCatalog;";
        public static MySqlServerVersion Version = new MySqlServerVersion(new Version(8, 0, 30));
    }
}
