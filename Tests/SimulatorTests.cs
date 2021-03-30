using NUnit.Framework;
using AssemblerSimulator8085.Simulator;
using AssemblerSimulator8085.Core;
using AssemblerSimulator8085.Assembler;
namespace Tests
{
    static class SimulatorTests
    {
        static State8085 st = new State8085();
        static Simulator8085 s;
        static Assembler8085 a = new Assembler8085();
        static bool continue_execution;
        [SetUp]
        public static void SetUp()
        {
            s = new Simulator8085(st);
            continue_execution = true;
            s.halt += () => { continue_execution = false; };
        }
        [Test]
        public static void MVI()
        {
            string TestSource = "MVI a,01h\nMVI b,02h\nMVI c,03h\nMVI d,04h\nMVI e,05h\nMVI h,06h\nMVI l,07h\nHLT";
            var codebuffer = a.Assemble(TestSource, 0x4000);
            st.TryWriteToMemory(codebuffer, 0, codebuffer.Length, 0x4000);
            st.PC = 0x4000;
            while(continue_execution)
                s.Simulate();
            Assert.AreEqual(1, st.registers.A);
            Assert.AreEqual(2, st.registers.B);
            Assert.AreEqual(3, st.registers.C);
            Assert.AreEqual(4, st.registers.D);
            Assert.AreEqual(5, st.registers.E);
            Assert.AreEqual(6, st.registers.H);
            Assert.AreEqual(7, st.registers.L);
        }
        private static void MovHelper(int i, byte[][] codebuffers)
        {
            st.TryWriteToMemory(codebuffers[i], 0, codebuffers[i].Length, 0x4000);
            st.PC = 0x4000;
            while (continue_execution)
            {
                s.Simulate();
            }
            continue_execution = true;
            Assert.AreEqual(1, st.registers.A);
            Assert.AreEqual(1, st.registers.B);
            Assert.AreEqual(1, st.registers.C);
            Assert.AreEqual(1, st.registers.D);
            Assert.AreEqual(1, st.registers.E);
            Assert.AreEqual(1, st.registers.H);
            Assert.AreEqual(1, st.registers.L);
            Assert.AreEqual(1, st.M);
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

            st.registers.A = 1;

            for(int i=0;i<7;i++)
            {
                var codebuffer = a.Assemble(TestPrograms[i], 0);
                codebuffers[i] = codebuffer;
            }
            st.ResetState();
            st.registers.A = 1;
            MovHelper(0, codebuffers);
            st.ResetState();
            st.registers.B = 1;
            MovHelper(1, codebuffers);
            st.ResetState();
            st.registers.C = 1;
            MovHelper(2, codebuffers);
            st.ResetState();
            st.registers.D = 1;
            MovHelper(3, codebuffers);
            st.ResetState();
            st.registers.E = 1;
            MovHelper(4, codebuffers);
            st.ResetState();
            st.registers.H = 1;
            MovHelper(5, codebuffers);
            st.ResetState();
            st.registers.L = 1;
            MovHelper(6, codebuffers);
            st.ResetState();
        }

    }
}
