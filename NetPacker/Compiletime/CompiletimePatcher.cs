using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace NetPacker.Compiletime
{
    public static class CompiletimePatcher
    {
        public static TypeDef ListType = ModuleDefMD.Load(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll").Types.Single(x => x.Name == "List`1");

        public static void PackMethod(MethodDef target)
        {

        }

        public static void CreateMethodRestore(MethodDef target, MethodDef newMethod, ModuleDefMD module)
        {
            AssemblyRef dnlib = module.GetAssemblyRef(new UTF8String("dnlib"));
            TypeRefUser Instruction = new TypeRefUser(module, new UTF8String("dnlib"), new UTF8String("Instruction"), dnlib);
            TypeSig instructionSig = Instruction.ToTypeSig();
            
            var assemblyRef = module.CorLibTypes.AssemblyRef;
    
            var listRef = new TypeRefUser(module, @"System.Collections.Generic", "List`1", assemblyRef);
            var listGenericInstSig = new GenericInstSig(new ClassSig(listRef), instructionSig);

            var listTypeSpec = new TypeSpecUser(listGenericInstSig);

            var listCtor = new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), listTypeSpec);
            var instruictionCtor = new MemberRefUser(module, ".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void), instructionSig.TryGetTypeSpec());

            var listAdd = new MemberRefUser(module, "Add", MethodSig.CreateInstance(module.CorLibTypes.Void, new GenericVar(0)), listTypeSpec);

            // sdsd

            newMethod.Body.Instructions.Add(OpCodes.Newobj.ToInstruction(listCtor));
            newMethod.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction()); // Store list to local[0]
            /*
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Dup));
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Ldsfld, OpCodes.Add));
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Ldc_I4_S, 0x37));
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Box, module.CorLibTypes.Int32));
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Newobj, instruictionCtor));
            newMethod.Body.Instructions.Add(new Instruction(OpCodes.Callvirt, listAdd));
            */
        }

        public static void CreateJunkCode()
        {

        }
    }
}
