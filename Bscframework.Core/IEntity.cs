using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bscframework.Core
{
    public interface IEntity<T> 
        where T : IComparable, IEquatable<T>
    {
        T Id { get; set; }
    }
}
