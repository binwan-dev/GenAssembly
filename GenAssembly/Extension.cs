using System.Collections.Generic;
using System.Text;
using GenAssembly.Descripters;

namespace GenAssembly
{
    public static class Extension
    {
        
        public static string ToAccessCode(this AccessType access)
        {
            switch (access)
            {
                case AccessType.Public:
                    return "public";
                case AccessType.Private:
                    return "private";
                case AccessType.Protected:
                    return "protected";
                case AccessType.PublicReadonly:
                    return "public readonly";
                case AccessType.PublicStatic:
                    return "public static";
                case AccessType.PublicConst:
                    return "public const";
                case AccessType.PrivateReadonly:
                    return "private readonly";
                case AccessType.PrivateStatic:
                    return "private static";
                case AccessType.PrivateConst:
                    return "private const";
            }
            return "";
        }

        public static string ToParameterCode(this IList<ParameterDescripter> parameters)
        {
            if (parameters == null || parameters.Count == 0) return string.Empty;

            var strCode = new StringBuilder();
            foreach (var param in parameters)
            {
                strCode.Append($"{param.Type} {param.Name},");
            }
            strCode = strCode.Remove(strCode.Length - 1, 1);
            return strCode.ToString();
        }

        public static string ToTypeParamConstraintCode(this IList<TypeParameterDescripter> typeParameters)
        {
            if (typeParameters == null || typeParameters.Count == 0) return string.Empty;

            var strCode = new StringBuilder();
            foreach (var param in typeParameters)
            {
                strCode.Append($"where {param.Name}: {param.Constraint} ");
            }
            strCode = strCode.Remove(strCode.Length - 1, 1);
            return strCode.ToString();
        }

        public static string FindNamespace(this string script)
        {
            if (string.IsNullOrWhiteSpace(script)) return string.Empty;
            var arr = script.Split('\n');
            foreach (var item in arr)
            {
                if (!item.Contains("namespace")) continue;
                return item.Split(' ')[1];
            }
            return string.Empty;
        }
    }
}
