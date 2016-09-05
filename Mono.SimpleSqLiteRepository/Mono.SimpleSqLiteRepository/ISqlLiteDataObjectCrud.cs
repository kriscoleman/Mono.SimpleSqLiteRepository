using System.Data;
using Mono.Data.Sqlite;

namespace Mono.SimpleSqLiteRepository
{
    public interface ISqlLiteDataObjectCrud<T> where T : ISqlLiteDataObject
    {
        /// <summary>
        ///     The TSQL string for creating the table for Piggy Banks
        /// </summary>
        string CreateSqlLiteTable();

        /// <summary>
        ///     Read (Get) all the things
        /// </summary>
        string ReadAll();

        /// <summary>
        ///     Read (Get) a thing
        /// </summary>
        string Read();

        /// <summary>
        ///     Parses our IDataRecord into a PiggyBank
        /// </summary>
        T FromReader(IDataRecord r);

        /// <summary>
        ///     Update a thing
        /// </summary>
        void Update(T item, SqliteCommand command);

        /// <summary>
        ///     Insert a thing
        /// </summary>
        void Insert(T item, SqliteCommand command);

        /// <summary>
        ///     Delete a thing
        /// </summary>
        void Delete(int id, SqliteCommand command);
    }
}