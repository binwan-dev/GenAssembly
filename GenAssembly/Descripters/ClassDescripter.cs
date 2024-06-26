﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Text;

namespace GenAssembly.Descripters
{
    public class ClassDescripter
    {
        private readonly string _code;

        public ClassDescripter(string name, string code, string namespaces = CodeBuilder.DefaultNamespace)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("The class name is not to be null!");
            if (string.IsNullOrWhiteSpace(namespaces)) throw new ArgumentNullException("The class namespace is not to be null!");

            Name = name;
            Namespace = namespaces;

            _code = code;

            DecodeCode(code);
        }

        public ClassDescripter(string name, string namespaces = CodeBuilder.DefaultNamespace)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("The class name is not to be null!");
            if (string.IsNullOrWhiteSpace(namespaces)) throw new ArgumentNullException("The class namespace is not to be null!");

            Name = name;
            Namespace = namespaces;

            Methods = new List<MethodDescripter>();
            Properties = new List<PropertyDescripter>();
            Fields = new List<FieldDescripter>();
            UsingNamespaces = new List<string>();
            Constructors = new List<ConstructorDescripter>();

            AddUsing("System.Linq");
            AddUsing("System");
            AddUsing("System.Collections.Generic");
        }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public string[] BaseTypes { get; private set; }

        public bool IsRecord {get;private set;}

        public List<ConstructorDescripter> Constructors { get; private set; }

        public List<FieldDescripter> Fields { get; private set; }

        public List<PropertyDescripter> Properties { get; private set; }

        public AccessType Access { get; private set; }

        public List<MethodDescripter> Methods { get; private set; }

        public List<string> UsingNamespaces { get; private set; }

        public ClassDescripter SetBaseType(params string[] baseTypes)
        {
            BaseTypes = baseTypes;
            return this;
        }

        public ClassDescripter CreateConstructor(params ConstructorDescripter[] constructors)
        {
            Constructors.AddRange(constructors);
            return this;
        }

        public ClassDescripter CreateMember(params MethodDescripter[] methodDescripters)
        {
            Methods.AddRange(methodDescripters);
            return this;
        }

        public ClassDescripter CreateFiled(params FieldDescripter[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public ClassDescripter CreateProperty(params PropertyDescripter[] properties)
        {
            Properties.AddRange(properties);
            return this;
        }

        public ClassDescripter AddUsing(params string[] usingNamespaces)
        {
            if (UsingNamespaces == null)
                UsingNamespaces = new List<string>();
            foreach (var item in usingNamespaces)
            {
                if (UsingNamespaces.Contains(item)) continue;
                UsingNamespaces.Add(item);
            }
            return this;
        }

        public ClassDescripter SetAccess(AccessType access)
        {
            Access = access;
            return this;
        }

        public ClassDescripter SetRecord()
        {
            IsRecord = true;
            return this;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_code)) return _code;

            var classStr = new StringBuilder();
            if (UsingNamespaces != null && UsingNamespaces.Count > 0)
            {
                foreach (var item in UsingNamespaces)
                {
                    if (item.Contains("using "))
                        classStr.AppendLine($"{item}");
                    else
                        classStr.AppendLine($"using {item};");
                }
                classStr.AppendLine();
            }
            classStr.AppendLine($"namespace {Namespace}");
            classStr.AppendLine("{");
            var classType = IsRecord ? "record" : "class";
            classStr.Append($"    {Access.ToAccessCode()} {classType} {Name}");
            if (BaseTypes != null && BaseTypes.Length > 0)
            {
                classStr.Append(" : ");
                foreach (var baseType in BaseTypes)
                {
                    classStr.Append($"{baseType}, ");
                }
                classStr = classStr.Remove(classStr.Length - 2, 2);
            }
            classStr.AppendLine();
            classStr.AppendLine("    {");
            foreach (var item in Fields) classStr.AppendLine(item.ToString());
            foreach (var item in Constructors) classStr.AppendLine(item.ToString());
            foreach (var item in Properties) classStr.AppendLine(item.ToString());
            foreach (var item in Methods) classStr.AppendLine(item.ToString());
            classStr.AppendLine("    }");
            classStr.AppendLine("}");

            return classStr.ToString();
        }

        private void DecodeCode(string code)
        {
            var nameIsDecoded = false;
            foreach (var item in code.Split('\n'))
            {
                if (item.Contains("namespace "))
                {
                    Namespace = item.Replace("namespace", "")
                        .Replace("}", "")
                        .Trim();
                }
                if (!nameIsDecoded && item.Contains(" class "))
                {
                    var strArr = item.Split(':');
                    Name = Regex.Split(strArr[0], " class ")[1].Trim();
                    if (strArr.Length > 1)
                    {
                        strArr = Regex.Split(strArr[1], "where")[0].Trim().Split(',');
                        BaseTypes = strArr;
                    }
                    nameIsDecoded = true;
                }
                if (item.Contains("using "))
                {
                    AddUsing(item);
                }
            }
        }
    }

    public struct CodeParameter
    {
        public CodeParameter(string type, string name)
        {
            this.Type = type;
            this.Name = name;

        }
        public string Type { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name}";
        }
    }

    public struct CodeMemberAttribute
    {
        public CodeMemberAttribute(params string[] names)
        {
            this.Names = names;

        }
        public IList<string> Names { get; set; }

        public override string ToString()
        {
            if (Names == null || Names.Count == 0) return "private";
            string nameStr = string.Empty;
            foreach (var item in Names) nameStr += item + " ";
            return nameStr;
        }
    }
}
