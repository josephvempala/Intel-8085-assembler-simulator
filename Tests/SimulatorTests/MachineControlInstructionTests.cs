using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTests
{
    [TestFixture]
    class MachineControlInstructionTests : InstructionTestBase
    {
        [Test]
        public void IN()
        {
            LoadProgram("INFFH\nhlt");
            state.TryWriteToIOPorts(new byte[] { 0xF0 }, 0, 1, 0xff);
            RunProgram();
            Assert.AreEqual(0xf0,state.registers.A);
        }
        [Test]
        public void OUT()
        {
            LoadProgram("OUTFFH\nhlt");
            state.registers.A = 0xFF;
            RunProgram();
            Assert.AreEqual(0xff, state.IO[0xff]);
        }
        [Test]
        public void PUSH()
        {
            LoadProgram("PUSHB\nPUSHD\nPUSHH\nhlt");
            state.registers.BC = 0xff00;
            state.registers.DE = 0x00ff;
            state.registers.HL = 0xdddd;
            RunProgram();
            Assert.AreEqual(0xff, state.Memory[0xffff]);
            Assert.AreEqual(0x00, state.Memory[0xfffe]);
            Assert.AreEqual(0x00, state.Memory[0xfffd]);
            Assert.AreEqual(0xff, state.Memory[0xfffc]);
            Assert.AreEqual(0xdd, state.Memory[0xfffb]);
            Assert.AreEqual(0xdd, state.Memory[0xfffa]);
        }
        [Test]
        public void POP()
        {
            LoadProgram("POPH\nPOPD\nPOPB\nhlt");
            state.SP-=6;
            state.Memory[0xffff] = 0xff;
            state.Memory[0xfffe] = 0x00;
            state.Memory[0xfffd] = 0x00;
            state.Memory[0xfffc] = 0xff;
            state.Memory[0xfffb] = 0xdd;
            state.Memory[0xfffa] = 0xdd;
            RunProgram();
            Assert.AreEqual(0xff00, state.registers.BC);
            Assert.AreEqual(0x00ff, state.registers.DE);
            Assert.AreEqual(0xdddd, state.registers.HL);
        }
        [Test]
        public void HLT()
        {
            LoadProgram("hlt");
            RunProgram();
        }
        [Test]
        public void XTHL()
        {
            LoadProgram("XTHL\nhlt");
            state.SP -= 2;
            state.Memory[0xffff] = 0xff;
            state.Memory[0xfffe] = 0x00;
            RunProgram();
            Assert.AreEqual(0xff00, state.registers.HL);
            Assert.AreEqual(0x00, state.Memory[0xfffe]);
            Assert.AreEqual(0x00, state.Memory[0xffff]);
        }
        [Test]
        public void SPHL()
        {
            LoadProgram("SPHL\nhlt");
            state.registers.HL = 0x4000;
            RunProgram();
            Assert.AreEqual(0x4000, state.SP);
        }
        [Test]
        public void EI()
        {
            LoadProgram("EI\nhlt");
            RunProgram();
            Assert.IsTrue(state.interruptStatus.InterruptEnable);
        }
        [Test]
        public void DI()
        {
            LoadProgram("DI\nhlt");
            RunProgram();
            Assert.IsFalse(state.interruptStatus.InterruptEnable);
        }
    }
}
