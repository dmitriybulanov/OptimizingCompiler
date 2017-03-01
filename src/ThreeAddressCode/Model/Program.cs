using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class Program
    {
        public List<ThreeAddressCommand> Commands { get; set; } = new List<ThreeAddressCommand>();

        public Program() { }

        public Program(IEnumerable<ThreeAddressCommand> commands)
        {
            Commands.AddRange(commands);
        }

        public void Add(ThreeAddressCommand command) => Commands.Add(command);

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Commands.Select(x => x.ToString()));
        }
    }
}
