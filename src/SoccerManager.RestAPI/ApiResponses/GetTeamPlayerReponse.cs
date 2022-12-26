namespace SoccerManager.RestAPI.ApiResponses
{
    public class GetTeamPlayerReponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public int Age { get; set; }
        public decimal MarketValue { get; set; }
    }
}
