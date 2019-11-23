namespace Kledex.Sample.CommandSequence.Commands
{
    public class SampleCommandSequence : Kledex.Commands.CommandSequence
    {
        public SampleCommandSequence()
        {
            AddCommand(new FirstCommand { Name = "My Name" });
            AddCommand(new SecondCommand());
            AddCommand(new ThirdCommand());
        }
    }
}
