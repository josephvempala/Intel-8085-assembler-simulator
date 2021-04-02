using AssemblerSimulator8085.Assembler;
using AssemblerSimulator8085.Core;
using AssemblerSimulator8085.Simulator;
using NUnit.Framework;

namespace SimulatorTests
{
    [TestFixture]
    class DataTransferInstructionTests : InstructionTestBase
    {
        private static void MovHelper(int i, byte[][] codebuffers)
        {
            state.TryWriteToMemory(codebuffers[i], 0, codebuffers[i].Length, 0x4000);
            state.PC = 0x4000;
            RunProgram();
            Assert.AreEqual(1, state.registers.A);
            Assert.AreEqual(1, state.registers.B);
            Assert.AreEqual(1, state.registers.C);
            Assert.AreEqual(1, state.registers.D);
            Assert.AreEqual(1, state.registers.E);
            Assert.AreEqual(1, state.registers.H);
            Assert.AreEqual(1, state.registers.L);
            Assert.AreEqual(1, state.M);
        }
        [Test]
        public static void MVI()
        {
            LoadProgram("MVI a,01h\nMVI b,02h\nMVI c,03h\nMVI d,04h\nMVI e,05h\nMVI h,06h\nMVI l,07h\nHLT");
            RunProgram();
            Assert.AreEqual(1, state.registers.A);
            Assert.AreEqual(2, state.registers.B);
            Assert.AreEqual(3, state.registers.C);
            Assert.AreEqual(4, state.registers.D);
            Assert.AreEqual(5, state.registers.E);
            Assert.AreEqual(6, state.registers.H);
            Assert.AreEqual(7, state.registers.L);
        }
        [Test]
        public static void MOV()
        {
            string[] TestPrograms = new string[] { "mova,a\nmovb,a\nmovc,a\nmovd,a\nmove,a\nmovh,a\nmovl,a\nmovm,a\nHLT",
                                                "mova,b\nmovb,b\nmovc,b\nmovd,b\nmove,b\nmovh,b\nmovl,b\nmovm,b\nHLT",
                                                "mova,c\nmovb,c\nmovc,c\nmovd,c\nmove,c\nmovh,c\nmovl,c\nmovm,c\nHLT",
                                                "mova,d\nmovb,d\nmovc,d\nmovd,d\nmove,d\nmovh,d\nmovl,d\nmovm,d\nHLT",
                                                "mova,e\nmovb,e\nmovc,e\nmovd,e\nmove,e\nmovh,e\nmovl,e\nmovm,e\nHLT",
                                                "mova,h\nmovb,h\nmovc,h\nmovd,h\nmove,h\nmovh,h\nmovl,h\nmovm,h\nHLT",
                                                "mova,l\nmovb,l\nmovc,l\nmovd,l\nmove,l\nmovh,l\nmovl,l\nmovm,l\nHLT"};
            byte[][] codebuffers = new byte[8][];

            state.registers.A = 1;

            for (int i = 0; i < 7; i++)
            {
                var codebuffer = assembler.Assemble(TestPrograms[i], 0);
                codebuffers[i] = codebuffer;
            }
            state.ResetState();
            state.registers.A = 1;
            MovHelper(0, codebuffers);
            state.ResetState();
            state.registers.B = 1;
            MovHelper(1, codebuffers);
            state.ResetState();
            state.registers.C = 1;
            MovHelper(2, codebuffers);
            state.ResetState();
            state.registers.D = 1;
            MovHelper(3, codebuffers);
            state.ResetState();
            state.registers.E = 1;
            MovHelper(4, codebuffers);
            state.ResetState();
            state.registers.H = 1;
            MovHelper(5, codebuffers);
            state.ResetState();
            state.registers.L = 1;
            MovHelper(6, codebuffers);
            state.ResetState();
        }
        [Test]
        public static void STA()
        {
            LoadProgram("STA5000h\nhlt");
            state.registers.A = 0x50;
            RunProgram();
            Assert.AreEqual(0x50, state.Memory[0x5000]);
        }
        [Test]
        public static void LDA()
        {
            LoadProgram("LDA5000h\nhlt");
            state.Memory[0x5000] = 0x45;
            state.registers.A = 0x50;
            RunProgram();
            Assert.AreEqual(0x45, state.registers.A);
        }
        [Test]
        public static void LHLD()
        {
            LoadProgram("LHLD5000h\nhlt");
            state.Memory[0x5000] = 0xFF;
            state.Memory[0x5001] = 0xFF;
            RunProgram();
            Assert.AreEqual(0xffff, state.registers.HL);
        }
        [Test]
        public static void SHLD()
        {
            LoadProgram("SHLD5000h\nhlt");
            state.registers.HL = 0xffff;
            RunProgram();
            Assert.AreEqual(0xff, state.Memory[0x5000]);
            Assert.AreEqual(0xff, state.Memory[0x5001]);
        }
        [Test]
        public static void LDAX()
        {
            LoadProgram("LDAXB\nhlt");
            state.registers.BC = 0x5000;
            state.Memory[0x5000] = 0xff;
            RunProgram();
            Assert.AreEqual(0xff, state.registers.A);
            LoadProgram("LDAXD\nhlt");
            state.registers.DE = 0x5000;
            state.Memory[0x5000] = 0xff;
            RunProgram();
            Assert.AreEqual(0xff, state.registers.A);
        }
        [Test]
        public static void XCHG()
        {
            LoadProgram("xchg\nhlt");
            state.registers.DE = 0xffff;
            RunProgram();
            Assert.AreEqual(0xffff, state.registers.HL);
            Assert.AreEqual(0x0000, state.registers.DE);
        }
    }
}
