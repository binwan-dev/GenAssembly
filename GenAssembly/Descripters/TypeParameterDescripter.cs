using System;
using System.Collections.Generic;
using System.Text;

namespace GenAssembly.Descripters
{
    public struct TypeParameterDescripter
    {
        public TypeParameterDescripter(string name, string constraint)
        {
            Name = name;
            Constraint = constraint;
        }

        public string Name { get; set; }

        public string Constraint { get; set; }
    }
}
