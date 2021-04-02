using NUnit.Framework;

namespace SimulatorTests
{
    [TestFixture]
    class ArithmeticInstructionTests : InstructionTestBase
    {
        [Test]
        public static void ADD()
        {
            LoadProgram("adda\naddb\naddc\naddd\nadde\naddh\naddl\nhlt");
            state.registers.A = 1;
            state.registers.B = 2;
            state.registers.C = 3;
            state.registers.D = 4;
            state.registers.E = 5;
            state.registers.H = 6;
            state.registers.L = 7;
            RunProgram();
            Assert.IsFalse(state.flags.CY);
            Assert.AreEqual(0x1d, state.registers.A);
            LoadProgram("addb\nhlt");
            state.registers.A = 0xfe;
            state.registers.B = 0x2;
            RunProgram();
            Assert.AreEqual(0,state.registers.A);
            Assert.IsTrue(state.flags.CY);
        }
        [Test]
        public static void ADC()
        {
            LoadProgram("adca\nadcb\nadcc\nadcd\nadce\nadch\nadcl\nhlt");
            state.registers.A = 1;
            state.registers.B = 2;
            state.registers.C = 3;
            state.registers.D = 4;
            state.registers.E = 5;
            state.registers.H = 6;
            state.registers.L = 7;
            RunProgram();
            Assert.IsFalse(state.flags.CY);
            Assert.AreEqual(0x1d, state.registers.A);
            LoadProgram("adcb\nhlt");
            state.registers.A = 0x5;
            state.flags.CY = true;
            state.registers.B = 0x2;
            RunProgram();
            Assert.AreEqual(0x8, state.registers.A);
            Assert.IsFalse(state.flags.CY);
        }
        [Test]
        public static void ADI()
        {
            LoadProgram("adi50h\nhlt");
            state.registers.A = 0x10;
            RunProgram();
            Assert.AreEqual(0x60, state.registers.A);
            Assert.IsFalse(state.flags.CY);
            LoadProgram("adi2h\nhlt");
            state.registers.A = 0xfe;
            RunProgram();
            Assert.AreEqual(0, state.registers.A);
            Assert.IsTrue(state.flags.CY);
        }
        [Test]
        public static void ACI()
        {
            LoadProgram("aci50h\nhlt");
            state.registers.A = 0x10;
            RunProgram();
            Assert.AreEqual(0x60, state.registers.A);
            Assert.IsFalse(state.flags.CY);
            LoadProgram("aci1h\nhlt");
            state.flags.CY = true;
            state.registers.A = 0x1;
            RunProgram();
            Assert.AreEqual(0x3, state.registers.A);
            Assert.IsFalse(state.flags.CY);
        }
        [Test]
        public static void DAD()
        {
            LoadProgram("DADB\nhlt");
            state.registers.BC = 0x00ff;
            state.registers.HL = 0xff00;
            RunProgram();
            Assert.AreEqual(0xffff, state.registers.HL);
            LoadProgram("DADD\nhlt");
            state.registers.DE = 0x00ff;
            state.registers.HL = 0xff00;
            RunProgram();
            Assert.AreEqual(0xffff, state.registers.HL);
        }
        [Test]
        public static void SUB()
        {
            LoadProgram("suba\nsubb\nsubc\nsubd\nsube\nsubh\nsubl\nhlt");
            state.registers.A = 1;
            state.registers.B = 2;
            state.registers.C = 3;
            state.registers.D = 4;
            state.registers.E = 5;
            state.registers.H = 6;
            state.registers.L = 7;
            RunProgram();
            Assert.IsTrue(state.flags.S);
            Assert.AreEqual(0xE5, state.registers.A);
            LoadProgram("subb\nhlt");
            state.registers.A = 0x0a;
            state.registers.B = 0x2;
            RunProgram();
            Assert.AreEqual(0x8, state.registers.A);
            Assert.IsFalse(state.flags.S);
        }
        [Test]
        public static void SBB()
        {
            LoadProgram("sbba\nsbbb\nsbbc\nsbbd\nsbbe\nsbbh\nsbbl\nhlt");
            state.registers.A = 1;
            state.registers.B = 2;
            state.registers.C = 3;
            state.registers.D = 4;
            state.registers.E = 5;
            state.registers.H = 6;
            state.registers.L = 7;
            RunProgram();
            Assert.IsFalse(state.flags.CY);
            Assert.AreEqual(0xe4, state.registers.A);
            LoadProgram("sbbb\nhlt");
            state.registers.A = 0x5;
            state.flags.CY = true;
            state.registers.B = 0x2;
            RunProgram();
            Assert.AreEqual(0x2, state.registers.A);
            Assert.IsFalse(state.flags.CY);
        }
        [Test]
        public static void SUI()
        {
            LoadProgram("sui13h\nhlt");
            state.registers.A = 0x44;
            RunProgram();
            Assert.AreEqual(0x31,state.registers.A);
        }
        [Test]
        public static void SBI()
        {
            LoadProgram("sbi13h\nhlt");
            state.registers.A = 0x44;
            state.flags.CY = true;
            RunProgram();
            Assert.AreEqual(0x30, state.registers.A);
        }
        [Test]
        public static void INR()
        {
            LoadProgram("inra\ninrb\ninrc\ninrd\ninre\ninrh\ninrl\nhlt");
            RunProgram();
            Assert.AreEqual(0x1, state.registers.A);
            Assert.AreEqual(0x1, state.registers.B);
            Assert.AreEqual(0x1, state.registers.C);
            Assert.AreEqual(0x1, state.registers.D);
            Assert.AreEqual(0x1, state.registers.E);
            Assert.AreEqual(0x1, state.registers.H);
            Assert.AreEqual(0x1, state.registers.L);
        }
        [Test]
        public static void DCR()
        {
            LoadProgram("dcra\ndcrb\ndcrc\ndcrd\ndcre\ndcrh\ndcrl\nhlt");
            RunProgram();
            Assert.AreEqual(0xff, state.registers.A);
            Assert.AreEqual(0xff, state.registers.B);
            Assert.AreEqual(0xff, state.registers.C);
            Assert.AreEqual(0xff, state.registers.D);
            Assert.AreEqual(0xff, state.registers.E);
            Assert.AreEqual(0xff, state.registers.H);
            Assert.AreEqual(0xff, state.registers.L);
        }
        [Test]
        public static void INX()
        {
            LoadProgram("inxb\ninxd\ninxh\nhlt");
            RunProgram();
            Assert.AreEqual(0x1, state.registers.BC);
            Assert.AreEqual(0x1, state.registers.DE);
            Assert.AreEqual(0x1, state.registers.HL);
        }
        [Test]
        public static void DCX()
        {
            LoadProgram("dcxb\ndcxd\ndcxh\nhlt");
            RunProgram();
            Assert.AreEqual(0xffff, state.registers.BC);
            Assert.AreEqual(0xffff, state.registers.DE);
            Assert.AreEqual(0xffff, state.registers.HL);
        }
        [Test]
        public static void DAA()
        {
            LoadProgram("mvia,38h\nmvib,45h\naddb\nDAA\nhlt");
            RunProgram();
            Assert.AreEqual(0x83, state.registers.A);
            Assert.IsFalse(state.flags.CY);
            Assert.IsTrue(state.flags.AC);
            LoadProgram("mvia,38h\nmvib,41h\naddb\nDAA\nhlt");
            RunProgram();
            Assert.AreEqual(0x79, state.registers.A);
            Assert.IsFalse(state.flags.CY);
            Assert.IsFalse(state.flags.AC);
            LoadProgram("mvia,83h\nmvib,54h\naddb\nDAA\nhlt");
            RunProgram();
            Assert.AreEqual(0x37, state.registers.A);
            Assert.IsTrue(state.flags.CY);
            Assert.IsFalse(state.flags.AC);
            LoadProgram("mvia,88h\nmvib,44h\naddb\nDAA\nhlt");
            RunProgram();
            Assert.AreEqual(0x32, state.registers.A);
            Assert.IsTrue(state.flags.CY);
            Assert.IsFalse(state.flags.AC);
        }
    }
}
