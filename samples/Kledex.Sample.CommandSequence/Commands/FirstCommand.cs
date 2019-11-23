using Kledex.Commands;

namespace Kledex.Sample.CommandSequence.Commands
{
    public class FirstCommand : Command
    {
        public string Name { get; set; }

        public FirstCommand()
        {
            Validate = true;
        }
    }
}
