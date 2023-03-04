using sms_net_amazon_pinpoint.Models;

namespace sms_net_amazon_pinpoint.SmsManager
{
    public interface ISmsNotifyRepository
    {
        Task Execute(SmsNotifyDto model);
    }
}