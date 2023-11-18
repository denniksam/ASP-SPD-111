using Microsoft.AspNetCore.Mvc;

namespace ASP_SPD_111.Models.Home
{
    /* Моделі форм мають автоматичний мапер, тобто значення 
     * полів (властивостей) моделі заповнюються за збігом з
     * іменами полів форми. Однак, часто в HTML прийнято вживати
     * kebab-casing (user-firstname), який не дозволений у C#.
     * У такому разі мапінг зазначається атрибутами
     * FromQuery - з URL (get-параметри)
     * FromForm - з тіла (post-параметри)
     */
    public class TransferFormModel
    {
        [FromForm(Name = "user-firstname")]
        public String UserFirstname { get; set; } = null!;

        [FromForm(Name = "user-lastname")]
        public String UserLastname { get; set; } = null!;
    }
}
