using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private readonly IDataStore _dataStore;
    private readonly IPaymentValidatorFactory _validatorFactory;
    private readonly IPaymentValidator _validator;
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        _dataStore = Substitute.For<IDataStore>();
        _validatorFactory = Substitute.For<IPaymentValidatorFactory>();
        _validator = Substitute.For<IPaymentValidator>();
        _sut = new PaymentService(_dataStore, _validatorFactory);
    }

    // Lookup account failure
    
    [Fact]
    public void MakePayment_WhenAccountDoesNotExist_ReturnsFailedResult()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments);
        _dataStore.GetAccount(request.DebtorAccountNumber).Returns((Account)null);

        var result = _sut.MakePayment(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void MakePayment_WhenAccountDoesNotExist_DoesNotAttemptValidation()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments);
        _dataStore.GetAccount(request.DebtorAccountNumber).Returns((Account)null);

        _sut.MakePayment(request);

        _validatorFactory.DidNotReceive().GetValidator(Arg.Any<PaymentScheme>());
    }

    [Fact]
    public void MakePayment_WhenAccountDoesNotExist_DoesNotUpdateAccount()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments);
        _dataStore.GetAccount(request.DebtorAccountNumber).Returns((Account)null);

        _sut.MakePayment(request);

        _dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
    }
    
    // Validate account state failure

    [Fact]
    public void MakePayment_WhenValidationFails_ReturnsFailedResult()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(false);

        var result = _sut.MakePayment(request);

        Assert.False(result.Success);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotDebitAccount()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments, amount: 100m);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(false);

        _sut.MakePayment(request);

        Assert.Equal(500m, account.Balance);
    }
    
    [Fact]
    public void MakePayment_WhenValidationFails_DoesNotUpdateAccount()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(false);

        _sut.MakePayment(request);

        _dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
    }
    
    // Happy path

    [Fact]
    public void MakePayment_WhenValidAndSufficientFunds_ReturnsSuccessResult()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments, amount: 100m);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(true);

        var result = _sut.MakePayment(request);

        Assert.True(result.Success);
    }

    [Fact]
    public void MakePayment_WhenPaymentSucceeds_DebitsAccount()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments, amount: 150m);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(true);

        _sut.MakePayment(request);

        Assert.Equal(350m, account.Balance);
    }

    [Fact]
    public void MakePayment_WhenPaymentSucceeds_UpdatesAccount()
    {
        var request = BuildRequest(PaymentScheme.FasterPayments, amount: 100m);
        var account = BuildAccount(balance: 500m);

        _dataStore.GetAccount(request.DebtorAccountNumber).Returns(account);
        _validatorFactory.GetValidator(request.PaymentScheme).Returns(_validator);
        _validator.IsValid(account, request.Amount).Returns(true);

        _sut.MakePayment(request);

        _dataStore.Received(1).UpdateAccount(account);
    }

    // Helpers

    private static MakePaymentRequest BuildRequest(
        PaymentScheme scheme,
        decimal amount = 100m,
        string debtorAccountNumber = "12345678")
    {
        return new MakePaymentRequest
        {
            PaymentScheme = scheme,
            Amount = amount,
            DebtorAccountNumber = debtorAccountNumber
        };
    }

    private static Account BuildAccount(decimal balance)
    {
        return new Account
        {
            Balance = balance
        };
    }
}