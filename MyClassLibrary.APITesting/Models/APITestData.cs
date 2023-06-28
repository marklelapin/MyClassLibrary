using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.APITesting.Models
{
    public class APITestData
    {
        /// <summary>
        /// Integer Id identifying the collection.
        /// </summary>
        /// <remarks>
        public int CollectionId { get; set; }

        /// <summary>
        /// The Collection Title
        /// </summary>
        public string CollectionTitle { get; set; }

        /// <summary>
        /// Integer Id identifying the test.
        /// </summary>
        /// <remarks>
        public int TestId { get; set; }

        /// <summary>
        /// Current title for the test.
        /// </summary>
        public string TestTitle { get; set; }

        /// <summary>
        /// The DateTime the Tests in the collection were run.
        /// </summary>
        public DateTime TestDateTime { get; set; }

        /// <summary>
        /// Whether or not the test was successfull.
        /// </summary>
        public bool WasSuccessful { get; set; }

        /// <summary>
        /// Message summarising briefly what failed if the test wasn't successful.
        /// </summary>
        public string? FailureMessage { get; set; } = "";

        /// <summary>
        /// The result expected from the test.
        /// </summary>
        public string? ExpectedResult { get; set; } = "";

        /// <summary>
        /// The actual result from the test.
        /// </summary>
        public string? ActualResult { get; set; } = "";


        public APITestData(int collectionId,string collectionTitle,int testId,string testTitle,DateTime testDateTime,bool wasSuccessful,string? failureMessage = null,string? expectedResult = null,string? actualResult = null)
        {        
            CollectionId = collectionId;
            CollectionTitle = collectionTitle;  
            TestId = testId;
            TestTitle = testTitle;
            TestDateTime = testDateTime;
            WasSuccessful = wasSuccessful;
            FailureMessage = failureMessage ?? "";
            ExpectedResult = expectedResult ?? "";
            ActualResult = actualResult ?? "";
        }

    }
}
