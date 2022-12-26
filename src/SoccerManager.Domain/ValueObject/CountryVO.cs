namespace SoccerManager.Domain.ValueObject
{
    public class CountryVO
    {
        public static string GetRandomCountry()
        {
            var countries = new string[]
            {
                "Brazil",
                "Germany",
                "Italy",
                "France",
                "Argentina",
                "Spain",
                "England",
                "Uruguay",
                "Netherlands",
                "Portugal",
                "Belgium",
                "Denmark",
                "Switzerland",
                "Colombia",
                "Sweden",
                "Croatia",
                "Poland",
                "Japan",
                "Chile",
                "Wales",
                "Mexico",
                "Iran",
                "Morocco",
                "Peru",
                "United States",
                "Senegal",
                "Serbia",
                "Australia",
            };
            var index = new Random().Next(countries.Length);
            return countries[index];
        }
    }
}
