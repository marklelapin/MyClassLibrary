using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class TestObject : LocalServerIdentityUpdate
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime? FavouriteDate { get; set; }

        public List<string>? FavouriteFoods { get; set; }
    
        public TestObject() { }

        public TestObject(string firstName, string lastName, DateTime favouriteDate, List<string> favouriteFoods)
        {
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteFoods = favouriteFoods;
        }
        public TestObject(Guid id,DateTime created,string createdBy,DateTime? updatedOnServer,bool isActive, string firstName, string lastName, DateTime favouriteDate, List<string> favouriteFoods)
        {
            Id = id;
            Created = created;
            CreatedBy = createdBy;
            UpdatedOnServer = updatedOnServer;
            IsActive = isActive;
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteFoods = favouriteFoods;
        }

 

    }
}
