using Kledex.Commands;

namespace Kledex.Tests.Fakes
{
    public class sampleCommandSequence : CommandSequence
    {
        public sampleCommandSequence()
        {
            AddCommand(new CreateSomething());
        }
    }
}
