using ObenApp.App_Code;
using ObenApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Interfaces
{
    public interface IProfileAdministrationService
    {
        ObservableCollection<ProfileAdministration> SearchUser();
        ObservableCollection<string> GetOptions();
    }
}
