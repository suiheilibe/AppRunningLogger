using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Data.SQLite;

namespace AppRunningTimer
{
    class AppManager
    {
        private const string DB_NAME = "art.db";
        private bool inited = false;
        static void AddAppEntry()
        {
            new SQLiteConnection("");
        }
        string IdentFromProcess(Process process)
        {
            return "";
        }

        void InitDatabase()
        {
            if (inited)
            {
                return;
            }
            var appsTable =
@"create table app (
id integer primary key,
name text)";
            var rulesTable =
@"create table rule (
id integer,
appid integer,
name text)";
            var conn = new SQLiteConnection("Data Source=" + DB_NAME);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "";
            conn.Open();
            inited = true;
            return;
        }
    }
}
