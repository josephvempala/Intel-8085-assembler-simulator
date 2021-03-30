using NUnit.Framework;
using AssemblerSimulator8085.Assembler;
using System.Collections.Generic;
using System;

namespace Tests
{
    class AssemblerTests
    {
        Assembler8085 assembler = new Assembler8085();
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void BasicInstructionTests()
        {
            List<string> correctTestInstructions = new List<string>()
            {
                "Lda 50A0h",
                "inx H",
                "Lda 1010101010101010B",
                "mvI   b  ,01010101b    ",
                "Lda 256d",
                "mvI c,24d",
                "mov a,b",
            };
            List<byte[]> results = new List<byte[]>();
            foreach(var i in correctTestInstructions)
                results.Add(assembler.Assemble(i));
            foreach (var i in results)
                Assert.IsNotNull(i);
        }
        [Test]
        public void BasicProgramTest()
        {
            string TestProgram = "MVI B, 00H\n MVI C, 08H\n MOV A, D\n BACK: RAR\n JNC SKIP\n INR B\n SKIP: DCR C\n JNZ BACK\n HLT ";
            var result = assembler.Assemble(TestProgram);
            Assert.IsNotNull(result);
        }
        [Test]
        public void UnresolvedLabelErrorCheck()
        {
            string TestProgram = "MVI B, 00H\n MVI C, 08H\n MOV A, D\n RAR\n JNC SKIP\n INR B\n SKIP: DCR C\n JNZ BACK\n HLT ";
            var result = assembler.Assemble(TestProgram);
            Assert.IsNull(result);
            Assert.AreEqual(1, assembler.errors_list.Count);
            Assert.AreEqual("Label \"BACK\"Could not be found", assembler.errors_list[0].error);
        }
        [Test]
        public void InvalidInstructionErrorCheck()
        {
            string TestProgram = "tea 5325h";
            var result = assembler.Assemble(TestProgram);
            Assert.IsNull(result);
            Assert.AreEqual(1, assembler.errors_list.Count);
            Assert.AreEqual("Invalid opcode/operand at", assembler.errors_list[0].error);
            TestProgram = " ";
            result = assembler.Assemble(TestProgram);
            Assert.IsNull(result);
            Assert.AreEqual(0, assembler.errors_list.Count);
        }
    }
}
