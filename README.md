# GenAssembly
Generate assembly for csharp  
## Install  
```
dotnet add package GenAssembly 
```
## Used
Interface mapping is recommended for access and used ioc container

There is simple code. used reflection to call generate assembly
```csharp
var codeAssembly = await CodeBuilder.Default.CreateClass(
                new ClassDescripter("Person").CreateMember(
                    new MethodDescripter("Hello")
                    .SetParams(new ParameterDescripter("string", "name"))
                    .SetCode(@"
                        Console.WriteLine($""Hello {name}"");
                    ")
                )
            ).BuildAsync();

// Call            
var obj = codeAssembly.GetObj("Person");
var method = Type.GetType("Person").GetMethod("Hello", new Type[] { typeof(string) });
method.Invoke(obj, new object[] { "world" });
```
