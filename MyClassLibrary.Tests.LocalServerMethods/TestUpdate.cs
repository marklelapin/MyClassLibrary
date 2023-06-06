
using System.Text.Json.Serialization;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class TestUpdate : LocalServerModelUpdate
    {
        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public DateTime? FavouriteDate { get; set; }
        
        public List<string>? FavouriteFoods { get; set; }

        public bool IsCool { get; set; }
    
        public TestUpdate() :base()
        {

        }

        public TestUpdate(Guid id) : base(id) { }

        public TestUpdate(Guid id,string firstName, string lastName, DateTime? favouriteDate, List<string>? favouriteFoods, bool isCool) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteFoods = favouriteFoods;
            IsCool = isCool;    
        }

        [JsonConstructor]
        public TestUpdate(Guid id, DateTime created,string createdBy,DateTime? updatedOnServer,bool isConflicted, bool isActive, string? firstName, string? lastName, DateTime? favouriteDate, List<string>? favouriteFoods,bool isCool): base(id)
        {
            Id = id;
            Created = created;
            CreatedBy = createdBy;
            UpdatedOnServer = updatedOnServer;
            IsConflicted = isConflicted;
            IsActive = isActive;
            FirstName = firstName;
            LastName = lastName;
            FavouriteDate = favouriteDate;
            FavouriteFoods = favouriteFoods;
            IsCool = isCool;
        }

 

    }
}
