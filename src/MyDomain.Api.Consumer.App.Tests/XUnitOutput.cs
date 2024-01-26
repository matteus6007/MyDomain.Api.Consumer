using PactNet.Infrastructure.Outputters;

using Xunit.Abstractions;

namespace MyDomain.Api.Consumer.App.Tests
{
    public class XUnitOutput(ITestOutputHelper output) : IOutput
    {
        private readonly ITestOutputHelper _output = output;

        public void WriteLine(string line)
        {
            _output.WriteLine(line);
        }
    }
}