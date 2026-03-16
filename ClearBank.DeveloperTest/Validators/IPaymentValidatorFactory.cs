using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentValidatorFactory
{
    IPaymentValidator GetValidator(PaymentScheme paymentScheme);
}