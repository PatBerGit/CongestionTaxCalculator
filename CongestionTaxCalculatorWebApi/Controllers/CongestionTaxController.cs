using CongestionModels.Models;
using CongestionTaxCalculatorWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculatorWebApi.Controllers
{
    public class CongestionTaxController : Controller
    {
        private readonly ICongestionTaxCalculatorService _congestionTaxCalculatorService;

        public CongestionTaxController(ICongestionTaxCalculatorService congestionTaxCalculatorService)
        {
            _congestionTaxCalculatorService = congestionTaxCalculatorService;
        }

        [HttpPost("GetTax")]
        public ActionResult GetTax([FromBody] Tax tax)
        {
            try
            {
                var result = _congestionTaxCalculatorService.GetTax(tax.Vehicle, tax.Dates);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPost("GetTollFee")]
        public ActionResult GetTollFee([FromBody] TollFee tollFee)
        {
            try
            {
                var result = _congestionTaxCalculatorService.GetTollFee(tollFee.Vehicle, tollFee.Date);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
