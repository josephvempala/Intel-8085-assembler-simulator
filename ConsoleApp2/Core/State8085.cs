using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("Tests")]
namespace AssemblerSimulator8085.Core
{
    public class State8085
    {
        public struct Flags
        {
            private bool cy, p, ac, z, s; //Flags
            internal byte flagreg; //flag register
            public bool CY
            {
                get
                {
                    return cy;
                }
                set
                {
                    if (value)
                    {
                        flagreg += 1;
                    }
                    else
                    {
                        flagreg -= 1;
                    }
                    cy = value;
                }
            }//Carry Flag
            public bool P
            {
                get
                {
                    return p;
                }
                set
                {
                    if (value)
                    {
                        flagreg += 4;
                    }
                    else
                    {
                        flagreg -= 4;
                    }
                    p = value;
                }
            }//Parity Flag
            public bool AC
            {
                get
                {
                    return ac;
                }
                set
                {
                    if (value)
                    {
                        flagreg += 16;
                    }
                    else
                    {
                        flagreg -= 16;
                    }
                    ac = value;
                }

            }//Alternate Carry Flag
            public bool Z
            {
                get
                {
                    return z;
                }
                set
                {
                    if (value)
                    {
                        flagreg += 64;
                    }
                    else
                    {
                        flagreg -= 64;
                    }
                    z = value;
                }
            }//Zero Flag
            public bool S
            {
                get
                {
                    return s;
                }
                set
                {
                    if (value)
                    {
                        flagreg += 128;
                    }
                    else
                    {
                        flagreg -= 128;
                    }
                    s = value;
                }
            }//Sign flag
        }
        public struct Registers
        {
            public ushort BC
            {
                get
                {
                    return BitConverter.ToUInt16(new byte[] { C, B }, 0);
                }
                set
                {
                    byte[] temp = BitConverter.GetBytes(value);
                    B = temp[0];
                    C = temp[1];
                }
            }//BC Register Pair
            public ushort DE
            {
                get
                {
                    return BitConverter.ToUInt16(new byte[] { E, D }, 0);
                }
                set
                {
                    byte[] temp = BitConverter.GetBytes(value);
                    D = temp[0];
                    E = temp[1];
                }
            }//DE Register Pair
            public ushort HL
            {
                get
                {
                    return BitConverter.ToUInt16(new byte[] { L, H }, 0);
                }
                set
                {
                    byte[] temp = BitConverter.GetBytes(value);
                    H = temp[0];
                    L = temp[1];
                }
            }//HL Register Pair
            public byte B { get; set; }
            public byte C { get; set; }
            public byte D { get; set; }
            public byte E { get; set; }
            public byte H { get; set; }
            public byte L { get; set; }
            public byte A { get; set; }
        }
        public struct InterruptMaskStatus
        {
            public bool InterruptEnable { get; set; }
            public bool RST7_5 { get; set; }
            public bool RST6_5 { get; set; }
            public bool RST5_5 { get; set; }
        }
        public Flags flags;
        public Registers registers;
        public InterruptMaskStatus interruptMaskStatus;
        public ushort SP { get; set; }//Stack Pointer
        public ushort PC { get; set; }//Program counter
        public ushort PSW
        {
            get
            {
                return BitConverter.ToUInt16(new byte[] { flags.flagreg, registers.A }, 0);
            }
            set
            {
                byte[] temp = BitConverter.GetBytes(value);
                flags.flagreg = temp[0];
                registers.A = temp[1];
            }
        }//Program Status Word
        public byte[] IO { get; private set; } //IO ports
        public byte[] Memory { get; private set; } //16 bit memory
        public byte M { get { return Memory[registers.HL]; } set { Memory[registers.HL] = value; } } //Memory pointed at by HL

        public void ResetMemory()
        {
            Memory = new byte[ushort.MaxValue + 1];
        }

        public void ResetFlags()
        {
            flags = new Flags();
        }

        public void ResetIOPorts()
        {
            IO = new byte[byte.MaxValue + 1];
        }

        public void ResetInterrupts()
        {
            interruptMaskStatus = new InterruptMaskStatus();
        }

        public void ResetRegisters()
        {
            SP = 0;
            PC = 0;
            registers.A = 0;
            registers.BC = 0;
            registers.DE = 0;
            registers.HL = 0;
        }

        public void ResetState()
        {
            ResetRegisters();
            ResetFlags();
            ResetInterrupts();
            ResetIOPorts();
            ResetMemory();
        }

        public bool TryWriteToMemory(byte[] buffer, int startIndex, int endIndex, int loadAt)
        {
            if (endIndex - startIndex < Memory.Length - loadAt)
                for (int i = startIndex; i < endIndex; i++)
                {
                    Memory[loadAt+i] = buffer[i];
                }
            else
                return false;
            return true;
        }

        public State8085()
        {
            Memory = new byte[ushort.MaxValue + 1];
            IO = new byte[byte.MaxValue + 1];
            flags = new Flags();
            registers = new Registers();
            interruptMaskStatus = new InterruptMaskStatus();
        }
    }
}
