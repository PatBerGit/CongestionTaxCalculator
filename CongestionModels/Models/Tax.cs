namespace CongestionModels.Models
{
    public record Tax
    {
        public Vehicle Vehicle { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}
