using NUnit.Framework;
using AssemblerSimulator8085.Core;
using System.Linq;
using System;

namespace StateTests
{
    [TestFixture]
    public class StateTests
    {
        State8085 state = new State8085();
        Random random = new Random();
        [SetUp]
        public void Setup()
        {
            for(int i =0; i<state.IO.Length;i++)
            {
                state.IO[i] = (byte)random.Next(byte.MinValue, byte.MaxValue);
            }
            for (int i = 0; i < state.Memory.Length; i++)
            {
                state.Memory[i] = (byte)random.Next(byte.MinValue, byte.MaxValue);
            }
            state.PC = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            state.PSW = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            state.SP = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            state.registers.A = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.B = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.C = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.D = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.E = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.H = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.registers.L = (byte)random.Next(byte.MinValue, byte.MaxValue);
            state.interruptStatus.InterruptEnable = true;
            state.interruptStatus.RST5_5Enabled = true;
            state.interruptStatus.RST6_5Enabled = true;
            state.interruptStatus.RST7_5Enabled = true;
            state.flags.AC = true;
            state.flags.CY = true;
            state.flags.S = true;
            state.flags.Z = true;
            state.flags.P = true;
        }
        [TearDown]
        public void TearDown()
        {
            state.ResetState();
        }
        [Test]
        public void ResetFlags()
        {
            state.ResetFlags();
            Assert.IsFalse(state.flags.AC);
            Assert.IsFalse(state.flags.CY);
            Assert.IsFalse(state.flags.S);
            Assert.IsFalse(state.flags.Z);
            Assert.IsFalse(state.flags.P);
        }
        [Test]
        public void ResetInterrupts()
        {
            state.ResetInterrupts();
            Assert.IsFalse(state.interruptStatus.InterruptEnable);
            Assert.IsFalse(state.interruptStatus.RST5_5Enabled);
            Assert.IsFalse(state.interruptStatus.RST6_5Enabled);
            Assert.IsFalse(state.interruptStatus.RST7_5Enabled);
        }
        [Test]
        public void ResetIOPorts()
        {
            state.ResetIOPorts();
            foreach(var i in state.IO)
            {
                Assert.AreEqual(0, i);
            }
        }
        [Test]
        public void ResetMemory()
        {
            state.ResetMemory();
            foreach (var i in state.Memory)
            {
                Assert.AreEqual(0, i);
            }
        }
        [Test]
        public void ResetREgisters()
        {
            state.ResetRegisters();
            Assert.AreEqual(0,state.SP);
            Assert.AreEqual(0, state.PC);
            Assert.AreEqual(0, state.registers.A);
            Assert.AreEqual(0, state.registers.B);
            Assert.AreEqual(0, state.registers.C);
            Assert.AreEqual(0, state.registers.D);
            Assert.AreEqual(0, state.registers.E);
            Assert.AreEqual(0, state.registers.H);
            Assert.AreEqual(0, state.registers.L);
        }
        [Test]
        public void WriteToMemory()
        {
            state.ResetMemory();
            byte[] bytes = new byte[400];
            for(int i=0;i<bytes.Length;i++)
            {
                bytes[i]= (byte)random.Next(byte.MinValue, byte.MaxValue);
            }
            bool temp = state.TryWriteToMemory(bytes, 0, bytes.Length, 0xffff-400);
            Assert.IsTrue(temp);
            for(int i = 0;i<bytes.Length;i++)
            {
                Assert.AreEqual(bytes[i], state.Memory[0xffff-400+i]);
            }
            temp = state.TryWriteToMemory(bytes, 0, bytes.Length, 0xffff - 1);
            Assert.IsFalse(temp);
        }
    }
}