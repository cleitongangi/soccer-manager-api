namespace SoccerManager.RestAPI.ApiResponses
{
    public class LoginReponse
    {
        public LoginReponse(string token, DateTime expiration)
        {
            Token = token;
            Expiration = expiration;
        }

        public string Token { get; set; }
        public DateTime Expiration { get; set; }        
    }
}
