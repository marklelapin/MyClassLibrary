using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.LocalServerMethods.Extensions;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Tests methods that write to Database by writing to the test Database and then getting values back.
    /// </summary>
    /// <remarks>
    /// In order for these to work effectively the corresponding ILocalDataAccessGetTest need to by passed first.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface ISaveAndGetUpdateTypeTests<T> where T : ILocalServerModelUpdate
    {
        /// <summary>
        /// Saves update to local storage and then gets them back. Checks that output is same as input.
        /// </summary>
        /// <remarks>
        /// Should be setup for each new UpdateType created.
        /// </remarls>
        public Task SaveAndGetLocalTest();


        /// <summary>
        /// Saves update to server storage and then gets them back. Checks that output is same as input.
        /// </summary>
        /// <remarks>
        /// Should be setup for each new UpdateType created.
        /// </remarls>
        public Task SaveAndGetServerTest();





    }
}
