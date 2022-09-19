using CongestionModels.Models;
using System;
using System.Collections.Generic;

namespace congestion.calculator
{
    public interface ICongestionCalculator
    {
        int GetTax(Vehicle vehicle, List<DateTime> dates);
        int GetTollFee(Vehicle vehicle, DateTime date);
    }
}
