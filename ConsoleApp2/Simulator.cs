using System;

namespace AssemblerSimulator8085
{
    internal class Simulator
    {
        private ushort start_at = 0; //default at 0
        private State _state;
        public Simulator(State state)
        {
            _state = state;
        }
        public void Reset()
        {
            _state.PC = start_at;
        }
        public void StartAt(ushort memory_address)
        {
            start_at = memory_address;
            Reset();
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
        private void Simulate()
        {
            switch (_state.Memory[_state.PC])
            {
                case 0x0://nop
                    break;
                case 0x1://lxib
                    _state.BC = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x2: //staxb
                    _state.Memory[_state.BC] = _state.A;
                    break;
                case 0x3://inxb
                    _state.BC++;
                    break;
                case 0x4://inrb
                    _state.B = AddByte(_state.B, 1, true);
                    break;
                case 0x5://dcrb
                    _state.B = SubtractByte(_state.B, 1, true);
                    break;
                case 0x6://mvib
                    _state.PC++;
                    _state.B = _state.Memory[_state.PC];
                    break;
                case 0x7://rlc
                    if(_state.A>0x80)
                    {
                        _state.A = (byte)(_state.A << 1);
                        _state.A++;
                    }
                    else
                    {
                        _state.A = (byte)(_state.A << 1);
                    }
                    break;
                case 0x8://no inst
                    break;
                case 0x9://dadb
                    _state.CY = (_state.HL + _state.BC > 0xffff);
                    _state.HL = (ushort)(_state.HL + _state.BC);
                    break;
                case 0xa://ldaxb
                    _state.A = _state.Memory[_state.BC];
                    break;
                case 0xb://dcxb
                    _state.BC--;
                    break;
                case 0xc://inrc
                    _state.C = AddByte(_state.C, 1, true);
                    break;
                case 0xd://dcrc
                    _state.C = SubtractByte(_state.C, 1, true);
                    break;
                case 0xe://mvic
                    _state.PC++;
                    _state.C = _state.Memory[_state.PC];
                    break;
                case 0xf://rrc
                    if(_state.A % 2 == 0)
                    {
                        _state.A = (byte)(_state.A >> 1);
                    }
                    else
                    {
                        _state.A = (byte)((_state.A >> 1));
                        _state.A += 0x80;
                    }
                    break;
                case 0x10://no inst
                    break;
                case 0x11://lxid
                    _state.DE = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x12://staxd
                    _state.Memory[_state.DE] = _state.A;
                    break;
                case 0x13://inxd
                    _state.DE++;
                    break;
                case 0x14://inrd
                    _state.D = AddByte(_state.D, 1, true);
                    break;
                case 0x15://dcrd
                    _state.D = SubtractByte(_state.D, 1, true);
                    break;
                case 0x16://mvid
                    _state.PC++;
                    _state.D = _state.Memory[_state.PC];
                    break;
                case 0x17://ral
                    if(_state.CY)
                    {
                        _state.A = (byte)(_state.A << 1);
                        _state.A++;
                        _state.CY = (_state.A > 0x80);
                    }
                    else
                    {
                        _state.A = (byte)(_state.A << 1);
                        _state.CY = (_state.A > 0x80);
                    }
                    break;
                case 0x18://no inst
                    break;
                case 0x19://dadd
                    _state.CY = (_state.HL + _state.DE > 0xffff);
                    _state.HL = (ushort)(_state.HL + _state.DE);
                    break;
                case 0x1a://ldaxd
                    _state.A = _state.Memory[_state.DE];
                    break;
                case 0x1b://dcxd
                    _state.DE--;
                    break;
                case 0x1c://inre
                    _state.E = AddByte(_state.E, 1, true);
                    break;
                case 0x1d://dcre
                    _state.E = SubtractByte(_state.E, 1, true);
                    break;
                case 0x1e://mvie
                    _state.PC++;
                    _state.E = _state.Memory[_state.PC];
                    break;
                case 0x1f://rar
                    if(_state.CY)//refactor
                    {
                        if(_state.A%2==0)
                        {
                            _state.A = (byte)(_state.A >> 1);
                            _state.A += 0x80;
                            _state.CY = false;
                        }
                        else
                        {
                            _state.A = (byte)(_state.A >> 1);
                            _state.A += 0x80;
                        }
                    }
                    else
                    {
                        if(_state.A%2==0)
                        {
                            _state.A = (byte)(_state.A >> 1);
                        }
                        else
                        {
                            _state.A = (byte)(_state.A >> 1);
                            _state.CY = true;
                        }
                    }
                    break;
                case 0x20://rim

                    break;
                case 0x21://lxih
                    _state.HL = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x22://shld
                    _state.PC++;
                    _state.Memory[_state.PC] = _state.L;
                    _state.PC++;
                    _state.Memory[_state.PC] = _state.H;
                    break;
                case 0x23://inxh
                    _state.HL++;
                    break;
                case 0x24://inrh
                    _state.H = AddByte(_state.H, 1, true);
                    break;
                case 0x25://dcrh
                    _state.H = SubtractByte(_state.H, 1, true);
                    break;
                case 0x26://mvih
                    _state.PC++;
                    _state.H = _state.Memory[_state.PC];
                    break;
                case 0x27://daa

                    break;
                case 0x28://no-inst
                    break;
                case 0x29://dadh
                    _state.CY = (_state.HL + _state.HL > 0xffff);
                    _state.HL = (ushort)(_state.HL + _state.HL);
                    break;
                case 0x2a://lhld
                    _state.HL = BitConverter.ToUInt16(_state.Memory, GetNextTwoBytes());
                    break;
                case 0x2b://dcxh
                    _state.HL--;
                    break;
                case 0x2c://inrl
                    _state.L = AddByte(_state.L, 1, true);
                    break;
                case 0x2d://dcrl
                    _state.L = SubtractByte(_state.L, 1, true);
                    break;
                case 0x2e://mvil
                    _state.PC++;
                    _state.L = _state.Memory[_state.PC];
                    break;
                case 0x2f://cma
                    _state.A = (byte)~_state.A;
                    break;
                case 0x30://no_inst
                    break;
                case 0x31://lxisp
                    _state.SP = GetNextTwoBytes();
                    _state.PC += 2;
                    break;
                case 0x32://sta
                    _state.Memory[GetNextTwoBytes()] = _state.A;
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
                    _state.CY = true;
                    break;
                case 0x38://no_inst
                    break;
                case 0x39://dadsp
                    _state.CY = (_state.HL + _state.SP > 0xffff);
                    _state.HL = (ushort)(_state.HL + _state.SP);
                    break;
                case 0x3a://lda
                    _state.A = _state.Memory[GetNextTwoBytes()];
                    _state.PC += 2;
                    break;
                case 0x3b://dcxsp
                    _state.SP--;
                    break;
                case 0x3c://inra
                    _state.A = AddByte(_state.A, 1, true);
                    break;
                case 0x3d://dcra
                    _state.A = SubtractByte(_state.A, 1, true);
                    break;
                case 0x3e://mvia
                    _state.PC++;
                    _state.A = _state.Memory[_state.PC];
                    break;
                case 0x3f://cmc
                    _state.CY = !_state.CY;
                    break;
                case 0x40://movb,b        
                    _state.B = _state.B;
                    break;
                case 0x41://movb,c        
                    _state.B = _state.C;
                    break;
                case 0x42://movb,d
                    _state.B = _state.D;
                    break;
                case 0x43://movb,e
                    _state.B = _state.E;
                    break;
                case 0x44://movb,h
                    _state.B = _state.H;
                    break;
                case 0x45://movb,l
                    _state.B = _state.L;
                    break;
                case 0x46://movb,m
                    _state.B = _state.M;
                    break;
                case 0x47://movb,a
                    _state.B = _state.A;
                    break;
                case 0x48://movc,b
                    _state.C = _state.B;
                    break;
                case 0x49://movc,c
                    _state.C = _state.C;
                    break;
                case 0x4a://movc,d
                    _state.C = _state.D;
                    break;
                case 0x4b://movc,e
                    _state.C = _state.E;
                    break;
                case 0x4c://movc,h
                    _state.C = _state.H;
                    break;
                case 0x4d://movc,l
                    _state.C = _state.L;
                    break;
                case 0x4e://movc,m
                    _state.C = _state.M;
                    break;
                case 0x4f://movc,a
                    _state.C = _state.A;
                    break;
                case 0x50://movd,b
                    _state.D = _state.B;
                    break;
                case 0x51://movd,c
                    _state.D = _state.C;
                    break;
                case 0x52://movd,d
                    _state.D = _state.D;
                    break;
                case 0x53://movd,e
                    _state.D = _state.E;
                    break;
                case 0x54://movd,h
                    _state.D = _state.H;
                    break;
                case 0x55://movd,l
                    _state.D = _state.L;
                    break;
                case 0x56://movd,m
                    _state.D = _state.M;
                    break;
                case 0x57://movd,a
                    _state.D = _state.A;
                    break;
                case 0x58://move,b
                    _state.E = _state.B;
                    break;
                case 0x59://move,c
                    _state.E = _state.C;
                    break;
                case 0x5a://move,d
                    _state.E = _state.D;
                    break;
                case 0x5b://move,e
                    _state.E = _state.E;
                    break;
                case 0x5c://move,h
                    _state.E = _state.H;
                    break;
                case 0x5d://move,l
                    _state.E = _state.L;
                    break;
                case 0x5e://move,m
                    _state.E = _state.M;
                    break;
                case 0x5f://move,a
                    _state.E = _state.A;
                    break;
                case 0x60://movh,b
                    _state.H = _state.B;
                    break;
                case 0x61://movh,c
                    _state.H = _state.C;
                    break;
                case 0x62://movh,d
                    _state.H = _state.D;
                    break;
                case 0x63://movh,e
                    _state.H = _state.E;
                    break;
                case 0x64://movh,h
                    _state.H = _state.H;
                    break;
                case 0x65://movh,l
                    _state.H = _state.L;
                    break;
                case 0x66://movh,m
                    _state.H = _state.M;
                    break;
                case 0x67://movh,a
                    _state.H = _state.A;
                    break;
                case 0x68://movl,b
                    _state.L = _state.B;
                    break;
                case 0x69://movl,c
                    _state.L = _state.C;
                    break;
                case 0x6a://movl,d
                    _state.L = _state.D;
                    break;
                case 0x6b://movl,e
                    _state.L = _state.E;
                    break;
                case 0x6c://movl,h
                    _state.L = _state.H;
                    break;
                case 0x6d://movl,l
                    _state.L = _state.L;
                    break;
                case 0x6e://movl,m
                    _state.L = _state.M;
                    break;
                case 0x6f://movl,a
                    _state.L = _state.A;
                    break;
                case 0x70://movm,b
                    _state.M = _state.B;
                    break;
                case 0x71://movm,c
                    _state.M = _state.C;
                    break;
                case 0x72://movm,d
                    _state.M = _state.D;
                    break;
                case 0x73://movm,e
                    _state.M = _state.E;
                    break;
                case 0x74://movm,h
                    _state.M = _state.H;
                    break;
                case 0x75://movm,l
                    _state.M = _state.L;
                    break;
                case 0x76://movm,m
                    _state.M = _state.M;
                    break;
                case 0x77://movm,a
                    _state.M = _state.A;
                    break;
                case 0x78://mova,b
                    _state.A = _state.B;
                    break;
                case 0x79://mova,c
                    _state.A = _state.C;
                    break;
                case 0x7a://mova,d
                    _state.A = _state.D;
                    break;
                case 0x7b://mova,e
                    _state.A = _state.E;
                    break;
                case 0x7c://mova,h
                    _state.A = _state.H;
                    break;
                case 0x7d://mova,l
                    _state.A = _state.L;
                    break;
                case 0x7e://mova,m
                    _state.A = _state.M;
                    break;
                case 0x7f://mova,a
                    _state.A = _state.A;
                    break;
                case 0x80://addb
                    _state.A = AddByte(_state.A, _state.B, false);
                    break;
                case 0x81://addc
                    _state.A = AddByte(_state.A, _state.C, false);
                    break;
                case 0x82://addd
                    _state.A = AddByte(_state.A, _state.D, false);
                    break;
                case 0x83://adde
                    _state.A = AddByte(_state.A, _state.E, false);
                    break;
                case 0x84://addh
                    _state.A = AddByte(_state.A, _state.H, false);
                    break;
                case 0x85://addl
                    _state.A = AddByte(_state.A, _state.L, false);
                    break;
                case 0x86://addm
                    _state.A = AddByte(_state.A, _state.M, false);
                    break;
                case 0x87://adda
                    _state.A = AddByte(_state.A, _state.A, false);
                    break;
                case 0x88://adcb
                    _state.A = AddByteWithCarry(_state.A, _state.B, false);
                    break;
                case 0x89://adcc
                    _state.A = AddByteWithCarry(_state.A, _state.C, false);
                    break;
                case 0x8a://adcd
                    _state.A = AddByteWithCarry(_state.A, _state.D, false);
                    break;
                case 0x8b://adce
                    _state.A = AddByteWithCarry(_state.A, _state.E, false);
                    break;
                case 0x8c://adch
                    _state.A = AddByteWithCarry(_state.A, _state.H, false);
                    break;
                case 0x8d://adcl
                    _state.A = AddByteWithCarry(_state.A, _state.L, false);
                    break;
                case 0x8e://adcm
                    _state.A = AddByteWithCarry(_state.A, _state.M, false);
                    break;
                case 0x8f://adca
                    _state.A = AddByteWithCarry(_state.A, _state.A, false);
                    break;
                case 0x90://subb
                    _state.A = SubtractByte(_state.A, _state.B, false);
                    break;
                case 0x91://subc
                    _state.A = SubtractByte(_state.A, _state.C, false);
                    break;
                case 0x92://subd
                    _state.A = SubtractByte(_state.A, _state.D, false);
                    break;
                case 0x93://sube
                    _state.A = SubtractByte(_state.A, _state.E, false);
                    break;
                case 0x94://subh
                    _state.A = SubtractByte(_state.A, _state.H, false);
                    break;
                case 0x95://subl
                    _state.A = SubtractByte(_state.A, _state.L, false);
                    break;
                case 0x96://subm
                    _state.A = SubtractByte(_state.A, _state.M, false);
                    break;
                case 0x97://suba
                    _state.A = SubtractByte(_state.A, _state.A, false);
                    break;
                case 0x98://sbbb
                    _state.A = SubtractByteWithBorrow(_state.A, _state.B, false);
                    break;
                case 0x99://sbbc
                    _state.A = SubtractByteWithBorrow(_state.A, _state.C, false);
                    break;
                case 0x9a://sbbd
                    _state.A = SubtractByteWithBorrow(_state.A, _state.D, false);
                    break;
                case 0x9b://sbbe
                    _state.A = SubtractByteWithBorrow(_state.A, _state.E, false);
                    break;
                case 0x9c://sbbh
                    _state.A = SubtractByteWithBorrow(_state.A, _state.H, false);
                    break;
                case 0x9d://sbbl
                    _state.A = SubtractByteWithBorrow(_state.A, _state.L, false);
                    break;
                case 0x9e://sbbm
                    _state.A = SubtractByteWithBorrow(_state.A, _state.M, false);
                    break;
                case 0x9f://sbba
                    _state.A = SubtractByteWithBorrow(_state.A, _state.A, false);
                    break;
                case 0xa0://anab
                    _state.A = (byte)(_state.A & _state.B);
                    LogicalOpFlags();
                    break;
                case 0xa1://anac
                    _state.A = (byte)(_state.A & _state.C);
                    LogicalOpFlags();
                    break;
                case 0xa2://anad
                    _state.A = (byte)(_state.A & _state.D);
                    LogicalOpFlags();
                    break;
                case 0xa3://anae
                    _state.A = (byte)(_state.A & _state.E);
                    LogicalOpFlags();
                    break;
                case 0xa4://anah
                    _state.A = (byte)(_state.A & _state.H);
                    LogicalOpFlags();
                    break;
                case 0xa5://anal
                    _state.A = (byte)(_state.A & _state.L);
                    LogicalOpFlags();
                    break;
                case 0xa6://anam
                    _state.A = (byte)(_state.A & _state.M);
                    LogicalOpFlags();
                    break;
                case 0xa7://anaa
                    _state.A = (byte)(_state.A & _state.A);
                    LogicalOpFlags();
                    break;
                case 0xa8://xrab
                    _state.A = (byte)(_state.A ^ _state.B);
                    LogicalOpFlags();
                    break;
                case 0xa9://xrac
                    _state.A = (byte)(_state.A ^ _state.C);
                    LogicalOpFlags();
                    break;
                case 0xaa://xrad
                    _state.A = (byte)(_state.A ^ _state.D);
                    LogicalOpFlags();
                    break;
                case 0xab://xrae
                    _state.A = (byte)(_state.A ^ _state.E);
                    LogicalOpFlags();
                    break;
                case 0xac://xrah
                    _state.A = (byte)(_state.A ^ _state.H);
                    LogicalOpFlags();
                    break;
                case 0xad://xral
                    _state.A = (byte)(_state.A ^ _state.L);
                    LogicalOpFlags();
                    break;
                case 0xae://xram
                    _state.A = (byte)(_state.A ^ _state.M);
                    LogicalOpFlags();
                    break;
                case 0xaf://xraa
                    _state.A = (byte)(_state.A ^ _state.A);
                    LogicalOpFlags();
                    break;
                case 0xb0://orab
                    _state.A = (byte)(_state.A | _state.B);
                    LogicalOpFlags();
                    break;
                case 0xb1://orac
                    _state.A = (byte)(_state.A | _state.C);
                    LogicalOpFlags();
                    break;
                case 0xb2://orad
                    _state.A = (byte)(_state.A | _state.D);
                    LogicalOpFlags();
                    break;
                case 0xb3://orae
                    _state.A = (byte)(_state.A | _state.E);
                    LogicalOpFlags();
                    break;
                case 0xb4://orah
                    _state.A = (byte)(_state.A | _state.H);
                    LogicalOpFlags();
                    break;
                case 0xb5://oral
                    _state.A = (byte)(_state.A | _state.L);
                    LogicalOpFlags();
                    break;
                case 0xb6://oram
                    _state.A = (byte)(_state.A | _state.M);
                    LogicalOpFlags();
                    break;
                case 0xb7://oraa
                    _state.A = (byte)(_state.A | _state.A);
                    LogicalOpFlags();
                    break;
                case 0xb8://cmpb
                    SubtractByte(_state.A, _state.B, false);
                    break;
                case 0xb9://cmpc
                    SubtractByte(_state.A, _state.C, false);
                    break;
                case 0xba://cmpd
                    SubtractByte(_state.A, _state.D, false);
                    break;
                case 0xbb://cmpe
                    SubtractByte(_state.A, _state.E, false);
                    break;
                case 0xbc://cmph
                    SubtractByte(_state.A, _state.H, false);
                    break;
                case 0xbd://cmpl
                    SubtractByte(_state.A, _state.L, false);
                    break;
                case 0xbe://cmpm
                    SubtractByte(_state.A, _state.M, false);
                    break;
                case 0xbf://cmpa
                    SubtractByte(_state.A, _state.A, false);
                    break;
                case 0xc0://rnz
                    if(!_state.Z)
                    {
                        _state.PC = PopFromStack();
                    }
                    break;
                case 0xc1://popb
                    _state.BC = PopFromStack();
                    break;
                case 0xc2://jnz
                    if (!_state.Z)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xc3://jmp
                    _state.PC = GetNextTwoBytes();
                    break;
                case 0xc4://cnz
                    if(!_state.Z)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                    {
                        _state.PC += 2;
                    }
                    break;
                case 0xc5://pushb
                    PushToStack(_state.BC);
                    break;
                case 0xc6://adi
                    _state.PC++;
                    _state.A = AddByteWithCarry(_state.A, _state.Memory[_state.PC], false);
                    break;
                case 0xc7://rst0
                    break;
                case 0xc8://rz
                    if (_state.Z)
                        _state.PC = PopFromStack();
                    break;
                case 0xc9://ret
                    _state.PC = PopFromStack();
                    break;
                case 0xca://jz
                    if (_state.Z)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xcb://no inst
                    break;
                case 0xcc://cz
                    if (_state.Z)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xcd://call
                    PushToStack(_state.PC);
                    _state.PC = GetNextTwoBytes();
                    break;
                case 0xce://aci
                    _state.PC++;
                    _state.A = AddByteWithCarry(_state.A, _state.Memory[_state.PC], false);
                    break;
                case 0xcf://rst1
                    break;
                case 0xd0://rnc
                    if (!_state.CY)
                        _state.PC = PopFromStack();
                    break;
                case 0xd1://popd
                    _state.DE = PopFromStack();
                    break;
                case 0xd2://jnc
                    if (!_state.CY)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xd3://out
                    break;
                case 0xd4://cnc
                    if (!_state.CY)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xd5://pushd
                    PushToStack(_state.DE);
                    break;
                case 0xd6://sui
                    _state.PC++;
                    _state.A = SubtractByte(_state.A, _state.Memory[_state.PC], false);
                    break;
                case 0xd7://rst2
                    break;
                case 0xd8://rc
                    if(_state.CY)
                        _state.PC = PopFromStack();
                    break;
                case 0xd9://no_inst
                    break;
                case 0xda://jc
                    if (_state.CY)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xdb://in
                    break;
                case 0xdc://cc
                    if (_state.CY)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xdd://no_inst
                    break;
                case 0xde://sbi
                    _state.PC++;
                    _state.A = SubtractByteWithBorrow(_state.A, _state.Memory[_state.PC], false);
                    break;
                case 0xdf://rst3
                    break;
                case 0xe0://rpo
                    if (_state.P == false)
                        _state.PC = PopFromStack();
                    break;
                case 0xe1://poph
                    _state.HL = PopFromStack();
                    break;
                case 0xe2://jpo
                    if (_state.P == false)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xe3://xthl
                    {
                        ushort temp = _state.HL;
                        _state.HL = _state.SP;
                        _state.SP = temp;
                    }
                    break;
                case 0xe4://cpo
                    if (_state.P == false)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xe5://pushh
                    PushToStack(_state.HL);
                    break;
                case 0xe6://ani
                    _state.PC++;
                    _state.A = (byte)(_state.A & _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xe7://rst 4
                    break;
                case 0xe8://rpe
                    if(_state.P==true)
                        _state.PC = PopFromStack();
                    break;
                case 0xe9://pchl
                    _state.PC = _state.HL;
                    break;
                case 0xea://jpe
                    if (_state.P == true)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xeb://xchg
                    {
                        ushort temp;
                        temp = _state.HL;
                        _state.HL = _state.DE;
                        _state.DE = temp;
                    }
                    break;
                case 0xec://cpe
                    if (_state.P == true)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xed://no_inst
                    break;
                case 0xee://xri
                    _state.PC++;
                    _state.A = (byte)(_state.A ^ _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xef://rst 5
                    break;
                case 0xf0://RP
                    if (_state.S==false)
                        _state.PC = PopFromStack();
                    break;
                case 0xf1://poppsw
                    _state.PSW = PopFromStack();
                    break;
                case 0xf2://jp
                    if (_state.S == false)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xf3://di
                    break;
                case 0xf4://cp
                    if (_state.S == false)
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
                    _state.A = (byte)(_state.A | _state.Memory[_state.PC]);
                    LogicalOpFlags();
                    break;
                case 0xf7://rst6
                    break;
                case 0xf8://rm
                    if (_state.S)
                        _state.PC = PopFromStack();
                    break;
                case 0xf9://sphl
                    _state.SP = _state.HL;
                    break;
                case 0xfa://jm
                    if (_state.S)
                        _state.PC = GetNextTwoBytes();
                    else
                        _state.PC += 2;
                    break;
                case 0xfb://ei
                    break;
                case 0xfc://cm
                    if (_state.S)
                    {
                        PushToStack(_state.PC);
                        _state.PC = GetNextTwoBytes();
                    }
                    else
                        _state.PC += 2;
                    break;
                case 0xfd://no_inst
                    break;
                case 0xfe://cpi
                    _state.PC++;
                    SubtractByteWithBorrow(_state.A, _state.Memory[_state.PC], false);
                    break;
                case 0xff://rst7
                    break;
            }
            _state.PC++;
        }
        private void PushToStack(ushort element)
        {
            _state.SP -= 2;
            byte[] temp = BitConverter.GetBytes(element);
            _state.Memory[_state.SP + 1] = temp[0];
            _state.Memory[_state.SP + 2] = temp[1];
        }
        private ushort PopFromStack()
        {
            _state.SP += 2;
            return BitConverter.ToUInt16(_state.Memory, _state.SP-2);
        }
        private static bool CheckParity(byte Reg)
        {
            int count = 0;
            int temp = Reg;
            for (int i = 0; i < 8; i++)
            {
                if (temp % 2 == 0)
                {
                    count++;
                    temp = temp >> 1;
                }
            }
            return (count % 2 == 0);
        }
        private void LogicalOpFlags()
        {
            _state.CY = false;
            _state.AC = false;
            _state.Z = (_state.A == 0);
            _state.S = (_state.A / 128 == 1);
            _state.P = CheckParity(_state.A);
        }
        private void ArithmeticOpFlags(ushort result, bool keep_carry)
        {
            var temp = BitConverter.GetBytes(result);
            if(!keep_carry)
                _state.CY = (result > 0xff);
            _state.AC = false;
            _state.Z = (result == 0);
            _state.S = (result / 128 == 1);
            _state.P = CheckParity(temp[0]);
        }
        private byte AddByte(byte augend, byte addend, bool keep_carry)
        {
            ushort result = (ushort)(augend + addend);
            ArithmeticOpFlags(result, keep_carry);
            if ((augend & 0xf) + (addend & 0xf) > 0xf)
                _state.AC = true;
            return (byte)result;
        }
        private byte AddByteWithCarry(byte augend, byte addend, bool keep_carry)
        {
            ushort result = (ushort)(augend + addend + (_state.CY ? 1 : 0));
            ArithmeticOpFlags(result, keep_carry);
            return (byte)result;
        }

        private byte SubtractByte(byte minuend, byte subtrahend, bool keep_carry)
        {
            ushort result = (ushort)(minuend - subtrahend);
            ArithmeticOpFlags(result, keep_carry);
            if ((minuend & 0xf) + (~subtrahend & 0xf) + 1 > 0xf)
                _state.AC = true;
            return (byte)result;
        }

        private byte SubtractByteWithBorrow(byte minuend, byte subtrahend, bool keep_carry)
        {
            ushort result = (ushort)(minuend - subtrahend - (_state.CY ? 1 : 0));
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
