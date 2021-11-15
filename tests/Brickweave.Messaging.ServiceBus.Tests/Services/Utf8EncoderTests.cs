using System.Text;
using Brickweave.Messaging.ServiceBus.Services;
using FluentAssertions;
using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests.Services
{
    public class Utf8EncoderTests
    {
        [Fact]
        public void Name_ReturnsUtf8()
        {
            var encoder = new Utf8Encoder();

            encoder.Name.Should().Be("UTF8");
        }

        [Fact]
        public void Encode_ReturnsUtfEncodedValue()
        {
            var encoder = new Utf8Encoder();

            var encoded = encoder.Encode("test");
            var decoded = Encoding.UTF8.GetString(encoded);

            decoded.Should().Be("test");
        }
    }
}
