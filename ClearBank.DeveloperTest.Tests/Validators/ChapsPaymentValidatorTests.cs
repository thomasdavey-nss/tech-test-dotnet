using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentValidatorTests
{
    private readonly ChapsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountHasChapsSchemeAndIsLive_ReturnsTrue()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Live };

        var result = _sut.IsValid(account, 100m);

        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotHaveChapsScheme_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Status = AccountStatus.Live };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenAccountIsNotLive_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Disabled };

        var result = _sut.IsValid(account, 100m);

        Assert.False(result);
    }
    
    [Fact]
    public void IsValid_WhenAccountDoesNotHaveChapsSchemeAndIsNotLive_ReturnsFalse()
    {
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Status = AccountStatus.Disabled };
 
        var result = _sut.IsValid(account, 100m);
 
        Assert.False(result);
    }
}