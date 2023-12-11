namespace green_craze_be_v1.Application.Model.Mail
{
    public class CreateMailRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string OTP { get; set; }

        public string Email { get; set; }
        public string Title { get; set; }
        public OrderConfirmationMail OrderConfirmationMail { get; set; }
    }

    public class OrderConfirmationMail
    {
        public string Address { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
    }
}