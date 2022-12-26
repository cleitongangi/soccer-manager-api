namespace SoccerManager.RestAPI.ApiResponses
{
    public class GetTeamResponse
    {
        public GetTeamResponse(long teamId, string teamName, string teamCountry, decimal teamValue)
        {
            TeamId = teamId;
            TeamName = teamName;
            TeamCountry = teamCountry;
            TeamValue = teamValue;
        }

        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamCountry { get; set; }
        /// <summary>
        /// Sum of the market values of all players on the team
        /// </summary>
        public decimal TeamValue { get; set; }
    }
}
