using Kledex.Commands;

namespace Kledex.Tests.Fakes
{
    public class SampleCommandSequence : CommandSequence
    {
        public SampleCommandSequence()
        {
            AddCommand(new CommandInSequence());
        }
    }
}
