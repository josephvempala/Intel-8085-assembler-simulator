using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTests
{
    [TestFixture]
    class BranchingInstructionTests : InstructionTestBase
    {
        [Test]
        public void JMP()
        {
            LoadProgram("JMP5000h");
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
        }
        [Test]
        public void JZ()
        {
            LoadProgram("JZ5000h");
            state.flags.Z = true;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JZ5000h");
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JNZ()
        {
            LoadProgram("JNZ5000h");
            state.flags.Z = false;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JNZ5000h");
            state.flags.Z = true;
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JC()
        {
            LoadProgram("JC5000h");
            state.flags.CY = true;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JC5000h");
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JNC()
        {
            LoadProgram("JNC5000h");
            state.flags.CY = false;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JNC5000h");
            state.flags.CY = true;
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JP()
        {
            LoadProgram("JP5000h");
            state.flags.S = false;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JP5000h");
            state.flags.S = true;
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JM()
        {
            LoadProgram("JM5000h");
            state.flags.S = true;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JM5000h");
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JPE()
        {
            LoadProgram("JPE5000h");
            state.flags.P = true;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JPE5000h");
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void JPO()
        {
            LoadProgram("JPO5000h");
            state.flags.P = false;
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            LoadProgram("JPO5000h");
            state.flags.P = true;
            simulator.Simulate();
            Assert.AreEqual(0x4003, state.PC);
        }
        [Test]
        public void CALL()
        {
            LoadProgram("CALL5000h");
            simulator.Simulate();
            Assert.AreEqual(0x5000, state.PC);
            Assert.AreEqual(0x00, state.Memory[0xffff]);
            Assert.AreEqual(0x40, state.Memory[0xfffe]);
        }
        [Test]
        public void RET()
        {
            LoadProgram("RET");
            state.SP -= 2;
            state.Memory[0xffff] = 0x00;
            state.Memory[0xfffe] = 0x40;
            simulator.Simulate();
            Assert.AreEqual(0x4000, state.PC);
        }
        [Test]
        public void RST0_7()
        {
            LoadProgram("RST0");
            simulator.Simulate();
            Assert.AreEqual(0x0000, state.PC);
            LoadProgram("RST1");
            simulator.Simulate();
            Assert.AreEqual(0x0008, state.PC);
            LoadProgram("RST2");
            simulator.Simulate();
            Assert.AreEqual(0x0010, state.PC);
            LoadProgram("RST3");
            simulator.Simulate();
            Assert.AreEqual(0x0018, state.PC);
            LoadProgram("RST4");
            simulator.Simulate();
            Assert.AreEqual(0x0020, state.PC);
            LoadProgram("RST5");
            simulator.Simulate();
            Assert.AreEqual(0x0028, state.PC);
            LoadProgram("RST6");
            simulator.Simulate();
            Assert.AreEqual(0x0030, state.PC);
            LoadProgram("RST7");
            simulator.Simulate();
            Assert.AreEqual(0x0038, state.PC);
        }
    }
}
