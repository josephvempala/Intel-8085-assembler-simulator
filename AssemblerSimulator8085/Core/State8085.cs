using AssemblerSimulator8085.HelperExtensions;
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
            private byte flagreg; //flag register
            public byte Flagreg
            {
                get
                {
                    return flagreg;
                }
                set
                {
                    bool[] temp = value.GetBits();
                    if(temp[0])
                    {
                        s = true;
                    }
                    else
                    {
                        s = false;
                    }
                    if (temp[1])
                    {
                        z = true;
                    }
                    else
                    {
                        z = false;
                    }
                    if (temp[3])
                    {
                        ac = true;
                    }
                    else
                    {
                        ac = false;
                    }
                    if (temp[5])
                    {
                        p = true;
                    }
                    else
                    {
                        p = false;
                    }
                    if (temp[7])
                    {
                        cy = true;
                    }
                    else
                    {
                        cy = false;
                    }
                    flagreg = value;
                }
            }
            public bool CY
            {
                get
                {
                    return cy;
                }
                set
                {
                    if (value is true & value != cy)
                    {
                        flagreg += 1;
                    }
                    else if(value is not true & value != cy)
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
                    if (value is true & value != p)
                    {
                        flagreg += 4;
                    }
                    else if (value is not true & value != p)
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
                    if (value is true & value != ac)
                    {
                        flagreg += 16;
                    }
                    else if (value is not true & value != ac)
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
                    if (value is true & value != z)
                    {
                        flagreg += 64;
                    }
                    else if (value is not true & value != z)
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
                    if (value is true & value != s)
                    {
                        flagreg += 128;
                    }
                    else if (value is not true & value != s)
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
                    B = temp[1];
                    C = temp[0];
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
                    D = temp[1];
                    E = temp[0];
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
                    H = temp[1];
                    L = temp[0];
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
        public struct InterruptStatus
        {
            public bool InterruptEnable { get; internal set; }
            public bool RST7_5Enabled { get; internal set; }
            public bool RST6_5Enabled { get; internal set; }
            public bool RST5_5Enabled { get; internal set; }
            public bool INTREnabled { get; internal set; }
            public bool RST7_5Pending { get; internal set; }
            public bool RST6_5Pending { get; internal set; }
            public bool RST5_5Pending { get; internal set; }
            public bool INTA { get; internal set; }
        }
        public bool serialIO { get; internal set; }
        public Flags flags;
        public Registers registers;
        public InterruptStatus interruptStatus;
        public ushort SP { get; set; }//Stack Pointer
        public ushort PC { get; set; }//Program counter
        public ushort PSW
        {
            get
            {
                return BitConverter.ToUInt16(new byte[] { flags.Flagreg, registers.A }, 0);
            }
            set
            {
                byte[] temp = BitConverter.GetBytes(value);
                flags.Flagreg = temp[0];
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
            interruptStatus = new InterruptStatus();
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
            if (endIndex - startIndex <= Memory.Length - loadAt)
                for (int i = startIndex; i < endIndex; i++)
                {
                    Memory[loadAt+i] = buffer[i];
                }
            else
                return false;
            return true;
        }

        public bool TryWriteToIOPorts(byte[] buffer, int startIndex, int endIndex, int loadAt)
        {
            if (endIndex - startIndex <= IO.Length - loadAt)
                for (int i = startIndex; i < endIndex; i++)
                {
                    IO[loadAt + i] = buffer[i];
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
            interruptStatus = new InterruptStatus();
        }
    }
}
