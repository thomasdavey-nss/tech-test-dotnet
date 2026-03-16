using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStore _dataStore;
        private readonly IPaymentValidatorFactory _paymentValidatorFactory;

        public PaymentService(IDataStore dataStore, IPaymentValidatorFactory paymentValidatorFactory)
        {
            _dataStore = dataStore;
            _paymentValidatorFactory = paymentValidatorFactory;
        }
        
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _dataStore.GetAccount(request.DebtorAccountNumber);
            
            if (account == null) 
                return MakePaymentResult.Failed();
            
            var validator = _paymentValidatorFactory.GetValidator(request.PaymentScheme);
            if (!validator.IsValid(account, request.Amount)) 
                return MakePaymentResult.Failed();
            
            account.Balance -= request.Amount;
            _dataStore.UpdateAccount(account);

            return MakePaymentResult.Succeeded();
        }
    }
}
