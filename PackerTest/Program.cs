using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetPacker;
using PackerTest.People;
using dnlib.DotNet;

namespace PackerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            NetPackerTest.Start();

            Console.WriteLine("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter Last Name: ");
            string lastName = Console.ReadLine();

            var person = new Person(firstName, lastName);
            bool isValid = PersonValidator.Validate(person);

            if(isValid)
            {
                Console.WriteLine(String.Format("{0} {1} is valid name", firstName, lastName));
            }

            Console.ReadLine();
        }
    }
}
