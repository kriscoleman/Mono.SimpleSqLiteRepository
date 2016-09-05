using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;

namespace Mono.SimpleSqLiteRepository.ExampleRepository
{
    /// <summary>
    ///     Note POCO
    /// </summary>
    public class Note : MyReproSqlLiteObject
    {
        readonly NoteCrud _crud = new NoteCrud();

        public override int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }

        public override ISqlLiteDataObjectCrud<MyReproSqlLiteObject> CRUD => (ISqlLiteDataObjectCrud<MyReproSqlLiteObject>) _crud;

        /// <summary>
        ///     Gets your Bank by ID
        /// </summary>
        public override ISqlLiteDataObject Get(SimpleSqlLiteRepository repository, int id) => repository.Get(id, CRUD);

        /// <summary>
        ///     Read all the banks
        /// </summary>
        public override IList<ISqlLiteDataObject> GetAll(SimpleSqlLiteRepository repository) => new List<ISqlLiteDataObject>(repository.GetAll(CRUD));

        /// <summary>
        ///     Update a bank
        /// </summary>
        public override int Save(SimpleSqlLiteRepository repository, ISqlLiteDataObject item) => repository.Save(item as Note, CRUD);

        /// <summary>
        ///     Kill a piggy (bank)
        /// </summary>
        public override int Delete(SimpleSqlLiteRepository repository, int id) => repository.Delete(id, CRUD);

        #region SQL for Data Access

        /// <summary>
        ///     All of the required TSQL and command/params and fromReader-transformations live here.
        /// </summary>
        internal class NoteCrud : ISqlLiteDataObjectCrud<Note>
        {
            /// <summary>
            ///     The TSQL string for creating the table for Piggy Banks
            /// </summary>
            public string CreateSqlLiteTable()
            {
                return string.Format("{0}{1}{2}{3}{4}",
                    string.Format("CREATE TABLE [{0}] ", Constants.TableName),
                    string.Format("({0} INTEGER PRIMARY KEY ASC, ", Constants.Id),
                    string.Format("{0} NTEXT, ", Constants.Name),
                    string.Format("{0} NTEXT, ", Constants.Notes),
                    string.Format("{0} INTEGER);", Constants.Done));
            }

            /// <summary>
            ///     A list of constants for things like field names and the table name
            /// </summary>
            static class Constants
            {
                /// <summary>
                ///     Plural name for Note. Table name should be "Notes"
                /// </summary>
                const string PluralName = "Notes";

                public const string TableName = PluralName;

                // DBO Fields
                internal const string Id = "_id";
                internal const string Name = "Name";
                internal const string Done = "Done";
                internal const string Notes = "Notes";
            }

            #region The GetMethods are strongly-coupled to the FromReader method

            /// <summary>
            ///     Read all the things
            /// </summary>
            public string ReadAll()
            {
                return string.Format("SELECT {0}{1}{2}{3}{4}",
                    string.Format("[{0}], ", Constants.Id),
                    string.Format("[{0}], ", Constants.Name),
                    string.Format("[{0}], ", Constants.Notes),
                    string.Format("[{0}] ", Constants.Done),
                    string.Format("FROM [{0}]", Constants.TableName));
            }

            /// <summary>
            ///     Read a thing
            /// </summary>
            public string Read()
            {
                return string.Format("SELECT {0}{1}{2}{3}{4}{5}",
                    string.Format("[{0}], ", Constants.Id),
                    string.Format("[{0}], ", Constants.Name),
                    string.Format("[{0}], ", Constants.Notes),
                    string.Format("[{0}] ", Constants.Done),
                    string.Format("FROM [{0}] ", Constants.TableName),
                    string.Format("WHERE [{0}] = ?", Constants.Id));
            }

            /// <summary>
            ///     Convert a DataRecord to a thing
            /// </summary>
            public Note FromReader(IDataRecord r)
            {
                var note = new Note
                {
                    Id = Convert.ToInt32(r["_id"]),
                    Name = r["Name"].ToString(),
                    Notes = r["Notes"].ToString(),
                    Done = Convert.ToInt32(r["Done"]) == 1
                };
                return note;
            }

            /// <summary>
            ///     Update a thing
            /// </summary>
            public void Update(Note item, SqliteCommand command)
            {
                command.CommandText = string.Format("{0}SET {1}{2}{3}{4}",
                    string.Format("UPDATE [{0}] ", Constants.TableName),
                    string.Format("[{0}] = ?, ", Constants.Name),
                    string.Format("[{0}] = ?, ", Constants.Notes),
                    string.Format("[{0}] = ? ", Constants.Done),
                    string.Format("WHERE [{0}] = ?;", Constants.Id));
                command.Parameters.Add(new SqliteParameter(DbType.String) {Value = item.Name});
                command.Parameters.Add(new SqliteParameter(DbType.String) {Value = item.Notes});
                command.Parameters.Add(new SqliteParameter(DbType.Int32) {Value = item.Done});
                command.Parameters.Add(new SqliteParameter(DbType.Int32) {Value = item.Id});
            }

            /// <summary>
            ///     Insert a thing
            /// </summary>
            public void Insert(Note item, SqliteCommand command)
            {
                command.CommandText = string.Format("{0}{1}{2}{3}) VALUES (? ,?, ?)",
                    string.Format("INSERT INTO [{0}] (", Constants.TableName),
                    string.Format("[{0}], ", Constants.Name),
                    string.Format("[{0}], ", Constants.Notes),
                    string.Format("[{0}]", Constants.Done));
                command.Parameters.Add(new SqliteParameter(DbType.String) {Value = item.Name});
                command.Parameters.Add(new SqliteParameter(DbType.String) {Value = item.Notes});
                command.Parameters.Add(new SqliteParameter(DbType.Int32) {Value = item.Done});
            }

            /// <summary>
            ///     Delete a thing
            /// </summary>
            public void Delete(int id, SqliteCommand command)
            {
                command.CommandText =
                    string.Format("{0}{1}",
                        string.Format("DELETE FROM [{0}] ", Constants.TableName),
                        string.Format("WHERE [{0}] = ?;", Constants.Id));
                command.Parameters.Add(new SqliteParameter(DbType.Int32) {Value = id});
            }

            #endregion
        }

        #endregion
    }
}