using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPacker.Detours
{
    public static class DetourHelper
    {
        public static void CompiletimeDetour(MethodDef target, MethodDef methodToCall, int insertIndex = 0, List<Instruction> preCallInstructions = null)
        {
            if(preCallInstructions == null)
            {
                target.Body.Instructions.Insert(insertIndex, new Instruction(OpCodes.Call, methodToCall));
            }
        }
    }
}
