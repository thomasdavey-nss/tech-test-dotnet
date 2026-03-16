namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        public bool Success { get; set; }
        
        public static MakePaymentResult Succeeded() => new MakePaymentResult { Success = true };
        public static MakePaymentResult Failed() => new MakePaymentResult { Success = false };
    }
}
