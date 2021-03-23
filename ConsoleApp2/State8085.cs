using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AssemblerSimulator8085
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
        public byte[] IO { get; set; } = new byte[byte.MaxValue + 1]; //IO ports
        public byte[] Memory { get; set; } = new byte[ushort.MaxValue + 1]; //16 bit memory
        public byte M { get { return Memory[registers.HL]; } set { Memory[registers.HL] = value; } } //Memory pointed at by HL
    }
}
