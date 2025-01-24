using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Interfaces
{
    public interface ICreatePositionAndBlockService
    {
        string SavePositionOrBlock(string option, string name, int floor);
    }
}
