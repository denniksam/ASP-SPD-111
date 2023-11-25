namespace ASP_SPD_111.Data.Entities
{
    public class User
    {
        public Guid      Id           { get; set; }
        public String    Login        { get; set; } = null!;
        public String?   Name         { get; set; }
        public String    Email        { get; set; } = null!;
        public String    PasswordSalt { get; set; } = null!;
        public String    PasswordDk   { get; set; } = null!;  // Derived Key (RFC 2898)
        public String?   Avatar       { get; set; }  // filename/URL
        public DateTime  RegisterDt   { get; set; }
        public DateTime? DeleteDt     { get; set; }
    }
}
