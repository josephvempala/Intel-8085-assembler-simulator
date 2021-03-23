using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerSimulator8085
{
    public class CodeEditorAPI
    {
        State8085 st;
        Assembler8085 asm;
        Simulator8085 sim;
        int current_line = 1;
        ushort? next_line_to_execute_address = null;//address at which the next line to execute is stored in 8085 memory
        Dictionary<int, ushort> code_addresses; //stores the address and line number of instructions
        Dictionary<ushort, int> address_codes;//reverse code_addresses for lookup
        bool continue_simulation = true;
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
            st.Memory = asm.Assemble(codelines, load_at);
            current_line = 1;
            code_addresses = asm.code_addresses;
            address_codes = code_addresses.SwapKeyValues();
            next_line_to_execute_address = code_addresses[current_line];
            continue_simulation = true;
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
                if(st.PC != next_line_to_execute_address) //incase we just completed a jump or call, PC will point to new address so we check that and set the next line accordingly
                {
                    if(address_codes.TryGetValue(st.PC, out int line))
                    {
                        current_line = line;
                    }
                    else
                    {
                        while(!address_codes.ContainsKey(st.PC) && st.PC<0xffff && continue_simulation)
                        {
                            sim.Simulate();
                        }
                        current_line = address_codes.GetValueOrDefault(st.PC);
                    }
                }
            }
            else
            {
                throw new Exception("Program is writing into code buffer");
            }
            return current_line;
        }
    }
}
