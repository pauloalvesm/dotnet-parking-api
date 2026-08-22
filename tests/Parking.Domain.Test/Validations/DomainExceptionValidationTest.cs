using FluentAssertions;
using Parking.Domain.Validations;

namespace Parking.Domain.Test.Validations;

public class DomainExceptionValidationTest
{
    [Fact(DisplayName = "GetErrors - No Errors No Exception Thrown")]
    public void DomainExceptionValidation_GetErrors_NoErrorsNoExceptionThrown()
    {

        Action action = () => DomainExceptionValidation.GetErrors(false, "An error occurred.");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = "GetErrors - Errors Domain Exception Thrown")]
    public void DomainExceptionValidation_GetErrors_ErrorsDomainExceptionThrown()
    {

        Action action = () => DomainExceptionValidation.GetErrors(true, "An error occurred.");

        action.Should().Throw<DomainExceptionValidation>().WithMessage("An error occurred.");
    }
}
