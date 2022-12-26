using Microsoft.AspNetCore.Mvc;

namespace SoccerManager.RestAPI.ApiInputs
{
    public class UpdateTeamInput
    {        
        public string? TeamName { get; set; }
        public string? TeamCountry { get; set; }
    }
}
