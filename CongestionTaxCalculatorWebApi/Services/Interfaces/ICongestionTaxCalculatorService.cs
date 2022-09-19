using CongestionModels.Models;

namespace CongestionTaxCalculatorWebApi.Services.Interfaces
{
    public interface ICongestionTaxCalculatorService
    {
        int GetTax(Vehicle vehicle, List<DateTime> dates);
        int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
