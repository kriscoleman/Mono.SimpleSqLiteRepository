using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;

namespace Mono.SimpleSqLiteRepository
{
    // Supporting DB and API architecture begins below. 

    // The Code Below would normally be in it's own library.

    /// <summary>
    ///     The Simplified SqlLite Database uses ADO.NET to create the [Items] table and create,read,update,delete data
    /// </summary>
    public class SimpleSqLiteDatabase
    {
        static readonly object Locker = new object();
        internal SqliteConnection Connection;
        internal string Path;

        /// <summary>
        ///     Initializes a new instance of the SimpleSqLiteDatabase.
        ///     if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        internal SimpleSqLiteDatabase(string dbPath, params string[] createTableSql)
        {
            Path = dbPath;

            // create the tables
            var exists = File.Exists(dbPath);

            if (exists) return;
            Connection = new SqliteConnection("Data Source=" + dbPath);

            Connection.Open();
            var commands = createTableSql.ToArray();

            foreach (var command in commands)
                using (var c = Connection.CreateCommand())
                {
                    c.CommandText = command;
                    var i = c.ExecuteNonQuery();
                }
        }

        #region CRUD for the SQLLiteRepository

        internal IEnumerable<T> GetAll<T>(ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject
        {
            var tl = new List<T>();

            lock (Locker)
            {
                Connection = new SqliteConnection("Data Source=" + Path);
                Connection.Open();
                using (var contents = Connection.CreateCommand())
                {
                    contents.CommandText = crud.ReadAll();
                    var r = contents.ExecuteReader();
                    while (r.Read())
                    {
                        tl.Add(crud.FromReader(r));
                    }
                }
                Connection.Close();
            }
            return tl;
        }

        internal T Get<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject
        {
            lock (Locker)
            {
                Connection = new SqliteConnection("Data Source=" + Path);
                Connection.Open();
                using (var command = Connection.CreateCommand())
                {
                    command.CommandText = crud.Read();
                    command.Parameters.Add(new SqliteParameter(DbType.Int32) {Value = id});
                    var r = command.ExecuteReader();
                    while (r.Read())
                    {
                        var thing = crud.FromReader(r);
                        return thing;
                    }
                }
                Connection.Close();
            }
            return null;
        }

        internal int Update<T>(T item, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject
        {
            lock (Locker)
            {
                int r;
                if (item.Id != 0)
                {
                    Connection = new SqliteConnection("Data Source=" + Path);
                    Connection.Open();
                    using (var command = Connection.CreateCommand())
                    {
                        crud.Update(item, command);
                        r = command.ExecuteNonQuery();
                    }
                    Connection.Close();
                    return r;
                }

                Connection = new SqliteConnection("Data Source=" + Path);
                Connection.Open();
                using (var command = Connection.CreateCommand())
                {
                    crud.Insert(item, command);
                    r = command.ExecuteNonQuery();
                }
                Connection.Close();
                return r;
            }
        }

        internal int Delete<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject
        {
            lock (Locker)
            {
                int r;
                Connection = new SqliteConnection("Data Source=" + Path);
                Connection.Open();
                using (var command = Connection.CreateCommand())
                {
                    crud.Delete(id, command);
                    r = command.ExecuteNonQuery();
                }
                Connection.Close();
                return r;
            }
        }

        #endregion
    }
}