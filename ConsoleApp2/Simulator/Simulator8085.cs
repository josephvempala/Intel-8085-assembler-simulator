using System;
using AssemblerSimulator8085.Core;
using AssemblerSimulator8085.HelperExtensions;

namespace AssemblerSimulator8085.Simulator
{
    internal class Simulator8085
    {
        private State8085 _state;
        public Action halt; //to stop when hlt inst encountered

        public Simulator8085(State8085 state)
        {
            _state = state;
        }

        private ushort GetNextTwoBytes()
        {
            if (_state.PC <= ushort.MaxValue - 2)
            {
                return BitConverter.ToUInt16(_state.Memory, _state.PC + 1);
            }
            else if (_state.PC == ushort.MaxValue - 1)
            {
                return BitConverter.ToUInt16(new byte[] { _state.Memory[_state.PC + 1], _state.Memory[0] }, 0);
            }
            else
            {
                return BitConverter.ToUInt16(_state.Memory, 0);
            }
        }

        public void Simulate()
        {
            switch (_state.Memory[_state.PC])
            {
                case 0x0://nop
                    break;
                case 0x1://lxib
                    _state.registers.BC = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x2: //staxb
                    _state.Memory[_state.registers.BC] = _state.registers.A;
                    break;
                case 0x3://inxb
                    _state.registers.BC++;
                    break;
                case 0x4://inrb
                    _state.registers.B = AddByte(_state.registers.B, 1, true);
                    break;
                case 0x5://dcrb
                    _state.registers.B = SubtractByte(_state.registers.B, 1, true);
                    break;
                case 0x6://mvib
                    _state.PC++;
                    _state.registers.B = _state.Memory[_state.PC];
                    break;
                case 0x7://rlc
                    if (_state.registers.A > 0x80)
                    {
                        _state.registers.A = (byte)(_state.registers.A << 1);
                        _state.flags.CY = true;
                        _state.registers.A++;
                    }
                    else
                    {
                        _state.registers.A = (byte)(_state.registers.A << 1);
                        _state.flags.CY = false;
                    }
                    break;
                case 0x8://no inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0x9://dadb
                    _state.flags.CY = (_state.registers.HL + _state.registers.BC > 0xffff);
                    _state.registers.HL = (ushort)(_state.registers.HL + _state.registers.BC);
                    break;
                case 0xa://ldaxb
                    _state.registers.A = _state.Memory[_state.registers.BC];
                    break;
                case 0xb://dcxb
                    _state.registers.BC--;
                    break;
                case 0xc://inrc
                    _state.registers.C = AddByte(_state.registers.C, 1, true);
                    break;
                case 0xd://dcrc
                    _state.registers.C = SubtractByte(_state.registers.C, 1, true);
                    break;
                case 0xe://mvic
                    _state.PC++;
                    _state.registers.C = _state.Memory[_state.PC];
                    break;
                case 0xf://rrc
                    if (_state.registers.A % 2 == 0)
                    {
                        _state.registers.A = (byte)(_state.registers.A >> 1);
                        _state.flags.CY = false;
                    }
                    else
                    {
                        _state.registers.A = (byte)((_state.registers.A >> 1));
                        _state.flags.CY = true;
                        _state.registers.A += 0x80;
                    }
                    break;
                case 0x10://no inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0x11://lxid
                    _state.registers.DE = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x12://staxd
                    _state.Memory[_state.registers.DE] = _state.registers.A;
                    break;
                case 0x13://inxd
                    _state.registers.DE++;
                    break;
                case 0x14://inrd
                    _state.registers.D = AddByte(_state.registers.D, 1, true);
                    break;
                case 0x15://dcrd
                    _state.registers.D = SubtractByte(_state.registers.D, 1, true);
                    break;
                case 0x16://mvid
                    _state.PC++;
                    _state.registers.D = _state.Memory[_state.PC];
                    break;
                case 0x17://ral
                    if (_state.flags.CY)
                    {
                        if ((_state.registers.A & 128) == 0)//checking if msb is 0
                        {
                            _state.flags.CY = false;
                        }
                        _state.registers.A = (byte)(_state.registers.A << 1);
                        _state.registers.A += 1;
                    }
                    else
                    {
                        if ((_state.registers.A & 128) == 128)//checking if msb is 0
                        {
                            _state.flags.CY = true;
                        }
                        _state.registers.A = (byte)(_state.registers.A << 1);
                    }
                    break;
                case 0x18://no inst
                    break;
                case 0x19://dadd
                    _state.flags.CY = (_state.registers.HL + _state.registers.DE > 0xffff);
                    _state.registers.HL = (ushort)(_state.registers.HL + _state.registers.DE);
                    break;
                case 0x1a://ldaxd
                    _state.registers.A = _state.Memory[_state.registers.DE];
                    break;
                case 0x1b://dcxd
                    _state.registers.DE--;
                    break;
                case 0x1c://inre
                    _state.registers.E = AddByte(_state.registers.E, 1, true);
                    break;
                case 0x1d://dcre
                    _state.registers.E = SubtractByte(_state.registers.E, 1, true);
                    break;
                case 0x1e://mvie
                    _state.PC++;
                    _state.registers.E = _state.Memory[_state.PC];
                    break;
                case 0x1f://rar
                    if(_state.flags.CY)
                    {
                        if ((_state.registers.A & 1) == 0)
                        {
                            _state.flags.CY = false;
                        }
                        _state.registers.A = (byte)(_state.registers.A >> 1);
                        _state.registers.A += 128;
                    }
                    else
                    {
                        if ((_state.registers.A & 1) == 1)
                        {
                            _state.flags.CY = true;
                        }
                        _state.registers.A = (byte)(_state.registers.A >> 1);
                    }
                    break;
                case 0x20://rim
                    {
                        int temp = 0;
                        if (_state.serialIO == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST7_5Pending == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST6_5Pending == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST5_5Pending == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.InterruptEnable == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST7_5Enabled == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST6_5Enabled == true)
                            temp += 1;
                        temp = temp << 1;
                        if (_state.interruptStatus.RST5_5Enabled == true)
                            temp += 1;
                        _state.registers.A = (byte)temp;
                    }
                    break;
                case 0x21://lxih
                    _state.registers.HL = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x22://shld
                    {
                        ushort address = GetNextTwoBytes();
                        _state.Memory[address] = _state.registers.L;
                        _state.Memory[address + 1] = _state.registers.H;
                    }
                    break;
                case 0x23://inxh
                    _state.registers.HL++;
                    break;
                case 0x24://inrh
                    _state.registers.H = AddByte(_state.registers.H, 1, true);
                    break;
                case 0x25://dcrh
                    _state.registers.H = SubtractByte(_state.registers.H, 1, true);
                    break;
                case 0x26://mvih
                    _state.PC++;
                    _state.registers.H = _state.Memory[_state.PC];
                    break;
                case 0x27://daa
                    {
                        if ((_state.registers.A & 0xf) > 9 || _state.flags.AC)
                        {
                            _state.registers.A = AddByteWithCarry(_state.registers.A, 0x6, false);
                            _state.flags.AC = true;
                        }
                        if ((_state.registers.A & 0xf0) >> 4 > 9 || _state.flags.CY)
                        {
                            _state.registers.A = AddByteWithCarry(_state.registers.A, 0x60, false);
                        }
                    }
                    break;
                case 0x28://no-inst
                    break;
                case 0x29://dadh
                    _state.flags.CY = (_state.registers.HL + _state.registers.HL > 0xffff);
                    _state.registers.HL = (ushort)(_state.registers.HL + _state.registers.HL);
                    break;
                case 0x2a://lhld
                    _state.registers.HL = BitConverter.ToUInt16(_state.Memory, GetNextTwoBytes());
                    break;
                case 0x2b://dcxh
                    _state.registers.HL--;
                    break;
                case 0x2c://inrl
                    _state.registers.L = AddByte(_state.registers.L, 1, true);
                    break;
                case 0x2d://dcrl
                    _state.registers.L = SubtractByte(_state.registers.L, 1, true);
                    break;
                case 0x2e://mvil
                    _state.PC++;
                    _state.registers.L = _state.Memory[_state.PC];
                    break;
                case 0x2f://cma
                    _state.registers.A = (byte)~_state.registers.A;
                    break;
                case 0x30://sim
                    {
                        bool[] temp = _state.registers.A.GetBits();
                        if (temp[6])//Serial Output Enable
                        {
                            _state.serialIO = temp[7];//Serial Output
                        }
                        if (temp[3])//Mask Set Enable
                        {
                            if (temp[2])//M7.5
                                _state.interruptStatus.RST5_5Enabled = false;
                            else
                                _state.interruptStatus.RST5_5Enabled = true;
                            if (temp[1])
                                _state.interruptStatus.RST6_5Enabled = false;
                            else
                                _state.interruptStatus.RST6_5Enabled = true;
                            if (temp[0])
                                _state.interruptStatus.RST7_5Enabled = false;
                            else
                                _state.interruptStatus.RST7_5Enabled = true;
                        }
                        if (temp[4])
                        {
                            _state.interruptStatus.RST7_5Enabled = false;
                        }
                    }
                    break;
                case 0x31://lxisp
                    _state.SP = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x32://sta
                    _state.Memory[GetNextTwoBytes()] = _state.registers.A;
                    _state.PC += 2;
                    break;
                case 0x33://inxsp
                    _state.SP++;
                    break;
                case 0x34://inrm
                    _state.M = AddByte(_state.M, 1, true);
                    break;
                case 0x35://dcrm
                    _state.M = SubtractByte(_state.M, 1, true);
                    break;
                case 0x36://mvim
                    _state.PC++;
                    _state.M = _state.Memory[_state.PC];
                    break;
                case 0x37://stc
                    _state.flags.CY = true;
                    break;
                case 0x38://no_inst
                    break;
                case 0x39://dadsp
                    _state.flags.CY = (_state.registers.HL + _state.SP > 0xffff);
                    _state.registers.HL = (ushort)(_state.registers.HL + _state.SP);
                    break;
                case 0x3a://lda
                    _state.registers.A = _state.Memory[GetNextTwoBytes()];
                    _state.PC += 2;
                    break;
                case 0x3b://dcxsp
                    _state.SP--;
                    break;
                case 0x3c://inra
                    _state.registers.A = AddByte(_state.registers.A, 1, true);
                    break;
                case 0x3d://dcra
                    _state.registers.A = SubtractByte(_state.registers.A, 1, true);
                    break;
                case 0x3e://mvia
                    _state.PC++;
                    _state.registers.A = _state.Memory[_state.PC];
                    break;
                case 0x3f://cmc
                    _state.flags.CY = !_state.flags.CY;
                    break;
                case 0x40://movb,b        
                    _state.registers.B = _state.registers.B;
                    break;
                case 0x41://movb,c        
                    _state.registers.B = _state.registers.C;
                    break;
                case 0x42://movb,d
                    _state.registers.B = _state.registers.D;
                    break;
                case 0x43://movb,e
                    _state.registers.B = _state.registers.E;
                    break;
                case 0x44://movb,h
                    _state.registers.B = _state.registers.H;
                    break;
                case 0x45://movb,l
                    _state.registers.B = _state.registers.L;
                    break;
                case 0x46://movb,m
                    _state.registers.B = _state.M;
                    break;
                case 0x47://movb,a
                    _state.registers.B = _state.registers.A;
                    break;
                case 0x48://movc,b
                    _state.registers.C = _state.registers.B;
                    break;
                case 0x49://movc,c
                    _state.registers.C = _state.registers.C;
                    break;
                case 0x4a://movc,d
                    _state.registers.C = _state.registers.D;
                    break;
                case 0x4b://movc,e
                    _state.registers.C = _state.registers.E;
                    break;
                case 0x4c://movc,h
                    _state.registers.C = _state.registers.H;
                    break;
                case 0x4d://movc,l
                    _state.registers.C = _state.registers.L;
                    break;
                case 0x4e://movc,m
                    _state.registers.C = _state.M;
                    break;
                case 0x4f://movc,a
                    _state.registers.C = _state.registers.A;
                    break;
                case 0x50://movd,b
                    _state.registers.D = _state.registers.B;
                    break;
                case 0x51://movd,c
                    _state.registers.D = _state.registers.C;
                    break;
                case 0x52://movd,d
                    _state.registers.D = _state.registers.D;
                    break;
                case 0x53://movd,e
                    _state.registers.D = _state.registers.E;
                    break;
                case 0x54://movd,h
                    _state.registers.D = _state.registers.H;
                    break;
                case 0x55://movd,l
                    _state.registers.D = _state.registers.L;
                    break;
                case 0x56://movd,m
                    _state.registers.D = _state.M;
                    break;
                case 0x57://movd,a
                    _state.registers.D = _state.registers.A;
                    break;
                case 0x58://move,b
                    _state.registers.E = _state.registers.B;
                    break;
                case 0x59://move,c
                    _state.registers.E = _state.registers.C;
                    break;
                case 0x5a://move,d
                    _state.registers.E = _state.registers.D;
                    break;
                case 0x5b://move,e
                    _state.registers.E = _state.registers.E;
                    break;
                case 0x5c://move,h
                    _state.registers.E = _state.registers.H;
                    break;
                case 0x5d://move,l
                    _state.registers.E = _state.registers.L;
                    break;
                case 0x5e://move,m
                    _state.registers.E = _state.M;
                    break;
                case 0x5f://move,a
                    _state.registers.E = _state.registers.A;
                    break;
                case 0x60://movh,b
                    _state.registers.H = _state.registers.B;
                    break;
                case 0x61://movh,c
                    _state.registers.H = _state.registers.C;
                    break;
                case 0x62://movh,d
                    _state.registers.H = _state.registers.D;
                    break;
                case 0x63://movh,e
                    _state.registers.H = _state.registers.E;
                    break;
                case 0x64://movh,h
                    _state.registers.H = _state.registers.H;
                    break;
                case 0x65://movh,l
                    _state.registers.H = _state.registers.L;
                    break;
                case 0x66://movh,m
                    _state.registers.H = _state.M;
                    break;
                case 0x67://movh,a
                    _state.registers.H = _state.registers.A;
                    break;
                case 0x68://movl,b
                    _state.registers.L = _state.registers.B;
                    break;
                case 0x69://movl,c
                    _state.registers.L = _state.registers.C;
                    break;
                case 0x6a://movl,d
                    _state.registers.L = _state.registers.D;
                    break;
                case 0x6b://movl,e
                    _state.registers.L = _state.registers.E;
                    break;
                case 0x6c://movl,h
                    _state.registers.L = _state.registers.H;
                    break;
                case 0x6d://movl,l
                    _state.registers.L = _state.registers.L;
                    break;
                case 0x6e://movl,m
                    _state.registers.L = _state.M;
                    break;
                case 0x6f://movl,a
                    _state.registers.L = _state.registers.A;
                    break;
                case 0x70://movm,b
                    _state.M = _state.registers.B;
                    break;
                case 0x71://movm,c
                    _state.M = _state.registers.C;
                    break;
                case 0x72://movm,d
                    _state.M = _state.registers.D;
                    break;
                case 0x73://movm,e
                    _state.M = _state.registers.E;
                    break;
                case 0x74://movm,h
                    _state.M = _state.registers.H;
                    break;
                case 0x75://movm,l
                    _state.M = _state.registers.L;
                    break;
                case 0x76://hlt
                    halt.Invoke();
                    break;
                case 0x77://movm,a
                    _state.M = _state.registers.A;
                    break;
                case 0x78://mova,b
                    _state.registers.A = _state.registers.B;
                    break;
                case 0x79://mova,c
                    _state.registers.A = _state.registers.C;
                    break;
                case 0x7a://mova,d
                    _state.registers.A = _state.registers.D;
                    break;
                case 0x7b://mova,e
                    _state.registers.A = _state.registers.E;
                    break;
                case 0x7c://mova,h
                    _state.registers.A = _state.registers.H;
                    break;
                case 0x7d://mova,l
                    _state.registers.A = _state.registers.L;
                    break;
                case 0x7e://mova,m
                    _state.registers.A = _state.M;
                    break;
                case 0x7f://mova,a
                    _state.registers.A = _state.registers.A;
                    break;
                case 0x80://addb
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.B, false);
                    break;
                case 0x81://addc
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.C, false);
                    break;
                case 0x82://addd
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.D, false);
                    break;
                case 0x83://adde
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.E, false);
                    break;
                case 0x84://addh
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.H, false);
                    break;
                case 0x85://addl
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.L, false);
                    break;
                case 0x86://addm
                    _state.registers.A = AddByte(_state.registers.A, _state.M, false);
                    break;
                case 0x87://adda
                    _state.registers.A = AddByte(_state.registers.A, _state.registers.A, false);
                    break;
                case 0x88://adcb
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.B, false);
                    break;
                case 0x89://adcc
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.C, false);
                    break;
                case 0x8a://adcd
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.D, false);
                    break;
                case 0x8b://adce
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.E, false);
                    break;
                case 0x8c://adch
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.H, false);
                    break;
                case 0x8d://adcl
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.L, false);
                    break;
                case 0x8e://adcm
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.M, false);
                    break;
                case 0x8f://adca
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.registers.A, false);
                    break;
                case 0x90://subb
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.B, false);
                    break;
                case 0x91://subc
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.C, false);
                    break;
                case 0x92://subd
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.D, false);
                    break;
                case 0x93://sube
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.E, false);
                    break;
                case 0x94://subh
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.H, false);
                    break;
                case 0x95://subl
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.L, false);
                    break;
                case 0x96://subm
                    _state.registers.A = SubtractByte(_state.registers.A, _state.M, false);
                    break;
                case 0x97://suba
                    _state.registers.A = SubtractByte(_state.registers.A, _state.registers.A, false);
                    break;
                case 0x98://sbbb
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.B, false);
                    break;
                case 0x99://sbbc
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.C, false);
                    break;
                case 0x9a://sbbd
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.D, false);
                    break;
                case 0x9b://sbbe
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.E, false);
                    break;
                case 0x9c://sbbh
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.H, false);
                    break;
                case 0x9d://sbbl
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.L, false);
                    break;
                case 0x9e://sbbm
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.M, false);
                    break;
                case 0x9f://sbba
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.registers.A, false);
                    break;
                case 0xa0://anab
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.B);
                    LogicalOpFlags();
                    break;
                case 0xa1://anac
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.C);
                    LogicalOpFlags();
                    break;
                case 0xa2://anad
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.D);
                    LogicalOpFlags();
                    break;
                case 0xa3://anae
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.E);
                    LogicalOpFlags();
                    break;
                case 0xa4://anah
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.H);
                    LogicalOpFlags();
                    break;
                case 0xa5://anal
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.L);
                    LogicalOpFlags();
                    break;
                case 0xa6://anam
                    _state.registers.A = (byte)(_state.registers.A & _state.M);
                    LogicalOpFlags();
                    break;
                case 0xa7://anaa
                    _state.registers.A = (byte)(_state.registers.A & _state.registers.A);
                    LogicalOpFlags();
                    break;
                case 0xa8://xrab
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.B);
                    LogicalOpFlags();
                    break;
                case 0xa9://xrac
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.C);
                    LogicalOpFlags();
                    break;
                case 0xaa://xrad
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.D);
                    LogicalOpFlags();
                    break;
                case 0xab://xrae
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.E);
                    LogicalOpFlags();
                    break;
                case 0xac://xrah
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.H);
                    LogicalOpFlags();
                    break;
                case 0xad://xral
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.L);
                    LogicalOpFlags();
                    break;
                case 0xae://xram
                    _state.registers.A = (byte)(_state.registers.A ^ _state.M);
                    LogicalOpFlags();
                    break;
                case 0xaf://xraa
                    _state.registers.A = (byte)(_state.registers.A ^ _state.registers.A);
                    LogicalOpFlags();
                    break;
                case 0xb0://orab
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.B);
                    LogicalOpFlags();
                    break;
                case 0xb1://orac
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.C);
                    LogicalOpFlags();
                    break;
                case 0xb2://orad
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.D);
                    LogicalOpFlags();
                    break;
                case 0xb3://orae
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.E);
                    LogicalOpFlags();
                    break;
                case 0xb4://orah
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.H);
                    LogicalOpFlags();
                    break;
                case 0xb5://oral
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.L);
                    LogicalOpFlags();
                    break;
                case 0xb6://oram
                    _state.registers.A = (byte)(_state.registers.A | _state.M);
                    LogicalOpFlags();
                    break;
                case 0xb7://oraa
                    _state.registers.A = (byte)(_state.registers.A | _state.registers.A);
                    LogicalOpFlags();
                    break;
                case 0xb8://cmpb
                    SubtractByte(_state.registers.A, _state.registers.B, false);
                    break;
                case 0xb9://cmpc
                    SubtractByte(_state.registers.A, _state.registers.C, false);
                    break;
                case 0xba://cmpd
                    SubtractByte(_state.registers.A, _state.registers.D, false);
                    break;
                case 0xbb://cmpe
                    SubtractByte(_state.registers.A, _state.registers.E, false);
                    break;
                case 0xbc://cmph
                    SubtractByte(_state.registers.A, _state.registers.H, false);
                    break;
                case 0xbd://cmpl
                    SubtractByte(_state.registers.A, _state.registers.L, false);
                    break;
                case 0xbe://cmpm
                    SubtractByte(_state.registers.A, _state.M, false);
                    break;
                case 0xbf://cmpa
                    SubtractByte(_state.registers.A, _state.registers.A, false);
                    break;
                case 0xc0://rnz
                    if (!_state.flags.Z)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xc1://popb
                    _state.registers.BC = PopFromStack();
                    break;
                case 0xc2://jnz
                    if (!_state.flags.Z)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xc3://jmp
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    break;
                case 0xc4://cnz
                    if (!_state.flags.Z)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                    {
                        _state.PC += 2;
                    }
                    break;
                case 0xc5://pushb
                    PushToStack(_state.registers.BC);
                    break;
                case 0xc6://adi
                    _state.PC++;
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.Memory[_state.PC], false);
                    break;
                case 0xc7://rst0
                    {
                        _state.PC = 0x0000;
                        return;
                    }
                    break;
                case 0xc8://rz
                    if (_state.flags.Z)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xc9://ret
                    _state.PC = PopFromStack();
                    return;
                    break;
                case 0xca://jz
                    if (_state.flags.Z)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xcb://no inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0xcc://cz
                    if (_state.flags.Z)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xcd://call
                    PushToStack(_state.PC);
                    _state.PC = GetNextTwoBytes();
                    return;
                    break;
                case 0xce://aci
                    _state.PC++;
                    _state.registers.A = AddByteWithCarry(_state.registers.A, _state.Memory[_state.PC], false);
                    break;
                case 0xcf://rst1
                    {
                        _state.PC = 0x0008;
                        return;
                    }
                    break;
                case 0xd0://rnc
                    if (!_state.flags.CY)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xd1://popd
                    _state.registers.DE = PopFromStack();
                    break;
                case 0xd2://jnc
                    if (!_state.flags.CY)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xd3://out
                    _state.PC++;
                    _state.IO[_state.Memory[_state.PC]] = _state.registers.A;
                    break;
                case 0xd4://cnc
                    if (!_state.flags.CY)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xd5://pushd
                    PushToStack(_state.registers.DE);
                    break;
                case 0xd6://sui
                    _state.PC++;
                    _state.registers.A = SubtractByte(_state.registers.A, _state.Memory[_state.PC], false);
                    break;
                case 0xd7://rst2
                    {
                        _state.PC = 0x0010;
                        return;
                    }
                    break;
                case 0xd8://rc
                    if (_state.flags.CY)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xd9://no_inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0xda://jc
                    if (_state.flags.CY)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xdb://in
                    _state.PC++;
                    _state.registers.A = _state.IO[_state.Memory[_state.PC]];
                    break;
                case 0xdc://cc
                    if (_state.flags.CY)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xdd://no_inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0xde://sbi
                    _state.PC++;
                    _state.registers.A = SubtractByteWithBorrow(_state.registers.A, _state.Memory[_state.PC], false);
                    break;
                case 0xdf://rst3
                    {
                        _state.PC = 0x0018;
                        return;
                    }
                    break;
                case 0xe0://rpo
                    if (_state.flags.P == false)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xe1://poph
                    _state.registers.HL = PopFromStack();
                    break;
                case 0xe2://jpo
                    if (_state.flags.P == false)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xe3://xthl
                    {
                        ushort temp = _state.registers.HL;
                        _state.registers.HL = PopFromStack();
                        PushToStack(temp);
                    }
                    break;
                case 0xe4://cpo
                    if (_state.flags.P == false)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xe5://pushh
                    PushToStack(_state.registers.HL);
                    break;
                case 0xe6://ani
                    _state.PC++;
                    _state.registers.A = (byte)(_state.registers.A & _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xe7://rst 4
                    {
                        _state.PC = 0x0020;
                        return;
                    }
                    break;
                case 0xe8://rpe
                    if (_state.flags.P == true)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xe9://pchl
                    _state.PC = _state.registers.HL;
                    break;
                case 0xea://jpe
                    if (_state.flags.P == true)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xeb://xchg
                    {
                        ushort temp;
                        temp = _state.registers.HL;
                        _state.registers.HL = _state.registers.DE;
                        _state.registers.DE = temp;
                    }
                    break;
                case 0xec://cpe
                    if (_state.flags.P == true)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xed://no_inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0xee://xri
                    _state.PC++;
                    _state.registers.A = (byte)(_state.registers.A ^ _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xef://rst 5
                    {
                        _state.PC = 0x0028;
                        return;
                    }
                    break;
                case 0xf0://RP
                    if (_state.flags.S == false)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xf1://poppsw
                    _state.PSW = PopFromStack();
                    break;
                case 0xf2://jp
                    if (_state.flags.S == false)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xf3://di
                    _state.interruptStatus.InterruptEnable = false;
                    break;
                case 0xf4://cp
                    if (_state.flags.S == false)
                    {
                        PushToStack(_state.PC);
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xf5://pushpsw
                    PushToStack(_state.PSW);
                    break;
                case 0xf6://ori
                    _state.PC++;
                    _state.registers.A = (byte)(_state.registers.A | _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xf7://rst6
                    {
                        _state.PC = 0x0030;
                        return;
                    }
                    break;
                case 0xf8://rm
                    if (_state.flags.S)
                    {
                        _state.PC = PopFromStack();
                        return;
                    }
                    break;
                case 0xf9://sphl
                    _state.SP = _state.registers.HL;
                    break;
                case 0xfa://jm
                    if (_state.flags.S)
                    {
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xfb://ei
                    _state.interruptStatus.InterruptEnable = true;
                    break;
                case 0xfc://cm
                    if (_state.flags.S)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                        return;
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xfd://no_inst
                    throw new Exception($"Invalid Instruction encountered at memory {_state.PC}");
                    break;
                case 0xfe://cpi
                    _state.PC++;
                    SubtractByteWithBorrow(_state.registers.A, _state.Memory[_state.PC], false);
                    break;
                case 0xff://rst7
                    {
                        _state.PC = 0x0038;
                        return;
                    }
                    break;
            }
            _state.PC++;
        }

        public void Interrupt_TRAP()
        {

        }

        public void Interrupt_RST7_5()
        {

        }

        public void Interrupt_RST6_5()
        {

        }

        public void Interrupt_RST5_5()
        {

        }

        public void Interrupt_INTR()
        {

        }

        private void PushToStack(ushort element)
        {
            _state.SP -= 2;
            byte[] temp = BitConverter.GetBytes(element);
            _state.Memory[_state.SP] = temp[0];
            _state.Memory[_state.SP + 1] = temp[1];
        }

        private ushort PopFromStack()
        {
            var temp = BitConverter.ToUInt16(_state.Memory,_state.SP);
            _state.SP += 2;
            return temp;
        }

        private static bool CheckParity(byte Reg)
        {
            int count = 0;
            int temp = Reg;
            for (int i = 0; i < 8; i++)
            {
                if ((temp & 1) == 1)
                {
                    count++;
                }
                temp = temp >> 1;
            }
            return ((count & 1) == 0);
        }

        private void LogicalOpFlags()
        {
            _state.flags.CY = false;
            _state.flags.AC = false;
            _state.flags.Z = (_state.registers.A == 0);
            _state.flags.S = (_state.registers.A / 128 == 1);
            _state.flags.P = CheckParity(_state.registers.A);
        }

        private void ArithmeticOpFlags(ushort result, bool keep_carry)
        {
            var temp = BitConverter.GetBytes(result);
            if(!keep_carry)
                _state.flags.CY = (result > 0xff);
            _state.flags.AC = false;
            _state.flags.Z = (result == 0);
            _state.flags.S = (result / 128 == 1);
            _state.flags.P = CheckParity(temp[0]);
        }

        private byte AddByte(byte augend, byte addend, bool keep_carry)
        {
            ushort result = (ushort)(augend + addend);
            ArithmeticOpFlags(result, keep_carry);
            if ((augend & 0xf) + (addend & 0xf) > 0xf)
                _state.flags.AC = true;
            return (byte)result;
        }

        private byte AddByteWithCarry(byte augend, byte addend, bool keep_carry)
        {
            ushort result = (ushort)(augend + addend + (_state.flags.CY ? 1 : 0));
            ArithmeticOpFlags(result, keep_carry);
            return (byte)result;
        }

        private byte SubtractByte(byte minuend, byte subtrahend, bool keep_carry)
        {
            ushort result = (ushort)(minuend - subtrahend);
            ArithmeticOpFlags(result, keep_carry);
            if ((minuend & 0xf) + (~subtrahend & 0xf) + 1 > 0xf)
                _state.flags.AC = true;
            return (byte)result;
        }

        private byte SubtractByteWithBorrow(byte minuend, byte subtrahend, bool keep_carry)
        {
            ushort result = (ushort)(minuend - subtrahend - (_state.flags.CY ? 1 : 0));
            ArithmeticOpFlags(result, keep_carry);
            return (byte)result;
        }

        public void Run()
        {
            while(_state.Memory[_state.PC]!=0x76 && _state.PC<=ushort.MaxValue)
            {
                Simulate();
            }
        }
    }
}
