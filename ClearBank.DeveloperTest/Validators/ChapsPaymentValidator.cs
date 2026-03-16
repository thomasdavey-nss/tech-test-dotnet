using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class ChapsPaymentValidator : IPaymentValidator
{
    public bool IsValid(Account account, decimal _) =>
        account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
        account.Status == AccountStatus.Live;
}