using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service
{
    public static class ListExtensionMethods
    {
        public static Collection<T> ToCollection<T>(this List<T> list)
        {
            return new Collection<T>(list);
        }
    }
}
