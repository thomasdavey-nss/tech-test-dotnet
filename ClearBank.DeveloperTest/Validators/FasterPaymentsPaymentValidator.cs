using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class FasterPaymentsPaymentValidator : IPaymentValidator
{
    public bool IsValid(Account account, decimal amount) =>
        account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
        account.Balance >= amount;
}