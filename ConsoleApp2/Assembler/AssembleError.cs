
namespace AssemblerSimulator8085.Assembler
{
    public class AssembleError
    {
        public string error { get; set; }
        public int line_no { get; set; }

        public AssembleError(string error_name, int line_number) => (error, line_no) = (error_name, line_number);
    }
}
