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

        public static List<T> SortByCreated<T>(this List<T> list) where T : LocalServerModelUpdate
        {
            return list.OrderByDescending(x => x.Created).ToList();
        }

        public static List<LocalToServerPostBack> SortByCreated(this List<LocalToServerPostBack> list) 
        {
            return list.OrderByDescending(x => x.Created).ToList();

        }

        public static List<ServerToLocalPostBack> SortByCreated(this List<ServerToLocalPostBack> list)
        {
            return list.OrderByDescending(x => x.Created).ToList();

        }
    }
}
