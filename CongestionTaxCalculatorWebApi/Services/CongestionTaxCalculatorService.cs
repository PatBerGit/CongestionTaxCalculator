using congestion.calculator;
using CongestionModels.Models;
using CongestionTaxCalculatorWebApi.Services.Interfaces;

namespace CongestionTaxCalculatorWebApi.Services
{
    public class TaxCalculatorService : ICongestionTaxCalculatorService
    {
        private readonly ICongestionCalculator _congestionTaxCalculator;

        public TaxCalculatorService(ICongestionCalculator congestionTaxCalculator)
        {
            _congestionTaxCalculator = congestionTaxCalculator;
        }

        public int GetTax(Vehicle vehicle, List<DateTime> dates)
        {
            return _congestionTaxCalculator.GetTax(vehicle, dates);
        }
        public int GetTollFee(Vehicle vehicle, DateTime date)
        {
            return _congestionTaxCalculator.GetTollFee(vehicle, date);
        }
    }
}
