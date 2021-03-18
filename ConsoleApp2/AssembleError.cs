﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerSimulator8085
{
    public class AssembleError
    {
        public string error { get; set; }
        public ushort line_no { get; set; }
        public AssembleError(string error_name, ushort line_number) => (error, line_no) = (error_name, line_number);
    }
}
