using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.APITesting
{
    /// <summary>
    /// Contains the necessary information to save and retrieve lists of APITets to database.
    /// </summary>
    public class APITestCollection
    {
        /// <summary>
        /// Integer Id identifying the collection.
        /// </summary>
        /// <remarks>
        public int Id { get; set; }

        /// <summary>
        /// Current title for the Id
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The list of tests in the order they are to be executed.
        /// </summary>
        public List<APITest> Tests { get; set;}

        /// <summary>
        /// The DateTime the Tests in the collection were run.
        /// </summary>
        public DateTime TestDateTime { get; set; }


        public APITestCollection(int id, string title, List<APITest> tests)
        {
            Id = id;
            Title = title;
            Tests = tests;
        }

    }
}
