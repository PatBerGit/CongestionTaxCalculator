using congestion.calculator;
using CongestionModels.Models;
using CongestionModels.Models.Configs;

namespace congestion_tax_calculator_tests
{
    [TestClass]
    public class ContestionTaxCalculatorTests
    {

        private readonly ICongestionCalculator calculator;
        public ContestionTaxCalculatorTests()
        {
            IEnumerable<DateOnly> _holyDates = new DateOnly[] { new DateOnly(2013,1,1),
            new DateOnly(2013, 3, 28),
            new DateOnly(2013, 3,29),
            new DateOnly(2013,4,1) };

            TaxPeriods _rangeTaxesList = new TaxPeriods();
            _rangeTaxesList.TaxHours = new List<TaxHours> {
                new TaxHours { StartOnly = TimeOnly.Parse("06:00"), EndOnly = TimeOnly.Parse("06:29"), Tax = 8 },
                new TaxHours { StartOnly = TimeOnly.Parse("06:30"), EndOnly = TimeOnly.Parse("06:59"), Tax = 13 },
                new TaxHours { StartOnly = TimeOnly.Parse("07:00"), EndOnly = TimeOnly.Parse("07:59"), Tax = 18 },
                new TaxHours { StartOnly = TimeOnly.Parse("08:00"), EndOnly = TimeOnly.Parse("08:29"), Tax = 13 },
                new TaxHours { StartOnly = TimeOnly.Parse("08:30"), EndOnly = TimeOnly.Parse("14:59"), Tax = 8 },
                new TaxHours { StartOnly = TimeOnly.Parse("15:00"), EndOnly = TimeOnly.Parse("15:29"), Tax = 13 },
                new TaxHours { StartOnly = TimeOnly.Parse("15:30"), EndOnly = TimeOnly.Parse("16:59"), Tax = 18 },
                new TaxHours { StartOnly = TimeOnly.Parse("17:00"), EndOnly = TimeOnly.Parse("17:59"), Tax = 13 },
                new TaxHours { StartOnly = TimeOnly.Parse("18:00"), EndOnly = TimeOnly.Parse("18:29"), Tax = 8 },
                new TaxHours { StartOnly = TimeOnly.Parse("18:30"), EndOnly = TimeOnly.Parse("05:59"), Tax = 0 }
            };

            List<string> _freeTollVehicles = new List<string> { "Motorcycle", "Tractor", "Emergency", "Diplomat", "Foreign", "Military" };
            calculator = new CongestionCalculator(_holyDates, _rangeTaxesList, _freeTollVehicles, 60, 60);
        }

        [TestMethod]
        public void GetTollFee_ShouldReturnZeroForHoliday()
        {
            var car = new Vehicle("Car");
            var result = calculator.GetTollFee(car, new DateTime(2013, 1, 1, 10, 0, 0));
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_ShouldReturnZeroForFreeTollVehicle()
        {
            var car = new Vehicle("Emergency");
            var result = calculator.GetTollFee(car, new DateTime(2013, 10, 1, 10, 0, 0));
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_ShouldReturnZeroForJuly()
        {
            var car = new Vehicle("Car");
            var result = calculator.GetTollFee(car, new DateTime(2013, 7, 1, 10, 0, 0));
            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public void GetTollFee_ShouldReturnZeroForSaturday()
        {
            var car = new Vehicle("Car");
            var result = calculator.GetTollFee(car, new DateTime(2013, 1, 5, 10, 0, 0));
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_ShouldReturnZeroForSunday()
        {
            var car = new Vehicle("Cars");
            var result = calculator.GetTollFee(car, new DateTime(2013, 1, 6, 10, 0, 0));
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTollFee_ShouldReturnEightForCarInDatePeriod()
        {
            var car = new Vehicle("Car");
            var result = calculator.GetTollFee(car, new DateTime(2013, 10, 1, 6, 10, 0));
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void GetTax_ShouldReturn112ForCarInDatePeriod()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 3, 11, 8, 10, 0));  //13
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));  //8
            dates.Add(new DateTime(2013, 5, 15, 12, 10, 0));  //8
            dates.Add(new DateTime(2013, 9, 1, 16, 10, 0));  //18
            dates.Add(new DateTime(2013, 10, 17, 6, 10, 0));  //8
            dates.Add(new DateTime(2013, 12, 21, 8, 10, 0)); //13
            dates.Add(new DateTime(2013, 12, 11, 8, 10, 0));  //13

            var car = new Vehicle("Car");
            var result = calculator.GetTax(car, dates);
            Assert.AreEqual(112, result);
        }

        [TestMethod]
        public void GetTax_ShouldReturnZeroForMotorcycleInDatePeriod()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 3, 11, 8, 10, 0));  //13
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));  //8
            dates.Add(new DateTime(2013, 5, 15, 12, 10, 0));  //8
            dates.Add(new DateTime(2013, 9, 1, 16, 10, 0));  //18
            dates.Add(new DateTime(2013, 10, 17, 6, 10, 0));  //8
            dates.Add(new DateTime(2013, 12, 21, 8, 10, 0)); //13

            var moto = new Vehicle("Motorcycle");
            var result = calculator.GetTax(moto, dates);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetTax_ShouldReturn104ForCarInDatePeriod()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 3, 11, 8, 10, 0));   //13
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));   //8
            dates.Add(new DateTime(2013, 4, 12, 9, 10, 0));   //8
            dates.Add(new DateTime(2013, 5, 15, 12, 10, 0));   //8
            dates.Add(new DateTime(2013, 9, 1, 16, 10, 0));   //18
            dates.Add(new DateTime(2013, 10, 21, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 10, 21, 7, 10, 0));   //18

            var car = new Vehicle("Car");
            var result = calculator.GetTax(car, dates);
            Assert.AreEqual(96, result);
        }

        [TestMethod]
        public void GetTax_ShouldReturnMax60ForCarInOneDay()
        {
            var dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 1, 2, 6, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 15, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 6, 37, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 7, 19, 0));   //18
            dates.Add(new DateTime(2013, 1, 2, 8, 10, 0));   //13
            dates.Add(new DateTime(2013, 1, 2, 9, 10, 0));   //8
            dates.Add(new DateTime(2013, 1, 2, 12, 10, 0));  //8
            dates.Add(new DateTime(2013, 1, 2, 16, 10, 0));  //18
            var car = new Vehicle("Car");
            var result = calculator.GetTax(car, dates);
            Assert.AreEqual(60, result);
        }
    }
}