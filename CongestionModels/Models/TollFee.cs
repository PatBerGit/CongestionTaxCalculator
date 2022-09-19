namespace CongestionModels.Models
{
    public record TollFee
    {
        public Vehicle Vehicle { get; set; }
        public DateTime Date { get; set; }
    }
}
