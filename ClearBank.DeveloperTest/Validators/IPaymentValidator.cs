using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentValidator
{
    public bool IsValid(Account account, decimal amount);
}