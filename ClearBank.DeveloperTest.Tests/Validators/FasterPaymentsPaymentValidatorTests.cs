using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class FasterPaymentsPaymentValidatorTests
{
    private readonly FasterPaymentsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountHasFasterPaymentsSchemeAndSufficientBalance_ReturnsTrue()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 500m };

        var result = _sut.IsValid(account, 100m);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotHaveFasterPaymentsScheme_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 500m };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenAccountBalanceIsLessThanAmount_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 50m };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenAccountBalanceEqualsAmount_ReturnsTrue()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 100m };

        var result = _sut.IsValid(account, 100m);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotHaveFasterPaymentsSchemeAndBalanceIsInsufficient_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 50m };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }
}