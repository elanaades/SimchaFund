namespace March_29_Homework_Simcha_Fund.Data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string SimchaName { get; set; }
        public DateTime SimchaDate { get; set; }
        public int ContributorCount { get; set; }
        public decimal TotalContributed { get; set; }
    }
}