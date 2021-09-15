using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenAssembly.Descripters
{
    public class AttributeDescripter
    {
        public AttributeDescripter(string name, params string[] parameters)
        {
            Name = name;
            Parameters = parameters.ToList();
        }

        public string Name { get; }

        public IList<string> Parameters { get; }

        public override string ToString()
        {
            var code=new StringBuilder();
            code.Append($"[{Name}");

            if(Parameters.Count==0)
            {
                code.Append("]");
                return code.ToString();
            }

            code.Append("(");
            foreach(var parameter in Parameters)
                code.Append($"{parameter}, ");
            code.Remove(code.Length-1, 1);
            code.Append(")]");
            
            return code.ToString(); 
        }
    }
}
