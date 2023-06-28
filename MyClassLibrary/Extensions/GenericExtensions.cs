namespace MyClassLibrary.Extensions
{
    public static class GenericExtensions
    {
        public static int? GetId<T>(this T obj)
        {

            if (obj != null)
            {
                try
                {
                    return (int?)obj.GetType().GetProperty("Id")?.GetValue(obj);
                }
                catch
                {
                    try
                    {
                        return (int?)obj.GetType().GetProperty("ID")?.GetValue(obj);
                    }
                    catch
                    {
                        throw new ArgumentException($"{obj.ToString()} does not contain a property of Id or ID. The GetID GenericExtension in MyClassLibrary can't therefore return the Id.");
                    }

                }
            }


            throw new ArgumentException("No object was passed through to GetId()");

        }

        public static List<object> ToListObject<T>(this List<T> list)
        {
            return list.ConvertAll<object>(x => x!);
        }
    }

}
