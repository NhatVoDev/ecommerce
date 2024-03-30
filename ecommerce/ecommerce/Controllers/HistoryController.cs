using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }
        // GET api/history
        [HttpGet]
        public async Task<IActionResult> GetAllHistoriesAsync()
        {
            var response = await _historyService.GetAllHistoriesAsync();
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        // GET api/history/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistoryByIdAsync(int id)
        {
            var response = await _historyService.GetHistoryByIdAsync(id);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        // Get api/history/payment/5
        [HttpGet("payment/{paymentId}")]
        public async Task<IActionResult> GetHistoriesByPaymentIdAsync(int paymentId)
        {
            var response = await _historyService.GetHistoriesByPaymentIdAsync(paymentId);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}