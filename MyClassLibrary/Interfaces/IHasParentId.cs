using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Interfaces
{
    public interface IHasParentId<T>
    {
        public T Id { get; set; }

        public T ParentId { get; set; }
    }
}
