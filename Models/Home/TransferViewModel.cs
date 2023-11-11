namespace ASP_SPD_111.Models.Home
{
    /* Модель з даними, необхідними для відображення
     * сторінки /Home/Transfer
     */
    public record TransferViewModel
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public String ControllerName { get; set; } = null!;
    }
}
