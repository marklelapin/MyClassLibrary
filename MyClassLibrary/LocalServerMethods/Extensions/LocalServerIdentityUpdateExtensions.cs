using MyClassLibrary.ErrorHandling;
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.LocalServerMethods.Extensions
{
    public static class Extensions
    {

        public static string GetUpdateType<T>(this T obj) where T : LocalServerModelUpdate
        {
            string output;

            output = typeof(T).Name;

            if (!output.Contains("Update"))
            {
                throw new IdentifiedException("The given LocalServerIdentityUpdate must follow the convention of class name in the form {type}Update.");
            }
            return output.Replace("Update", "");
        }
    }
}
