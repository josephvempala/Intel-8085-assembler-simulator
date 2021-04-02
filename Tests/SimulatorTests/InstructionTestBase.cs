using AssemblerSimulator8085.Assembler;
using AssemblerSimulator8085.Core;
using AssemblerSimulator8085.Simulator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTests
{
    class InstructionTestBase
    {
        protected static State8085 state = new State8085();
        protected static Simulator8085 simulator;
        protected static Assembler8085 assembler = new Assembler8085();
        protected static bool continue_execution;
        [SetUp]
        protected static void SetUp()
        {
            simulator = new Simulator8085(state);
            continue_execution = true;
            simulator.halt += () => { continue_execution = false; };
        }
        [TearDown]
        protected static void TearDown()
        {
            state.ResetState();
        }
        protected static void LoadProgram(string code)
        {
            state.ResetState();
            var codeBuffer = assembler.Assemble(code, 0);
            state.TryWriteToMemory(codeBuffer, 0, codeBuffer.Length, 0x4000);
            state.PC = 0x4000;
        }
        protected static void RunProgram()
        {
            while (continue_execution)
            {
                simulator.Simulate();
            }
            continue_execution = true;
        }
    }
}
