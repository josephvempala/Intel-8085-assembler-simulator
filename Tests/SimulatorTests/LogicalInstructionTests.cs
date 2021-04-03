using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTests
{
    [TestFixture]
    class LogicalInstructionTests : InstructionTestBase
    {
        [Test]
        public void ANA()
        {
            LoadProgram("ANAa\nANAb\nANAc\nANAd\nanae\nANAh\nANAl\nhlt");
            state.registers.A = 0b11111111;
            state.registers.B = 0b00000001;
            state.registers.C = 0b00000011;
            state.registers.D = 0b00000111;
            state.registers.E = 0b00001111;
            state.registers.H = 0b00011111;
            state.registers.L = 0b00111111;
            RunProgram();
            Assert.AreEqual(0b00000001, state.registers.A);
            Assert.IsFalse(state.flags.P);
            Assert.IsFalse(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void ANI()
        {
            LoadProgram("ANI11111111b\nhlt");
            state.registers.A = 0b11111111;
            RunProgram();
            Assert.AreEqual(0xff, state.registers.A);
            Assert.IsTrue(state.flags.P);
            Assert.IsTrue(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void ORA()
        {
            LoadProgram("ORAa\nORAb\nORAc\nORAd\norae\nORAh\nORAl\nhlt");
            state.registers.A = 0b00000000;
            state.registers.B = 0b00000001;
            state.registers.C = 0b00000011;
            state.registers.D = 0b00000111;
            state.registers.E = 0b00001111;
            state.registers.H = 0b00011111;
            state.registers.L = 0b00111111;
            RunProgram();
            Assert.AreEqual(0b00111111, state.registers.A);
            Assert.IsTrue(state.flags.P);
            Assert.IsFalse(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void ORI()
        {
            LoadProgram("ORI11111111b\nhlt");
            state.registers.A = 0;
            RunProgram();
            Assert.AreEqual(0xff, state.registers.A);
            Assert.IsTrue(state.flags.P);
            Assert.IsTrue(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void XRA()
        {
            LoadProgram("XRAA\nXRAB\nXRAC\nXRAD\nXRAE\nXRAH\nXRAL\nhlt");
            state.registers.A = 0b11000000;
            state.registers.B = 0b00000001;
            state.registers.C = 0b00000011;
            state.registers.D = 0b00000111;
            state.registers.E = 0b00001111;
            state.registers.H = 0b00011111;
            state.registers.L = 0b11111111;
            RunProgram();
            Assert.AreEqual(0b11101010, state.registers.A);
            Assert.IsFalse(state.flags.P);
            Assert.IsTrue(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void XRI()
        {
            LoadProgram("XRI00001111b\nhlt");
            state.registers.A = 0b11110011;
            RunProgram();
            Assert.AreEqual(0b11111100,state.registers.A);
            Assert.IsTrue(state.flags.P);
            Assert.IsTrue(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void CMA()
        {
            LoadProgram("CMA\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.AreEqual(0b01010101,state.registers.A);
            Assert.IsFalse(state.flags.P);
            Assert.IsFalse(state.flags.S);
            Assert.IsFalse(state.flags.Z);
        }
        [Test]
        public void CMC()
        {
            LoadProgram("CMC\nhlt");
            state.flags.CY = false;
            RunProgram();
            Assert.IsTrue(state.flags.CY);
        }
        [Test]
        public void STC()
        {
            LoadProgram("STC\nhlt");
            state.flags.CY = false;
            RunProgram();
            Assert.IsTrue(state.flags.CY);
        }
        [Test]
        public void CMP()
        {
            LoadProgram("CMPA\nhlt");
            state.registers.A = 0b10000001;
            RunProgram();
            Assert.IsTrue(state.flags.P);
            Assert.IsFalse(state.flags.S);
            Assert.IsTrue(state.flags.Z);
        }
        [Test]
        public void CPI()
        {
            LoadProgram("CPI01010101b\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.IsTrue(state.flags.P);
            Assert.IsFalse(state.flags.S);
            Assert.IsFalse(state.flags.Z);   
        }
        [Test]
        public void RLC()
        {
            LoadProgram("RLC\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.IsTrue(state.flags.CY);
            Assert.AreEqual(0b01010101,state.registers.A);
        }
        [Test]
        public void RRC()
        {
            LoadProgram("RRC\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.IsFalse(state.flags.CY);
            Assert.AreEqual(0b01010101, state.registers.A);
        }
        [Test]
        public void RAL()
        {
            LoadProgram("RAL\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.IsTrue(state.flags.CY);
            Assert.AreEqual(0b01010100, state.registers.A);
        }
        [Test]
        public void RAR()
        {
            LoadProgram("RAR\nhlt");
            state.registers.A = 0b10101010;
            RunProgram();
            Assert.IsFalse(state.flags.CY);
            Assert.AreEqual(0b01010101, state.registers.A);
        }
    }
}
