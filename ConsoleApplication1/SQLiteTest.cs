using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;

namespace AppRunningLogger
{
    class SQLiteTest
    {
        class AppDefinition : IComparable<AppDefinition>
        {
            [PrimaryKey, Unique]
            long Id { get; set; } = 0;
            [Unique]
            string Path { get; set; } = "";

            public int CompareTo(AppDefinition other)
            {
                return Id.CompareTo(other.Id);
            }
        }

        class AppRunningLog
        {
            [PrimaryKey, AutoIncrement]
            long Id { get; set; } = 0;
            [NotNull, Indexed]
            long AppId { get; set; } = 0;
            bool Active { get; set; } = false;
            DateTime Begin { get; set; } = DateTime.MinValue;
            DateTime End { get; set; } = DateTime.MinValue;
        }

        SQLiteConnection newSQLiteConnection(string dbName)
        {
            return new SQLiteConnection(new SQLitePlatformWin32(), dbName, true);
        }
    }
}
