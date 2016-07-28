////////////////////////////////////////////////////////////////////////////////
///自定义迭代器
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;

namespace TestConsoleApplication
{
    class CustomIterator
    {
        public void Run()
        {
            Person[] person = new Person[3] { new Person("A", "a"), new Person("B", "b"), new Person("C", "c") };
            People people = new People(person);
            foreach (Person p in people)
            {
                Console.WriteLine(p.firstName + "·" + p.lastName);
            }
        }

    }

    public class Person
    {
        public Person(string fName, string lName)
        {
            this.firstName = fName;
            this.lastName = lName;
        }
        public string firstName;
        public string lastName;
    }

    public class People : IEnumerable
    {
        private Person[] _people;
        public People(Person[] pArray)
        {
            _people = pArray;
        }
        //public IEnumerator GetEnumerator()
        //{
        //    return new PeopleEnum(_people);
        //}

        public IEnumerator GetEnumerator()
        {
            //return new PeopleEnum(_people);  

            for (int i = 0; i < _people.Length; i++)
            {
                yield return _people[i];
            }
        }
    }

    public class PeopleEnum : IEnumerator
    {
        public Person[] _people;
        int position = -1;

        public PeopleEnum(Person[] list)
        {
            _people = list;
        }
        public bool MoveNext()
        {
            position++;
            return (position < _people.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _people[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
