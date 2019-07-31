namespace SongsDatabase.Users
{
    using MongoDB.Bson.Serialization.Attributes;

    using Base;
    using Security;

    [BsonIgnoreExtraElements]
    public class User : BaseEntity
    {
        #region Constants

        private const string HashAlgorithm = "SHA512";
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the hash of the user password.
        /// </summary>
        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [BsonElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the description of the user.
        /// </summary>
        [BsonElement("description")]
        public string Description { get; set; }

        #endregion

        #region Public methods

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return SimpleHash.VerifyHash(password, HashAlgorithm, PasswordHash);
        }


        public bool UpdatePassword(string password)
        {
            // Check an actual password
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Check a changed password
            byte[] salt = null;
            var passwordHash = SimpleHash.ComputeHash(password, HashAlgorithm, ref salt);
            if (PasswordHash == passwordHash)
                return false;

            // Update & say OK
            PasswordHash = passwordHash;
            return true;
        }

        #endregion

        #region Constructors

        public User(string name, string password, string email, string description)
        {
            Name = name;
            Description = description;
            Email = email;
            byte[] salt = null;
            PasswordHash = SimpleHash.ComputeHash(password, HashAlgorithm, ref salt);
        }

        public User()
        {
            Name = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
        }

        #endregion
    }
}
