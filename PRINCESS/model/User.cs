using System.Text.Json.Serialization;

/**
*  authors:
*          @Anna Mann
*          @Gabriel Vega
**/

namespace PRINCESS.model
{
    public class User
    {
        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Usertype { get; set; }
        public string Token { get; set; }
    }
}
