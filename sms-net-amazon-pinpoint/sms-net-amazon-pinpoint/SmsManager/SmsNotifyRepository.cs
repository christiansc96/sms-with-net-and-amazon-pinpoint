using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;
using Amazon.Runtime;
using sms_net_amazon_pinpoint.Exceptions;
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
            string? cleanPhoneNumber = NormalizePhoneNumber(model.PhoneNumber);
            if (!string.IsNullOrEmpty(model.Message))
            {
                BasicAWSCredentials awsCredentials = new(awsAccessKey, awsSecretKey);
                using AmazonPinpointClient client = new(awsCredentials, RegionEndpoint.GetBySystemName(awsRegion));
                SendMessagesResponse response = await client.SendMessagesAsync(new SendMessagesRequest
                {
                    ApplicationId = awsProjectId,
                    MessageRequest = new MessageRequest
                    {
                        Addresses = new Dictionary<string, AddressConfiguration>
                        {
                            {
                                cleanPhoneNumber!,
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
                                Body = model.Message,
                                MessageType = awsMessageType,
                            }
                        }
                    }
                });
            }
            else
                throw new InvalidPhoneNumberException("El mensaje no puede estar vacío.");
        }

        private static string? NormalizePhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new InvalidPhoneNumberException("El número de teléfono no puede estar vacío.");

            string cleanPhoneNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "");
            if (cleanPhoneNumber.StartsWith("+504"))
                return cleanPhoneNumber;

            if (cleanPhoneNumber.StartsWith("+1"))
                cleanPhoneNumber = cleanPhoneNumber.Substring(2);

            if (cleanPhoneNumber.Length != 10 || !long.TryParse(cleanPhoneNumber, out _))
                throw new InvalidPhoneNumberException("El número de teléfono no es un número válido de 10 dígitos.");

            return $"+1{cleanPhoneNumber}";
        }
    }
}