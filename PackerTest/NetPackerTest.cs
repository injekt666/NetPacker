using dnlib.DotNet;
using NetPacker.Compiletime;
using NetPacker.Detours;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetPacker.Extensions;
namespace PackerTest
{
    public static class NetPackerTest
    {
        public static void Start()
        {

            
            ModuleDefMD sampleCode = ModuleDefMD.Load(@"C:\Users\colton\source\repos\PackerTest\PackerTest\bin\Debug\PackerTest.exe");
            //         ModuleDefMD thisAssembly = ModuleDefMD.Load(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var program = sampleCode.Types.Single(x => x.Name == "Program").Methods.Single(x => x.Name == "Main");
            var validate = sampleCode.Types.Single(x => x.Name == "PersonValidator").Methods.Single(x => x.Name == "Validate");
            var callInstructions = program.FindAllCallInstructions();
            foreach(var instruction in callInstructions)
            {
                try
                {
                    if ((MethodDef)instruction.Operand == validate)
                    {
                        Console.WriteLine(instruction.ToString());
                    }
                }

                catch(Exception ex)
                {
                //    Console.WriteLine(instruction.ToString() + "DID NOT WORK!! EXCEPTION - " + ex.Message);
                }
            }
           

            CompiletimeObfuscater.ObfuscateMethodCalls(validate, 30, new List<ModuleDefMD> { sampleCode });
      //      validate.GetCalls(new List<ModuleDefMD>() { sampleCode});
            var detourSource = sampleCode.Types.Single(x => x.Name == "PersonValidator").Methods.Single(x => x.Name == "DetourExample");
            var emptyMethod = sampleCode.Types.Single(x => x.Name == "PersonValidator").Methods.Single(x => x.Name == "EmptyMethod");

            DetourHelper.CompiletimeDetour(validate, detourSource);

            CompiletimePatcher.CreateMethodRestore(validate, detourSource, sampleCode);

            if (Process.GetProcessesByName("Obfuscated").Count() == 0)
            {
                Console.WriteLine("Saving");
                sampleCode.Write(@"C:\Users\colton\source\repos\PackerTest\PackerTest\bin\Debug\Obfuscated.exe");
                Console.WriteLine("Finsihed saving");
            }


            Console.WriteLine("NetPackerTest.Start() is over");
        }
    }
}
