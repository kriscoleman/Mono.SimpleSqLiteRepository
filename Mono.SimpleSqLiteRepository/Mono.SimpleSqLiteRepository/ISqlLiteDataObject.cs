using System.Collections.Generic;

namespace Mono.SimpleSqLiteRepository
{
    public interface ISqlLiteDataObject
    {
        int Id { get; set; }

        /// <summary>
        /// Gets your thing by ID
        /// </summary>
        ISqlLiteDataObject Get(SimpleSqlLiteRepository repository, int id);

        /// <summary>
        /// Read all the things
        /// </summary>
        IList<ISqlLiteDataObject> GetAll(SimpleSqlLiteRepository repository);

        /// <summary>
        /// Update a thing
        /// </summary>
        int Save(SimpleSqlLiteRepository repository, ISqlLiteDataObject item);

        /// <summary>
        /// Kill a thing
        /// </summary>
        int Delete(SimpleSqlLiteRepository repository, int id);
    }
}