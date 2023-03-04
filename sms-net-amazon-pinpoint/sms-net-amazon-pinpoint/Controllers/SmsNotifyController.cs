using Microsoft.AspNetCore.Mvc;
using sms_net_amazon_pinpoint.Models;
using sms_net_amazon_pinpoint.SmsManager;

namespace sms_net_amazon_pinpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsNotifyController : ControllerBase
    {
        private readonly ISmsNotifyRepository _smsNotifyRepository;

        public SmsNotifyController(ISmsNotifyRepository smsNotifyRepository)
        {
            this._smsNotifyRepository = smsNotifyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SendSms(SmsNotifyDto model)
        {
            try
            {
                await _smsNotifyRepository.Execute(model);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}