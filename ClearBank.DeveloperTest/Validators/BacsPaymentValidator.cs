using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class BacsPaymentValidator : IPaymentValidator
{
    public bool IsValid(Account account, decimal _) =>
        account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
}