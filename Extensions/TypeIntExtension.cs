using ObenApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Extensions
{
    public static class TypeIntExtension
    {
        public static bool CustomHandler(this int number, Func<int, bool> func)
        {
            if (func(number)) return true; else return false;
        }
    }
}
