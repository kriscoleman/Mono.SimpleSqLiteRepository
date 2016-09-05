using System;
using System.Collections.Generic;

namespace Mono.SimpleSqLiteRepository
{
    public abstract class SimpleSqlLiteRepository
    {
        protected static string DbLocation;
        protected SimpleSqLiteDatabase Db;

        /// <summary>
        /// Returns the Database File Path in the required platform-specific format
        /// </summary>
        internal static string DatabaseFilePath { get { return GetFilePathFormattedForPlatform(); } }

        internal virtual ISqlLiteDataObject Get<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject { return Db.Get(id, crud); }
        internal virtual IEnumerable<T> GetAll<T>(ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject { return Db.GetAll(crud); }
        internal virtual int Save<T>(T item, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject { return Db.Update(item, crud); }
        internal virtual int Delete<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : class, ISqlLiteDataObject { return Db.Delete(id, crud); }

        protected static string GetFilePathFormattedForPlatform(string sqlLiteDbFileName = "SimpleSqLiteDatabase") //todo: consider making this a required parameter. if multiple repositories didn't define a different file name, they'd all be opening connections to the same db file
        {
            sqlLiteDbFileName = string.Join(sqlLiteDbFileName, ".db3");

            // this is where I usually have a platform aware class generate my file paths, but for now I'll just use this:
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var path = System.IO.Path.Combine(libraryPath, sqlLiteDbFileName);

            return path;
        }
    }
}