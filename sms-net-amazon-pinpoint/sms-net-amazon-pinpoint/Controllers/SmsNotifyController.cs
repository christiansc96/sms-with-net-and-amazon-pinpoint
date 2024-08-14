using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using sms_net_amazon_pinpoint.Filter;
using sms_net_amazon_pinpoint.Models;
using sms_net_amazon_pinpoint.SmsManager;

namespace sms_net_amazon_pinpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class SmsNotifyController : ControllerBase
    {
        private readonly ISmsNotifyRepository _smsNotifyRepository;

        public SmsNotifyController(ISmsNotifyRepository smsNotifyRepository)
        {
            this._smsNotifyRepository = smsNotifyRepository;
        }

        [HttpPost]
        [RequireHeaderKey("X-Api-Key")]
        public async Task<IActionResult> SendSms(SmsNotifyDto model)
        {
            try
            {
                await _smsNotifyRepository.Execute(model);
                return Ok(new
                {
                    Message = "SMS enviado exitosamente",
                    StatusCode = 200
                });
            }
            catch(Exception ex)            
            {
                return BadRequest(new
                {
                    Message = "Error al enviar el SMS",
                    StatusCode = 400,
                    Error = ex.Message,
                });
            }
        }
    }
}