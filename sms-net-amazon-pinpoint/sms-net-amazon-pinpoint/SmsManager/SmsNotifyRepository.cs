using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;
using Amazon.Runtime;
using sms_net_amazon_pinpoint.Models;

namespace sms_net_amazon_pinpoint.SmsManager
{
    public class SmsNotifyRepository : ISmsNotifyRepository
    {
        private static readonly string awsAccessKey = "awsAccessKey"; //Access Key for the user.
        private static readonly string awsSecretKey = "awsSecretKey"; //Secret Access Key for the user.
        private static readonly string awsRegion = "awsRegion"; //Your Region
        private static readonly string awsMessageType = "awsMessageType"; //Type of message TRANSACTIONAL/PROMOTIONAL
        private static readonly string awsProjectId = "awsProjectId"; //Project ID for Pinpoint Project

        public async Task Execute(SmsNotifyDto model)
        {
            string phoneNumberCode = "+504";

            var awsCredentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
            using AmazonPinpointClient client = new(awsCredentials, RegionEndpoint.GetBySystemName(awsRegion));

            string cleanPhoneNumber = $"{phoneNumberCode}{model.PhoneNumber?.Trim().Replace("-", string.Empty)}";
            var response = await client.SendMessagesAsync(new SendMessagesRequest
            {
                ApplicationId = awsProjectId,
                MessageRequest = new MessageRequest
                {
                    Addresses = new Dictionary<string, AddressConfiguration>
                        {
                            {
                                cleanPhoneNumber,
                                new AddressConfiguration
                                {
                                    ChannelType = "SMS"
                                }
                            }
                        },
                    MessageConfiguration = new DirectMessageConfiguration
                    {
                        SMSMessage = new SMSMessage
                        {
                            Body = model.Message ?? "",
                            MessageType = awsMessageType,
                        }
                    }
                }
            });
            var x = response;
        }
    }
}