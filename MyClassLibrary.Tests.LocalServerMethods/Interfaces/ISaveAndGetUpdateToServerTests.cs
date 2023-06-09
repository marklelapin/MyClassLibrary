
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface IServerDataAccessPostTests<T> where T : LocalServerModelUpdate
    {
        /// <summary>
        /// Saves and then gets Updates to Local Storage and checks that output is same as input.
        /// </summary>
        /// <remarls>
        /// Should be setup for each new UpdateType created.
        /// </remarls>
        public Task SaveAndGetUpdatesTest(List<T> testUpdates, List<T> expected);



    }
}
