using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NetPacker.SourceMethods
{
    /// <summary>
    /// A temporary class that has stores static methods related to harmony so I can call them inside of other methods.
    /// </summary>
    class HarmonySources
    {
        private static IEnumerable<CodeInstruction> ReplaceInstructions(ILGenerator ilg, IEnumerable<CodeInstruction> instructions, List<CodeInstruction> newInstructions)
        {
            return newInstructions;
        }

        private static IEnumerable<CodeInstruction> AppendInstructions(ILGenerator ilg, IEnumerable<CodeInstruction> instructions, List<CodeInstruction> newInstructions)
        {
            var originalInstructions = new List<CodeInstruction>(instructions);
            originalInstructions.AddRange(newInstructions);
            return originalInstructions;
        }
    }
}
