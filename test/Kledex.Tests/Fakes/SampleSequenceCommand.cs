using Kledex.Commands;

namespace Kledex.Tests.Fakes
{
    public class SampleSequenceCommand : SequenceCommand
    {
        public SampleSequenceCommand()
        {
            AddCommand(new CreateSomething());
        }
    }
}
