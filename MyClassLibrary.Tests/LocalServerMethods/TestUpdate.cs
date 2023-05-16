using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class TestUpdate : LocalServerIdentityUpdate
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime? FavouriteDate { get; set; }

        public List<string>? FavouriteFoods { get; set; }
    
        public TestUpdate(Guid id) : base(id) { }

        public TestUpdate(Guid id,string firstName, string lastName, DateTime favouriteDate, List<string> favouriteFoods) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteFoods = favouriteFoods;
        }

        [JsonConstructor]
        public TestUpdate(Guid id,DateTime created,string createdBy,DateTime? updatedOnServer,bool isActive, string? firstName, string? lastName, DateTime? favouriteDate, List<string> favouriteFoods) : base(id)
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
