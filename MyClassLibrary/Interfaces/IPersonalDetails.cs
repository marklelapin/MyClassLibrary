using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtensions.Interfaces
{
    public interface IPersonalDetails    {
        
        string Title { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string PhoneNumber { get; }
        string Address { get; }
        string PostalCode { get; }
        DateTime DateOfBirth { get; }

    }
}
