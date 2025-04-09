using Xunit;
using TextToXmlApiNet.Services;


namespace TextToXmlApiNet.Tests
{
    public class FieldValidationServiceTests
    {
        [Fact]
        public void Should_Fail_When_Field_Too_Short()
        {
            var service = new FieldValidationService();

            var result = service.ValidateField("Name", "A");
            Assert.False(result.IsValid);
        }
    }
}
