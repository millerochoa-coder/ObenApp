using ObenApp.App_Code;
using ObenApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Services
{
    public class CreatePositionAndBlockService : ICreatePositionAndBlockService
    {
        public CreatePositionAndBlockService() { }

        public string SavePositionOrBlock(string option, string name, int floor)
        {
            string response = string.Empty;
            try
            {
                if (option.Contains("-"))
                {
                    response = CatalogAccess.EjecutarComandoEscalar("sp_SavePositionOrBlock",
                        new Parametro("@OPTION", 1),
                        new Parametro("@NAME", name),
                        new Parametro("@FKUSER", clsGlobal.LoggedUser.codsec));
                }
                else if (option.Contains("BLOQUE"))
                {
                    response = CatalogAccess.EjecutarComandoEscalar("sp_SavePositionOrBlock",
                        new Parametro("@OPTION", 2),
                        new Parametro("@NAME", name),
                        new Parametro("@FKUSER", clsGlobal.LoggedUser.codsec));
                }

                return response;
            }
            catch (Exception) { return string.Empty; }
        }
    }
}
