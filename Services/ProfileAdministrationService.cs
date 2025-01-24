using ObenApp.App_Code;
using ObenApp.Interfaces;
using ObenApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Services
{
    public class ProfileAdministrationService : IProfileAdministrationService
    {
        public ProfileAdministrationService() { }

        public ObservableCollection<ProfileAdministration> SearchUser()
        {
            DataTable users = CatalogAccess.EjecutarConsultaDataTable("sp_SearhAllUsers");
            ObservableCollection<ProfileAdministration> resultsToReturn = new ObservableCollection<ProfileAdministration>();

            try
            {
                foreach (DataRow user in users.Rows) resultsToReturn.Add(new ProfileAdministration() { Name = user["fullnameuser"].ToString() });

                if (resultsToReturn.Count > 0) return resultsToReturn; else throw new NullReferenceException();
            }
            catch (Exception) { return resultsToReturn; }
        }

        public ObservableCollection<string> GetOptions()
        {
            ObservableCollection<string> options = new ObservableCollection<string>() { "Cambiar contraseña", "Asignar permiso", "Bloquear o activar" };
            return options;
        }
    }
}
