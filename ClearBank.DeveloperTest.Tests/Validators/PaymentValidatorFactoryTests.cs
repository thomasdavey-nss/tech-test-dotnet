using System;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class PaymentValidatorFactoryTests
{
    private readonly PaymentValidatorFactory _sut = new();

    [Fact]
    public void GetValidator_ForBacs_ReturnsBacsValidator()
    {
        var validator = _sut.GetValidator(PaymentScheme.Bacs);

        Assert.IsType<BacsPaymentValidator>(validator);
    }

    [Fact]
    public void GetValidator_ForFasterPayments_ReturnsFasterPaymentsValidator()
    {
        var validator = _sut.GetValidator(PaymentScheme.FasterPayments);

        Assert.IsType<FasterPaymentsPaymentValidator>(validator);
    }

    [Fact]
    public void GetValidator_ForChaps_ReturnsChapsValidator()
    {
        var validator = _sut.GetValidator(PaymentScheme.Chaps);

        Assert.IsType<ChapsPaymentValidator>(validator);
    }

    [Fact]
    public void GetValidator_ForUnrecognisedScheme_ThrowsArgumentOutOfRangeException()
    {
        var unrecognised = (PaymentScheme)999;

        Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetValidator(unrecognised));
    }

    [Fact]
    public void GetValidator_ForUnrecognisedScheme_ExceptionMessageContainsSchemeValue()
    {
        const PaymentScheme unrecognised = (PaymentScheme)999;

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetValidator(unrecognised));

        Assert.Contains("999", ex.Message);
    }
}