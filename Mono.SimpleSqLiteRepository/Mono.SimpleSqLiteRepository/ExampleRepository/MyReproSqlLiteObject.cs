using System.Collections.Generic;

namespace Mono.SimpleSqLiteRepository
{
    /// <summary>
    ///     Any ISQLLiteDataObject derived from this base are acceptably handled by this Repository.
    /// </summary>
    public abstract class MyReproSqlLiteObject : ISqlLiteDataObject
    {
        public abstract ISqlLiteDataObjectCrud<MyReproSqlLiteObject> CRUD { get; }
        public abstract int Id { get; set; }
        public abstract ISqlLiteDataObject Get(SimpleSqlLiteRepository repository, int id);
        public abstract IList<ISqlLiteDataObject> GetAll(SimpleSqlLiteRepository repository);
        public abstract int Save(SimpleSqlLiteRepository repository, ISqlLiteDataObject item);
        public abstract int Delete(SimpleSqlLiteRepository repository, int id);
    }
}