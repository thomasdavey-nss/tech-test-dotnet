### Initial Thoughts

`PaymentService` is doing too much and violates SRP. The data store selection logic is duplicated, both the instantiation and the config read happen twice, and the validation logic is repetitive and tightly coupled to the service. Extracting these concerns into dedicated classes will improve readability, reduce duplication, and make each piece independently testable.

The following changes were identified:

- [x] Extract `Backup` magic string to a `const`
- [x] Extract and deduplicate data store selection logic
- [x] Introduce a shared `IDataStore` interface
- [x] Extract payment scheme validators into dedicated classes
- [x] Throw exception for unknown payment schemes (see Bug Fix section)
- [x] Simplify `MakePaymentResult` with static factory methods

---

### Changes / Approach

I followed a strangler fig approach, incrementally extracting logic from `PaymentService` into dedicated abstractions rather than rewriting everything at once. I considered writing E2E tests upfront as a safety net, but given the scale of the refactor and the time constraint they would have required constant updating as dependencies were extracted, so I prioritised getting clean architecture in place first, then writing focused unit tests against the final interfaces. Changes were made top to bottom through `PaymentService` and committed incrementally.

#### SOLID Principles

- **SRP**: Extracted validation logic into separate validator classes (`BacsPaymentValidator`, `FasterPaymentsPaymentValidator`, `ChapsPaymentValidator`), each responsible for a single payment scheme. `PaymentService` is now purely an orchestrator.
- **OCP**: Adding a new payment scheme requires only a new validator class and a single line in `PaymentValidatorFactory` no existing classes need to change.
- **DIP**: `PaymentService` now depends on `IDataStore` and `IPaymentValidatorFactory` abstractions via constructor injection, rather than concrete implementations.

#### Testability

- Constructor injection on `PaymentService` allows `IDataStore` and `IPaymentValidatorFactory` to be mocked in tests.
- Validator logic is independently testable without going through `PaymentService`.
- `DataStoreFactory` is not unit tested as it reads directly from `ConfigurationManager`. Given the time constraint this was left as-is, but in a production codebase this would be addressed by injecting an `IConfiguration` abstraction.

#### Bug Fix: Unknown Payment Schemes

The original code had no default case in the payment scheme switch, meaning unknown schemes would (presumably) incorrectly proceed with a successful payment. `PaymentValidatorFactory` now throws an `ArgumentOutOfRangeException` for unknown schemes, as this represents a programming error rather than a runtime condition.

#### MakePaymentResult

Added static factory methods `Succeeded()` and `Failed()` to reduce repetition and improve readability at call sites.

---

### Final Thoughts

Given more time I would have liked to:

- Inject `IConfiguration` into `DataStoreFactory` to make it fully testable
- Explore whether `PaymentValidatorFactory` could be replaced with a DI-registered `IDictionary<PaymentScheme, IPaymentValidator>` to remove the factory class entirely
- Add more edge case tests
- Add a `Program.cs` to demonstrate how the dependencies would be wired up in a real application

---

### Test Description
In the 'PaymentService.cs' file you will find a method for making a payment. At a high level the steps for making a payment are:

 - Lookup the account the payment is being made from
 - Check the account is in a valid state to make the payment
 - Deduct the payment amount from the account's balance and update the account in the database
 
What we’d like you to do is refactor the code with the following things in mind:  
 - Adherence to SOLID principals
 - Testability  
 - Readability 

We’d also like you to add some unit tests to the ClearBank.DeveloperTest.Tests project to show how you would test the code that you’ve produced. The only specific ‘rules’ are:  

 - The solution should build.
 - The tests should all pass.
 - You should not change the method signature of the MakePayment method.

You are free to use any frameworks/NuGet packages that you see fit.  
 
You should plan to spend around 1 to 3 hours to complete the exercise.
