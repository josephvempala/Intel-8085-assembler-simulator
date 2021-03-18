using Microsoft.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace AssemblerSimulator8085
{
    internal partial class Assembler 
    {
        private readonly Regex re = new Regex(@"((?<label>^.+?(?=:)):)?((?<1>ADCA|ADDA|ANAA|CMPA|DCRA|INRA|ORAA|SBBA|SUBA|XRAA|ADCB|ADDB|ANAB|CMPB|DADB|DCRB|DCXB|INRB|INXB|LDAXB|LXIB|ORAB|POPB|PUSHB|SBBB|STAXB|SUBB|XRAB|ADCC|ADDC|ANAC|CMPC|DCRC|INRC|ORAC|SBBC|SUBC|XRAC|ADCD|ADDD|ANAD|CMPD|DADD|DCRD|DCXD|INRD|INXD|LDAXD|LXID|ORAD|POPD|PUSHD|SBBD|STAXD|SUBD|XRAD|ADCE|ADDE|ANAE|CMPE|DCRE|INRE|ORAE|SBBE|SUBE|XRAE|ADCH|ADDH|ANAH|CMPH|DADH|DCRH|DCXH|INRH|INXH|LXIH|ORAH|POPH|PUSHH|SBBH|SUBH|XRAH|ADCL|ADDL|ANAL|CMPL|DCRL|INRL|ORAL|SBBL|SUBL|XRAL|ADCM|ADDM|ANAM|CMPM|DCRM|INRM|ORAM|SBBM|SUBM|XRAM|MOVA,A|MOVA,B|MOVA,C|MOVA,D|MOVA,E|MOVA,H|MOVA,L|MOVA,M|MOVB,A|MOVB,B|MOVB,C|MOVB,D|MOVB,E|MOVB,H|MOVB,L|MOVB,M|MOVC,A|MOVC,B|MOVC,C|MOVC,D|MOVC,E|MOVC,H|MOVC,L|MOVC,M|MOVD,A|MOVD,B|MOVD,C|MOVD,D|MOVD,E|MOVD,H|MOVD,L|MOVD,M|MOVE,A|MOVE,B|MOVE,C|MOVE,D|MOVE,E|MOVE,H|MOVE,L|MOVE,M|MOVH,A|MOVH,B|MOVH,C|MOVH,D|MOVH,E|MOVH,H|MOVH,L|MOVH,M|MOVL,A|MOVL,B|MOVL,C|MOVL,D|MOVL,E|MOVL,H|MOVL,L|MOVL,M|MOVM,A|MOVM,B|MOVM,C|MOVM,D|RST0|RST1|RST2|RST3|RST4|RST5|RST6|RST7|MOVM,E|MOVM,H|MOVM,L|POPPSW|PUSHPSW|DADSP|DCXSP|INXSP|LXISP|STC|CMA|CMC|DAA|DI|EI|HLT|NOP|PCHL|RAC|RAL|RAR|RET|RIM|RLC|RM|RNC|RNZ|RP|RPE|RPO|RRC|RZ|SIM|SPHL|XCHG|XTHL)|((?<2>MVIA,|MVIB,|MVIC,|MVID,|MVIE,|MVIH,|MVIL,|MVIM,|ACI|ADI|ANI|CPI|ORI|SBI|SUI|XRI)((?<hex2>[0-9A-F]{1,2}(?=H))|(?<dec2>[0-9]{1,3}(?=D))|(?<bin2>[0|1]{1,8}(?=B))).)|((?<3>CALL|CC|CM|CNC|CNZ|CP|CPE|CPO|CZ|JC|JM|JMP|JNC|JNZ|JP|JPE|JPO|JZ|LDA|LHLD|SHLD|STA)(((?<hex3>[0-9A-F]{1,4}(?=H))|(?<dec3>[0-9]{1,5}(?=D))|(?<bin3>[0|1]{1,16}(?=B))).|(?<lbl3>.+?$))))(?<extra>.+?$)?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        private List<AssembleError> ErrorsList = new();
        private DictionarySlim<string, ushort> Labels = new();
        private MultiValueDictionary<string, KeyValuePair<ushort, ushort>> to_be_resolved = new(); //MultiValueDictionary<Label Name,KeyValuePair<Substitution Address, Line Number>>
        private byte[] temp_memory = new byte[ushort.MaxValue + 1];
        private ushort line_number;
        private ushort counter;

        private void ResetAssembler()
        {
            ErrorsList = new();
            Labels = new();
            to_be_resolved = new();
            temp_memory = new byte[ushort.MaxValue + 1];
            line_number = 1;
            counter = 0;
        }
        
        private void CheckForUnresolvedLabels()
        {
            if (to_be_resolved.Count is not 0)
            {
                foreach (var label in to_be_resolved)
                    foreach (var line in label.Value)
                        ErrorsList.Add(new AssembleError($"Label \"{label.Key}\"Could not be found", line.Value));
            }
        }

        private void FindAndResolveLabels(Match match)
        {
            if (Labels.ContainsKey(match.Groups["label"].Value))
                ErrorsList.Add(new AssembleError($"Label {match.Groups["label"].Value} was previously defined", line_number));
            else
            {
                Labels.GetOrAddValueRef(match.Groups["label"].Value) = counter;
                if (to_be_resolved.TryGetValue(match.Groups["label"].Value, out var address_line))
                {
                    byte[] address_bytes = BitConverter.GetBytes(counter);
                    foreach (var item in address_line)
                    {
                        temp_memory[item.Key] = address_bytes[0];
                        temp_memory[item.Key + 1] = address_bytes[1];
                    }
                    to_be_resolved.Remove(match.Groups["label"].Value);
                }
            }
        }

        private void Assemble1ByteInstruction(Match match)
        {
            if (counter <= ushort.MaxValue)
            {
                byte inst_value = Instructions[match.Groups["1"].Value];
                temp_memory[counter] = inst_value;
                counter++;
            }
        }

        private void Assemble2ByteInstruction(Match match)
        {
            if (counter <= ushort.MaxValue - 1)
            {
                byte inst_value = Instructions[match.Groups["2"].Value];
                temp_memory[counter] = inst_value;
                counter++;
                if (match.Groups["hex2"].Success)
                    temp_memory[counter] = Convert.ToByte(match.Groups["hex2"].Value, 16);
                else if (match.Groups["dec2"].Success)
                {
                    byte temp = Convert.ToByte(match.Groups["dec2"].Value);
                    temp_memory[counter] = temp;
                }
                else if (match.Groups["bin2"].Success)
                    temp_memory[counter] = Convert.ToByte(match.Groups["bin2"].Value, 2);
                else if (match.Groups["lbl3"].Success)
                    ErrorsList.Add(new AssembleError($"Expected valid 8bit number in bin hex or dec found \"{match.Groups["lbl3"]}\" instead", line_number));
                counter++;
            }
            else
                ErrorsList.Add(new AssembleError($"Not enough memory to add 2-byte instruction {match.Groups["2"].Value} to memory address {counter.ToString("X")}", line_number));
        }

        private void Assemble3ByteInstruction(Match match)
        {
            if (counter <= ushort.MaxValue - 2)
            {
                byte inst_value = Instructions[match.Groups["3"].Value];
                temp_memory[counter] = inst_value;
                counter++;
                byte[] address = new byte[2];
                if (match.Groups["hex3"].Success)
                    address = BitConverter.GetBytes(Convert.ToUInt16(match.Groups["hex3"].Value, 16));
                else if (match.Groups["dec3"].Success)
                {
                    ushort temp = Convert.ToUInt16(match.Groups["dec3"].Value);
                    address = BitConverter.GetBytes(temp);
                }
                else if (match.Groups["bin3"].Success)
                    address = BitConverter.GetBytes(Convert.ToUInt16(match.Groups["bin3"].Value, 2));
                else if (match.Groups["lbl3"].Success)
                {
                    if (Labels.TryGetValue(match.Groups["lbl3"].Value, out ushort label_address))
                        address = BitConverter.GetBytes(label_address);
                    else
                        to_be_resolved.Add(match.Groups["lbl3"].Value, new KeyValuePair<ushort, ushort>(counter, line_number));
                }
                temp_memory[counter] = address[0];
                counter++;
                temp_memory[counter] = address[1];
                counter++;
            }
            else
                ErrorsList.Add(new AssembleError($"Not enough memory to add 3-byte instruction {match.Groups["2"].Value} to memory address {counter.ToString("X")}", line_number));
        }
        
        public List<AssembleError> Assemble(string source, out byte[] Memory, ushort load_at = 0) //Load into memory default = 0
        {
            ResetAssembler();
            counter = load_at;
            string[] processed_input = Regex.Replace(source, @"( +?)|(;.*?$)", "", RegexOptions.IgnoreCase | RegexOptions.Multiline).ToUpper().Split("\n");
            foreach (var line in processed_input)
            {
                var match = re.Match(line);
                if (match is null)
                {
                    if (line is not "")
                        ErrorsList.Add(new AssembleError($"Invalid opcode/operand at", line_number));
                    line_number++;
                    continue;
                }
                if (match.Groups["label"].Success)
                    FindAndResolveLabels(match);
                if (match.Groups["1"].Success)
                    Assemble1ByteInstruction(match);
                else if (match.Groups["2"].Success)
                    Assemble2ByteInstruction(match);
                else if (match.Groups["3"].Success)
                    Assemble3ByteInstruction(match);
                if (match.Groups["extra"].Success)
                    ErrorsList.Add(new AssembleError($"Expected end of instruction, found {match.Groups["extra"].Value}", line_number));
                line_number++;
            }
            CheckForUnresolvedLabels();
            if (ErrorsList.Count is 0)
                Memory = temp_memory;
            else
                Memory = null;
            return ErrorsList;
        }
    }
}
