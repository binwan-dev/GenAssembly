using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GenAssembly.Descripters
{
    public class MethodDescripter
    {
        private StringBuilder _codeBuilder;
        private string _code;
        private readonly ClassDescripter _class;

        public MethodDescripter(string name, ClassDescripter @class, bool isAsync = false)
        {
            Name = name;
            IsAsync = isAsync;
            ReturnTypeStr = "void";
            Access = AccessType.Public;
            _class = @class;

            RefenceTypes=new List<Type>();
            Parameters=new List<ParameterDescripter>();
            Attributes=new List<AttributeDescripter>();
            TypeParameters=new List<TypeParameterDescripter>();
        }

        public string Name { get; set; }

        public AccessType Access { get; set; }

        public IList<Type> RefenceTypes { get; }

        public string ReturnTypeStr { get; set; }

        public IList<ParameterDescripter> Parameters { get; }

        public List<AttributeDescripter> Attributes { get; }

        public string Code
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_code))
                    _code = _codeBuilder.ToString();
                return _code;
            }
        }

        public bool IsAsync { get; set; }

        public IList<TypeParameterDescripter> TypeParameters { get; }

        public MethodDescripter SetAccess(AccessType access)
        {
            Access = access;
            return this;
        }

        public MethodDescripter SetReturnType(string returnTypeStr)
        {
            ReturnTypeStr=returnTypeStr;
            return this;
        }
        
        public MethodDescripter SetReturnType(Type returnType)
        {
            ReturnTypeStr=returnType.Name;
            return this;
        }

        public MethodDescripter AppendCode(string code)
        {
            if (_codeBuilder == null)
            {
                _codeBuilder = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Code))
                    _codeBuilder.Append(Code);
            }
            _codeBuilder.AppendLine(code);
            if (!String.IsNullOrWhiteSpace(Code))
                _code = string.Empty;
            return this;
        }

        public override string ToString()
        {
            var strCode = new StringBuilder();

            strCode.Append(ToAttributes());
            strCode.Append($"        {Access.ToAccessCode()} ");
            if (IsAsync) strCode.Append("async ");
            strCode.AppendLine($"{ReturnTypeStr} {Name}({Parameters.ToParameterCode()}){TypeParameters.ToTypeParamConstraintCode()}");
            strCode.AppendLine("        {");
            strCode.AppendLine($"            {Code}");
            strCode.AppendLine("        }");

            return strCode.ToString();
        }

        private string ToAttributes()
        {
            var strCode=new StringBuilder("");
            foreach(var attribute in Attributes)
            {
                strCode.AppendLine($"        {attribute.ToString()}");
            }
            return strCode.ToString();
        }
    }
}
