using System.Diagnostics;
using System.Text.Json;

namespace AssemblerSimulator8085
{
    public class Simulator8085
    {
        public Simulator8085(State state)
        {

        }
        public static void Main()
        {
            string jeeme = JsonSerializer.Serialize(new State());
        }
    }
}
