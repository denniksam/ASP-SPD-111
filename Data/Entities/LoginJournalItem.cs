namespace ASP_SPD_111.Data.Entities
{
    /// <summary>
    /// Інформація про входи у систему
    /// </summary>
    public class LoginJournalItem
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Moment { get; set; }
    }
}
