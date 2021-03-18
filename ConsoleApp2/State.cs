using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AssemblerSimulator8085
{
    public class State
    {
        private bool cy, p, ac, z, s; //Flags
        private byte flagreg; // flag register
        private byte[] memory = new byte[ushort.MaxValue + 1]; //16 bit memory

        public bool CY 
        {
            get
            {
                return cy;
            }
            set 
            {
                if(value)
                {
                    flagreg += 1;
                }
                else
                {
                    flagreg -= 1;
                }
                cy = value;
            }
        }
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
        }
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

        }
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
        }

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
        }

        public ushort PSW
        {
            get
            {
                return BitConverter.ToUInt16(new byte[] { flagreg, A }, 0);
            }
            set
            {
                byte[] temp = BitConverter.GetBytes(value);
                flagreg = temp[0];
                A = temp[1];
            }
        }

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
        }

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
        }

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
        }

        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }
        public byte E { get; set; }
        public byte H { get; set; }
        public byte L { get; set; }
        public byte M { get { return memory[HL]; } set { memory[HL] = value; } }
        public byte A { get; set; }
        public ushort SP { get; set; }
        public ushort PC { get; set; }
        public byte[] Memory { get { return memory; } set { memory = value; } }
        public List<AssembleError> Errors { get; internal set; }
    }
}
