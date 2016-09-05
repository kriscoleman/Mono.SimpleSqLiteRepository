using System.Collections.Generic;

namespace Mono.SimpleSqLiteRepository
{
    /// <summary>
    /// Sample Type-Scoped Repository
    /// </summary>
    public class MyRepo : SimpleSqlLiteRepository
    {
        public static MyRepo Singleton = new MyRepo();

        protected MyRepo()
        {
            // set the _db location
            DbLocation = GetFilePathFormattedForPlatform("MyRepo");

            // instantiate the database	
            Db = new SimpleSqLiteDatabase(DbLocation, new Note.NoteCrud().CreateSqlLiteTable());
        }

        internal new ISqlLiteDataObject Get<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : MyReproSqlLiteObject { return Singleton.Db.Get(id, crud); }

        internal new IEnumerable<T> GetAll<T>(ISqlLiteDataObjectCrud<T> crud) where T : MyReproSqlLiteObject { return Singleton.Db.GetAll(crud); }

        internal new int Save<T>(T item, ISqlLiteDataObjectCrud<T> crud) where T : MyReproSqlLiteObject { return Singleton.Db.Update(item, crud); }

        internal new int Delete<T>(int id, ISqlLiteDataObjectCrud<T> crud) where T : MyReproSqlLiteObject { return Singleton.Db.Delete(id, crud); }
    }
}