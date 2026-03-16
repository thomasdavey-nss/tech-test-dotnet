using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(IDataStore dataStore, IPaymentValidatorFactory paymentValidatorFactory) : IPaymentService
    {
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = dataStore.GetAccount(request.DebtorAccountNumber);
            
            if (account == null) 
                return MakePaymentResult.Failed();
            
            var validator = paymentValidatorFactory.GetValidator(request.PaymentScheme);
            if (!validator.IsValid(account, request.Amount)) 
                return MakePaymentResult.Failed();
            
            account.Balance -= request.Amount;
            dataStore.UpdateAccount(account);

            return MakePaymentResult.Succeeded();
        }
    }
}
