﻿namespace SongsDatabase.Base
{

    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// The base MongoDb entity.
    /// </summary>
    public class BaseEntity : BaseMongoEntity
    {
        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name to use for equivalence checks.
        /// </summary>
        public override string EquivalenceName => Name;
    }
}
