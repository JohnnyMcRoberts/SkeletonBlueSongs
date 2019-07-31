namespace SongsDatabase.Base
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public abstract class BaseMongoEntity
    {
        /// <summary>
        /// Gets or sets the unique entity identifier.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets the name to use for equivalence checks.
        /// </summary>
        public abstract string EquivalenceName { get; }
    }
}
