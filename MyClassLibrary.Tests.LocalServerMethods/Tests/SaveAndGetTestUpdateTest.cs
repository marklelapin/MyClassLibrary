using MyClassLibrary.Tests.LocalServerMethods.Interfaces;


namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class SaveAndGetTestUpdateTest : ISaveAndGetUpdateTypeTests<TestUpdate>
    {
        private readonly ISaveAndGetUpdateTypeTests<TestUpdate> _saveAndGetTestProvider;

        public SaveAndGetTestUpdateTest(ISaveAndGetUpdateTypeTests<TestUpdate> saveAndGetTestProvider)
        {

           _saveAndGetTestProvider = saveAndGetTestProvider;
        }

        [Fact]
        public async Task SaveAndGetLocalTest()
        {
            await _saveAndGetTestProvider.SaveAndGetLocalTest();
        }

        [Fact]
        public async Task SaveAndGetServerTest()
        {
            await _saveAndGetTestProvider.SaveAndGetServerTest();
        }

    }
}
