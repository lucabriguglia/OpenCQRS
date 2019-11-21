using Kledex.Commands;

namespace Kledex.Sample.CommandSequence.Commands
{
    public class SampleSequenceCommand : SequenceCommand
    {
        public SampleSequenceCommand()
        {
            AddCommand(new FirstCommand { Name = "My Name" });
            AddCommand(new SecondCommand());
            AddCommand(new ThirdCommand());
        }
    }
}
