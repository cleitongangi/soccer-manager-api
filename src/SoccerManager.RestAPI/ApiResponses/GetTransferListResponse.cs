namespace SoccerManager.RestAPI.ApiResponses
{
    public class GetTransferListResponse
    {
        public long TransferId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public PlayerResponse? Player { get; set; }
        public SourceTeamResponse? SourceTeam { get; set; }

        public class PlayerResponse
        {
            public long Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Country { get; set; }
            public int Age { get; set; }       
            public string? PositionName { get; set; }
        }

        public class SourceTeamResponse
        {
            public long TeamId { get; set; }
            public string? TeamName { get; set; }
            public string? TeamCountry { get; set; }
        }
    }
}
