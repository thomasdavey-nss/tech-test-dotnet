using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class BacsPaymentValidatorTests
{
    private readonly BacsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountHasBacsScheme_ReturnsTrue()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

        var result = _sut.IsValid(account, 100m);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotHaveBacsScheme_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }
}