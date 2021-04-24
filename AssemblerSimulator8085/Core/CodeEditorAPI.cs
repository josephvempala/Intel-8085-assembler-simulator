using AssemblerSimulator8085.Assembler;
using AssemblerSimulator8085.HelperExtensions;
using AssemblerSimulator8085.Simulator;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssemblerSimulator8085.Core
{
    public class CodeEditorAPI
    {
        private Assembler8085 asm;
        private Simulator8085 sim;
        private int current_line = 1;
        private ushort? next_line_to_execute_address = null; //address at which the next line to execute is stored in 8085 memory
        private Dictionary<int, ushort> code_addresses; //stores the address and line number of instructions
        private Dictionary<ushort, int> address_codes; //reverse code_addresses for lookup
        private bool continue_simulation = true;
        private CancellationTokenSource cts = new(5000);

        public State8085 State { get; private set; }
        public List<AssembleError> errors
        {
            get
            {
                if (asm.errors_list is not null)
                    return asm.errors_list;
                else
                    return new List<AssembleError>();
            }
        }
        public CodeEditorAPI()
        {
            State = new();
            asm = new();
            sim = new(State);
            current_line = 1;
            next_line_to_execute_address = null;
            sim.halt += () => { continue_simulation = false; };
        }
        public void Assemble(string codelines, ushort load_at)
        {
            byte[] assembledProgram = asm.Assemble(codelines, load_at);
            if (assembledProgram is null)
                return;
            if (State.TryWriteToMemory(assembledProgram, 0, assembledProgram.Length, load_at))
            {
                current_line = 1;
                code_addresses = asm.code_addresses;
                address_codes = code_addresses.SwapKeyValues();
                next_line_to_execute_address = code_addresses[current_line];
                continue_simulation = true;
            }
            else
            {
                throw new Exception($"Program too large to be loaded at {load_at.ToString("X")}");
            }
        }
        public void Run()
        {
            int i = 0;
            while (continue_simulation && i < 0xffff)
            {
                sim.Simulate();
                i++;
            }
        }
        public async Task<int> SimulateNextLine()//returns current line after executing instruction
        {
            if (next_line_to_execute_address is null)
                throw new Exception("Code was not Assembled, Please Assemble code before execution");
            if (State.PC == next_line_to_execute_address && continue_simulation)
            {
                sim.Simulate();
                current_line++;
                next_line_to_execute_address = asm.code_addresses[current_line];
                if (State.PC != next_line_to_execute_address) //incase we just completed a jump or call, PC will point to new address so we check that and set the next line accordingly
                {
                    if (address_codes.TryGetValue(State.PC, out int line))
                    {
                        current_line = line;
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            while (!address_codes.ContainsKey(State.PC) && State.PC < 0xffff && continue_simulation)
                            {
                                sim.Simulate();
                            }
                        });
                        current_line = address_codes.GetValueOrDefault(State.PC);
                    }
                }
            }
            else
            {
                throw new Exception("Program is writing into code buffer area");
            }
            return current_line;
        }
    }
}
