namespace CongestionModels.Models
{
    public record Vehicle
    {
        public string VehicleType { get; set; }
        public Vehicle(string vehicleType)
        {
            VehicleType = vehicleType;
        }
    }
}
