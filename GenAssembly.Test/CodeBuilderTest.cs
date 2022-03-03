using System;
using System.IO;
using GenAssembly.Descripters;
using Xunit;

namespace GenAssembly.Test
{
    public class CodeBuilderTest
    {
        [Fact]
        public void CodeSavePath_Test()
        {
            var codeBuilder = new CodeBuilder("test", "GenAssembly.Test");
            var codeClass = new ClassDescripter("Person", namespaces: "GenAssembly.Test").AddUsing("System");
            codeClass.CreateMember(
		new MethodDescripter("Hello", codeClass)
		.SetAccess(AccessType.Public)
		.SetReturnType("void")
		.AppendCode("Console.WriteLine(\"Hello\");"));
            codeBuilder.CreateClass(codeClass);
            codeBuilder.BuildAsync().Wait();

            Assert.True(File.Exists(Path.Combine(CodeBuilder.CodeCachePath, "GenAssembly/Test/Person.cs")));
        }
    }
}
