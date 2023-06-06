using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Tests methods that write to Database by writing to the test Database and then getting values back.
    /// </summary>
    /// <remarks>
    /// In order for these to work effectively the corresponding ILocalDataAccessGetTest need to by passed first.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface ISaveAndGetUpdateToLocalTests<T> where T : LocalServerModelUpdate
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
