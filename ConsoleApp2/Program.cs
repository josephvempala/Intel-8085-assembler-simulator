using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace AssemblerSimulator8085
{
    public class Simulator8085
    {
        private State state { get; set; }
        private Assembler assembler { get; set; }
        private Simulator simulator { get; set; }
        private bool continue_running { get; set; }
        private ushort loaded_at { get; set; }

        public Simulator8085()
        {
            state = new();
            assembler = new();
            simulator = new(state);
            continue_running = true;
            simulator.halt += () => { continue_running = false; };
        }

        public bool Assemble(string source, ushort load_at)
        {
            loaded_at = load_at;
            state.PC = loaded_at;
            state.Memory = assembler.Assemble(source, load_at);
            if (state.Memory == null)
                return false;
            else
                return true;
        }

        public void ResetInterpreter()
        {
            state.PC = loaded_at;
        }

        public State GetState()
        {
            return state;
        }

        public IReadOnlyCollection<AssembleError> GetAssemblyErrors()
        {
            return assembler.errors_list.AsReadOnly();
        }

        public void RunUntilLineNumber(int line_number)
        {
            for(int i = 0; i < line_number; i++)
            {
                SimulateSingleInstruction();
            }
        }

        public void Run()
        {
            while(continue_running && state.PC <= ushort.MaxValue && state.PC >= loaded_at)
            {
                simulator.Simulate();
            }
            if(!(state.PC <= ushort.MaxValue && state.PC >= loaded_at))
            {
                throw new System.Exception("No HLT instruction implemented");
            }
        }

        public void SimulateSingleInstruction()
        {
            if (continue_running && (state.PC <= ushort.MaxValue && state.PC >= loaded_at))
                simulator.Simulate();
            else
                throw new System.Exception("No HLT instruction implemented");
        }
    }
}
