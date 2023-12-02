using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ASP_SPD_111.Models.Home
{
    public class SignupFormModel
    {
        [FromForm(Name = "signup-login")]
        public String Login { get; set; } = null!;

        [FromForm(Name = "signup-name")]
        public String Name { get; set; } = null!;

        [FromForm(Name = "signup-email")]
        public String Email { get; set; } = null!;

        [FromForm(Name = "signup-password")]
        public String Password { get; set; } = null!;

        [FromForm(Name = "signup-repeat")]
        public String Repeat { get; set; } = null!;

        [FromForm(Name = "signup-avatar")]
        [JsonIgnore]
        public IFormFile Avatar { get; set; } = null!;
    }
}
