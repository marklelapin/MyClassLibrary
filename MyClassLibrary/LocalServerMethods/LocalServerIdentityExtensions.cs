

using MyClassLibrary.Interfaces;
using MyClassLibrary.LocalServerMethods;

namespace MyClassLibrary.LocalServerMethods
{
    public static class LocalServerIdentityExtensions
    {
        public static void SortById<T>(this List<T> list) where T : LocalServerIdentity
        {
            list.Sort((x, y) => x.Id.CompareTo(y.Id));
        }

    }
}
