namespace SoccerManager.Domain.Entities
{
    // Users
    public class UserEntity
    {
        public long Id { get; set; } // Id (Primary key)
        public string Username { get; set; } = null!;// Username (length: 100)
        public string Password { get; set; } = null!;// Password (length: 100)
        public DateTime CreatedAt { get; set; } // CreatedAt

        // Reverse navigation

        /// <summary>
        /// Parent (One-to-One) User pointed by [Teams].[UserId] (FK_Teams_Users)
        /// </summary>
        public virtual TeamEntity? Team { get; set; } // Teams.FK_Teams_Users

        private UserEntity() { } // Constructor to be used by EF

        public UserEntity(string username, string password, DateTime createdAt)
        {
            Username = username;
            Password = password;
            CreatedAt = createdAt;
        }

        public UserEntity(long id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
