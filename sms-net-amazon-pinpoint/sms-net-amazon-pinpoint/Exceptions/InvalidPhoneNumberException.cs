namespace sms_net_amazon_pinpoint.Exceptions
{
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException() : base("La información es inválida.") { }

        public InvalidPhoneNumberException(string message) : base(message) { }

        public InvalidPhoneNumberException(string message, Exception inner) : base(message, inner) { }
    }
}