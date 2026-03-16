using System;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class PaymentValidatorFactory : IPaymentValidatorFactory
{
    public IPaymentValidator GetValidator(PaymentScheme paymentScheme)
    {
        return paymentScheme switch
        {
            PaymentScheme.Bacs => new BacsPaymentValidator(),
            PaymentScheme.FasterPayments => new FasterPaymentsPaymentValidator(),
            PaymentScheme.Chaps => new ChapsPaymentValidator(),
            _ => throw new ArgumentOutOfRangeException(nameof(paymentScheme),
                $"Unsupported payment scheme: {paymentScheme}")
        };
    }
}