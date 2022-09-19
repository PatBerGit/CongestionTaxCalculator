using System;
using System.Collections.Generic;
using CongestionModels.Models;
using CongestionModels.Models.Configs;
using System.Linq;

namespace congestion.calculator
{
    public class CongestionCalculator : ICongestionCalculator
    {
        /**
       * Calculate the total toll fee for one a vehicle
       *
       * @param vehicle - the vehicle
       * @param dates   - date and time of all passes of one vehicle
       * @return - the total congestion tax for that vehicle
       */

        private readonly IEnumerable<DateOnly> _holidays;
        private readonly TaxPeriods _TaxPeriodsList;
        private readonly List<string> _freeTollVehicles;
        private readonly double _gracePeriod;
        private readonly int _dayMaxFee;

        public CongestionCalculator(IEnumerable<DateOnly> holidays, TaxPeriods taxPeriodsList, List<string> freeTollVehicles, double gracePeriod, int dayMaxFee)
        {
            _holidays = holidays;
            _TaxPeriodsList = taxPeriodsList;
            _freeTollVehicles = freeTollVehicles;
            _gracePeriod = gracePeriod;
            _dayMaxFee = dayMaxFee;
        }

        #region Public methods
        public int GetTax(Vehicle vehicle, List<DateTime> dates)
        {
            if (IsTollFreeVehicle(vehicle)) return 0;

            var daysFee = new List<Tuple<DateOnly, int>>();
            int totalFee = 0;
            var datesByDay = dates.GroupBy(x => DateOnly.FromDateTime(x.Date));

            foreach (var day in datesByDay)
            {
                int dayFee = 0;
                if (!IsTollFreeDate(day.Key.ToDateTime(TimeOnly.Parse("12:00 AM"))))
                {
                    var pivotDay = day.Select(x => x).ToList();
                    while (pivotDay.Count() > 0)
                    {
                        var tollsInPeriod = pivotDay.Where(x => x <= pivotDay.FirstOrDefault().AddMinutes(_gracePeriod));
                        var hourFeePivot = 0;
                        foreach (var toll in tollsInPeriod)
                        {
                            hourFeePivot = Math.Max(hourFeePivot, GetTimeFee(TimeOnly.FromDateTime(toll)));
                        }
                        dayFee += hourFeePivot;
                        if (dayFee >= _dayMaxFee)
                        {
                            dayFee = _dayMaxFee;
                            pivotDay.Clear();
                        }
                        pivotDay = pivotDay.Where(x => !tollsInPeriod.Contains(x)).ToList();
                        Console.WriteLine(pivotDay.Count());
                    }
                    daysFee.Add(Tuple.Create(day.Key, dayFee));
                    totalFee += dayFee;
                }
            }
            return totalFee;
        }

        public int GetTollFee(Vehicle vehicle, DateTime date)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            TimeOnly time = TimeOnly.FromDateTime(date);
            return GetTimeFee(time);
        }

        #endregion

        #region Private methods
        private int GetTimeFee(TimeOnly time)
        {
            return _TaxPeriodsList.TaxHours
                            .Where(x => time.IsBetween(x.StartOnly, x.EndOnly))
                            .Select(x => x.Tax).FirstOrDefault();
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            string vehicleType = vehicle.VehicleType;

            return _freeTollVehicles.Any(x => x.Equals(vehicleType, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsTollFreeDate(DateTime date)
        {
            return IsWeekendOrJuly(date)
                ? true : _holidays.Any(d => d == DateOnly.FromDateTime(date));
        }

        private static bool IsWeekendOrJuly(DateTime date)
        {
            return date.Month == 7 || date.ToString("dddd") == "Saturday" || date.ToString("dddd") == "Sunday";
        }
        #endregion
    }
}
