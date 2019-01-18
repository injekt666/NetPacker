using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackerTest.People
{
    class Person
    {
        public string firstName = String.Empty;

        public string lastName = string.Empty;

        public bool IsValid { get { return PersonValidator.Validate(this); } }

        public Person(string _firstName, string _lastName)
        {
            firstName = _firstName;
            lastName = _lastName;
        }

        public void SetFirstName(string name)
        {
            firstName = name;
        }

        public void SetLastName(string name)
        {
            lastName = name;
        }

    }
}
