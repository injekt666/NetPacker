using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetPacker.Extensions;
namespace NetPacker.Compiletime
{
    public static class CompiletimeObfuscater
    {
        public static void ObfuscateMethodCalls(MethodDef target, int callLayers, List<ModuleDefMD> modules)
        {
            var callers = target.FindAllCallsToThisMethod(modules); // THIS HAS TO STAY AT THE TOP OF THE METHOD BECAUSE OTHERWISE IT WILL GRAB OBFUSCATOR GENERATED METHODS AND CAUSE A STACK OVERFLOW

            MethodDef lastTarget = target;
            for(int i = 0; i < callLayers; i++)
            {
                var callerMethod = CreateMethodCaller(lastTarget, "CALLERLAYER" + i.ToString());
                lastTarget = callerMethod;
            }

            ReplaceCallsToMethod(target, lastTarget, callers, modules);
        }

        public static void ReplaceCallsToMethod(MethodDef target, MethodDef replacementMethod, List<MethodDef> callers, List<ModuleDefMD> modules)
        {
 
            foreach (var caller in callers)
            {
                var callInstructions = caller.FindCallInstructionsToMethod(target);
                
                foreach (var callInstruction in callInstructions)
                {
                    callInstruction.Operand = replacementMethod;
                }
            }
        }

        public static MethodDef CreateMethodCaller(MethodDef method, string name, TypeDef classToSaveMethod = null)
        {
 
            MethodDefUser newMethod = null;
            if(String.IsNullOrEmpty(name))
            {
                throw new Exception("Name cannot be an empty string");
            }

            if(method.Parameters.Count > 4)
            {
                throw new Exception("Could not call CreateMethodCaller on [0}. Methods with more than 4 arguments are currently not supported.");
            }

            if(method.Parameters.Count > 0)
            {
                foreach(var param in method.Parameters)
                {
                    if(param.Type == null)
                    {
                        throw new Exception(String.Format("Failed to find type of Parameter {0} in method {1}. Parameter type name is {2}", param.Name, method.Name, param.Type.GetName()));
                    }
                }
            }
            if (method.IsStatic)
            {
                if (method.Parameters.Count > 0)
                {
                    newMethod = new MethodDefUser(name, MethodSig.CreateStatic(method.ReturnType, method.Parameters.Select(x => x.Type).ToArray()), method.ImplAttributes, method.Attributes);
                }

                else
                {
                    newMethod = new MethodDefUser(name, MethodSig.CreateStatic(method.ReturnType), method.ImplAttributes, method.Attributes);
                }
            }

            else
            {
                if (method.Parameters.Count > 0)
                {
                    newMethod = new MethodDefUser(name, MethodSig.CreateInstance(method.ReturnType, method.Parameters.Select(x => x.Type).ToArray()), method.ImplAttributes, method.Attributes);
                }

                else
                {
                    newMethod = new MethodDefUser(name, MethodSig.CreateInstance(method.ReturnType), method.ImplAttributes, method.Attributes);
                }
            }



            newMethod.Body = new CilBody();

            if (method.Parameters.Count >= 1) {
                newMethod.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            }

            if (method.Parameters.Count >= 2) {
                newMethod.Body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
            }

            if (method.Parameters.Count >= 3) {
                newMethod.Body.Instructions.Add(OpCodes.Ldarg_2.ToInstruction());
            }

            if (method.Parameters.Count >= 4) {
                newMethod.Body.Instructions.Add(OpCodes.Ldarg_3.ToInstruction());
            }
            /*
            if(method.Parameters.Count >= 5)
            {
                for(int i = 4; i < method.Parameters.Count - 1; i++)
                {
                    newMethod.Body.Instructions.Add(new Instruction(OpCodes.Ldarg_S, i));
                }
            }
            */
            if (method.IsVirtual)
            {
                newMethod.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(method));
            }

            else
            {
                newMethod.Body.Instructions.Add(OpCodes.Call.ToInstruction(method));
            }

            if(newMethod.ReturnType.GetName() != "Void")
            {
                newMethod.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            }

            if(classToSaveMethod != null)
            {
                classToSaveMethod.Methods.Add(newMethod.ResolveMethodDef());
            }

            else
            {
                method.DeclaringType.Methods.Add(newMethod.ResolveMethodDef());
            }

            return newMethod.ResolveMethodDef();
        }
    }
}
