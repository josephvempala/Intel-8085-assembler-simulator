using System.Collections.Generic;

namespace AssemblerSimulator8085.Assembler
{
    internal partial class Assembler8085
    {
        private readonly Dictionary<string, byte> Instructions = new Dictionary<string, byte>()
        {
            {"ADCA",0x8F},
            {"ACI",0xCE},
            {"ADCB",0x88},
            {"ADCC",0x89},
            {"ADCD",0x8A},
            {"ADCE",0x8B},
            {"ADCH",0x8C},
            {"ADCL",0x8D},
            {"ADCM",0x8E},
            {"ADDA",0x87},
            {"ADDB",0x80},
            {"ADDC",0x81},
            {"ADDD",0x82},
            {"ADDE",0x83},
            {"ADDH",0x84},
            {"ADDL",0x85},
            {"ADDM",0x86},
            {"ADI",0xC6},
            {"ANAA",0xA7},
            {"ANAB",0xA0},
            {"ANAC",0xA1},
            {"ANAD",0xA2},
            {"ANAE",0xA3},
            {"ANAH",0xA4},
            {"ANAL",0xA5},
            {"ANAM",0xA6},
            {"ANI",0xE6},
            {"CALL",0xCD},
            {"CC",0xDC},
            {"CM",0xFC},
            {"CMA",0x2F},
            {"CMC",0x3F},
            {"CMPA",0xBF},
            {"CMPB",0xB8},
            {"CMPC",0xB9},
            {"CMPD",0xBA},
            {"CMPE",0xBB},
            {"CMPH",0xBC},
            {"CMPL",0xBD},
            {"CMPM",0xBE},
            {"CNC",0xD4},
            {"CNZ",0xC4},
            {"CP",0xF4},
            {"CPE",0xEC},
            {"CPI",0xFE},
            {"CPO",0xE4},
            {"CZ",0xCC},
            {"DAA",0x27},
            {"DADB",0x9},
            {"DADD",0x19},
            {"DADH",0x29},
            {"DADSP",0x39},
            {"DCRA",0x3D},
            {"DCRB",0x5},
            {"DCRC",0x0D},
            {"DCRD",0x15},
            {"DCRE",0x1D},
            {"DCRH",0x25},
            {"DCRL",0x2D},
            {"DCRM",0x35},
            {"DCXB",0x0B},
            {"DCXD",0x1B},
            {"DCXH",0x2B},
            {"DCXSP",0x3B},
            {"DI",0xF3},
            {"EI",0xFB},
            {"HLT",0x76},
            {"INRA",0x3C},
            {"INRB",0x4},
            {"INRC",0x0C},
            {"INRD",0x14},
            {"INRE",0x1C},
            {"INRH",0x24},
            {"INRL",0x2C},
            {"INRM",0x34},
            {"INXB",0x3},
            {"INXD",0x13},
            {"INXH",0x23},
            {"INXSP",0x33},
            {"JC",0xDA},
            {"JM",0xFA},
            {"JMP",0xC3},
            {"JNC",0xD2},
            {"JNZ",0xC2},
            {"JP",0xF2},
            {"JPE",0xEA},
            {"JPO",0xE2},
            {"JZ",0xCA},
            {"LDA",0x3A},
            {"LDAXB",0x0A},
            {"LDAXD",0x1A},
            {"LHLD",0x2A},
            {"LXIB,",0x1},
            {"LXID,",0x11},
            {"LXIH,",0x21},
            {"LXISP,",0x31},
            {"MOVA,A",0x7F},
            {"MOVA,B",0x78},
            {"MOVA,C",0x79},
            {"MOVA,D",0x7A},
            {"MOVA,E",0x7B},
            {"MOVA,H",0x7C},
            {"MOVA,L",0x7D},
            {"MOVA,M",0x7E},
            {"MOVB,A",0x47},
            {"MOVB,B",0x40},
            {"MOVB,C",0x41},
            {"MOVB,D",0x42},
            {"MOVB,E",0x43},
            {"MOVB,H",0x44},
            {"MOVB,L",0x45},
            {"MOVB,M",0x46},
            {"MOVC,A",0x4F},
            {"MOVC,B",0x48},
            {"MOVC,C",0x49},
            {"MOVC,D",0x4A},
            {"MOVC,E",0x4B},
            {"MOVC,H",0x4C},
            {"MOVC,L",0x4D},
            {"MOVC,M",0x4E},
            {"MOVD,A",0x57},
            {"MOVD,B",0x50},
            {"MOVD,C",0x51},
            {"MOVD,D",0x52},
            {"MOVD,E",0x53},
            {"MOVD,H",0x54},
            {"MOVD,L",0x55},
            {"MOVD,M",0x56},
            {"MOVE,A",0x5F},
            {"MOVE,B",0x58},
            {"MOVE,C",0x59},
            {"MOVE,D",0x5A},
            {"MOVE,E",0x5B},
            {"MOVE,H",0x5C},
            {"MOVE,L",0x5D},
            {"MOVE,M",0x5E},
            {"MOVH,A",0x67},
            {"MOVH,B",0x60},
            {"MOVH,C",0x61},
            {"MOVH,D",0x62},
            {"MOVH,E",0x63},
            {"MOVH,H",0x64},
            {"MOVH,L",0x65},
            {"MOVH,M",0x66},
            {"MOVL,A",0x6F},
            {"MOVL,B",0x68},
            {"MOVL,C",0x69},
            {"MOVL,D",0x6A},
            {"MOVL,E",0x6B},
            {"MOVL,H",0x6C},
            {"MOVL,L",0x6D},
            {"MOVL,M",0x6E},
            {"MOVM,A",0x77},
            {"MOVM,B",0x70},
            {"MOVM,C",0x71},
            {"MOVM,D",0x72},
            {"MOVM,E",0x73},
            {"MOVM,H",0x74},
            {"MOVM,L",0x75},
            {"MVIA,",0x3E},
            {"MVIB,",0x6},
            {"MVIC,",0x0E},
            {"MVID,",0x16},
            {"MVIE,",0x1E},
            {"MVIH,",0x26},
            {"MVIL,",0x2E},
            {"MVIM,",0x36},
            {"NOP",0x0},
            {"ORAA",0xB7},
            {"ORAB",0xB0},
            {"ORAC",0xB1},
            {"ORAD",0xB2},
            {"ORAE",0xB3},
            {"ORAH",0xB4},
            {"ORAL",0xB5},
            {"ORAM",0xB6},
            {"ORI",0xF6},
            {"PCHL",0xE9},
            {"POPB",0xC1},
            {"POPD",0xD1},
            {"POPH",0xE1},
            {"POPPSW",0xF1},
            {"PUSHB",0xC5},
            {"PUSHD",0xD5},
            {"PUSHH",0xE5},
            {"PUSHPSW",0xF5},
            {"RAC",0xD8},
            {"RAL",0x17},
            {"RAR",0x1F},
            {"RET",0xC9},
            {"RIM",0x20},
            {"RLC",0x7},
            {"RM",0xF8},
            {"RNC",0xD0},
            {"RNZ",0xC0},
            {"RP",0xF0},
            {"RPE",0xE8},
            {"RPO",0xE0},
            {"RRC",0x0F},
            {"RST0",0xC7},
            {"RST1",0xCF},
            {"RST2",0xD7},
            {"RST3",0xDF},
            {"RST4",0xE7},
            {"RST5",0xEF},
            {"RST6",0xF7},
            {"RST7",0xFF},
            {"RZ",0xC8},
            {"SBBA",0x9F},
            {"SBBB",0x98},
            {"SBBC",0x99},
            {"SBBD",0x9A},
            {"SBBE",0x9B},
            {"SBBH",0x9C},
            {"SBBL",0x9D},
            {"SBBM",0x9E},
            {"SBI",0xDE},
            {"SHLD",0x22},
            {"SIM",0x30},
            {"SPHL",0xF9},
            {"STA",0x32},
            {"STAXB",0x2},
            {"STAXD",0x12},
            {"STC",0x37},
            {"SUBA",0x97},
            {"SUBB",0x90},
            {"SUBC",0x91},
            {"SUBD",0x92},
            {"SUBE",0x93},
            {"SUBH",0x94},
            {"SUBL",0x95},
            {"SUBM",0x96},
            {"SUI",0xD6},
            {"XCHG",0xEB},
            {"XRAA",0xAF},
            {"XRAB",0xA8},
            {"XRAC",0xA9},
            {"XRAD",0xAA},
            {"XRAE",0xAB},
            {"XRAH",0xAC},
            {"XRAL",0xAD},
            {"XRAM",0xAE},
            {"XRI",0xEE},
            {"XTHL",0xE3},
            {"IN",0xDB},
            {"OUT",0xD3}
        };
    }
}
