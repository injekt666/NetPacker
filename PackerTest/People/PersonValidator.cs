using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackerTest.People
{
    class PersonValidator
    {
        public static bool Validate(Person person)
        {
            if(String.IsNullOrEmpty(person.firstName))
            {
                Console.WriteLine("Error: No valid first name. ");
                return false;
            }

            if(String.IsNullOrEmpty(person.lastName))
            {
                Console.WriteLine("Error: No valid last name. ");
                return false;
            }

            return true;
        }

        public static bool ValidatePerson(int one, int two, int three, int four, int five, int six)
        {
            return true;
        }

        public static bool CallPerson(int one, int two, int three, int four, int five, int six)
        {
            return ValidatePerson(one, two, three, four, five, six);
        }

        public static void DetourExample()
        {
            Console.WriteLine("SUCCESSFUL METHOD DETOUR");
        }

        public static void EmptyMethod()
        {

        }
    }
}
