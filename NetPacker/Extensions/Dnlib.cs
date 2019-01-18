using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPacker.Extensions
{
    public static class Dnlib
    {
        public static List<Instruction> FindAllCallInstructions(this MethodDef method)
        {
            return method.Body.Instructions.Where(x => x.OpCode == OpCodes.Call || x.OpCode == OpCodes.Callvirt).ToList();
        }

        public static List<MethodDef> FindAllCallsToThisMethod(this MethodDef method, List<ModuleDefMD> assemblies)
        {
            List<MethodDef> methodCallers = new List<MethodDef>();
            foreach(var assembly in assemblies)
            {
                foreach(var type in assembly.Types)
                {
                    foreach(var classMethod in type.Methods)
                    {
                        try
                        {
                            var callInstructions = classMethod.FindAllCallInstructions();
                            var targetInstructions = callInstructions.Where(x => (MethodDef)x.Operand == method);
                            if(callInstructions.Count() > 0)
                            {
                                methodCallers.Add(classMethod);
                            }
                        }

                        catch(Exception ex)
                        {

                        }
                        
                    }
                }
            }

            return methodCallers;
        }

        public static List<Instruction> FindCallInstructionsToMethod(this MethodDef method, MethodDef target)
        {
            List<Instruction> callers = new List<Instruction>();

            foreach(var instruction in method.Body.Instructions)
            {
                try
                {
                    if((MethodDef)instruction.Operand == target)
                    {
                        callers.Add(instruction);
                    }
                }

                catch
                {
                    
                }
            }

            return callers;
        }
    }
}
