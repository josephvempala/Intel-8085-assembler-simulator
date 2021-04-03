using AssemblerSimulator8085.Assembler;
using AssemblerSimulator8085.HelperExtensions;
using AssemblerSimulator8085.Simulator;
using System;
using System.Collections.Generic;

namespace AssemblerSimulator8085.Core
{
    public class CodeEditorAPI
    {
        private State8085 st;
        private Assembler8085 asm;
        private Simulator8085 sim;
        private int current_line = 1;
        private ushort? next_line_to_execute_address = null; //address at which the next line to execute is stored in 8085 memory
        private Dictionary<int, ushort> code_addresses; //stores the address and line number of instructions
        private Dictionary<ushort, int> address_codes; //reverse code_addresses for lookup
        private bool continue_simulation = true;
        public CodeEditorAPI()
        {
            st = new();
            asm = new();
            sim = new(st);
            current_line = 1;
            next_line_to_execute_address = null;
            sim.halt += () => { continue_simulation = false; };
        }
        public void Assemble(string codelines, ushort load_at)
        {
            byte[] assembledProgram = asm.Assemble(codelines, load_at);
            if (st.TryWriteToMemory(assembledProgram, 0, assembledProgram.Length, load_at))
            {
                current_line = 1;
                code_addresses = asm.code_addresses;
                address_codes = code_addresses.SwapKeyValues();
                next_line_to_execute_address = code_addresses[current_line];
                continue_simulation = true;
            }
            else
            {
                throw new Exception($"Program too large to be loaded at {load_at}");
            }
        }
        public int SimulateNextLine()//returns current line after executing instruction
        {
            if (next_line_to_execute_address is null)
                throw new Exception("Code was not Assembled, Please Assemble code before execution");
            if (st.PC == next_line_to_execute_address && continue_simulation)
            {
                sim.Simulate();
                current_line++;
                next_line_to_execute_address = asm.code_addresses[current_line];
                if (st.PC != next_line_to_execute_address) //incase we just completed a jump or call, PC will point to new address so we check that and set the next line accordingly
                {
                    if (address_codes.TryGetValue(st.PC, out int line))
                    {
                        current_line = line;
                    }
                    else
                    {
                        while (!address_codes.ContainsKey(st.PC) && st.PC < 0xffff && continue_simulation)
                        {
                            sim.Simulate();
                        }
                        current_line = address_codes.GetValueOrDefault(st.PC);
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
