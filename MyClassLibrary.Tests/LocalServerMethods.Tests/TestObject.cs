using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    internal class TestObject : LocalServerIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime FavouriteDate { get; set; }

        public int FavouriteInteger { get; set; }

        public List<string> FavouriteFoods { get; set; }


        public TestObject(string firstName, string lastName, DateTime favouriteDate, int favouriteInteger, List<string> favouriteFoods)
        {
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteInteger = favouriteInteger;
            FavouriteFoods = favouriteFoods;
        }


    }
}
