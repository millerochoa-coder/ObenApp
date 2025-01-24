using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObenApp.Controller
{
    public class BarcodeMPME
    {
        public string CodBarcode { get; set; }
        public string CodInvFis { get; set; }
        public string CodPallet { get; set; }
        public string CodCoil { get; set; }
        public string Pallet { get; set; }
        public string CoilNumber { get; set; }
        public string OV { get; set; }
        public string PalletPosition { get; set; }
        public string Material { get; set; }

        public string Cod { get; set; }
        public string CantOrdenada { get; set; }
        public string CantEntregada { get; set; }
        public string Request { get; set; }
        public string SelectedOption { get; set; }
        public static string CurrentOV { get; set; }
        public string NetWeightPallet { get; set; }

        public List<BarcodeMPME> PalletLocation { get; set; }

        public class UserModel
        {
            [PrimaryKey, AutoIncrement]
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string UM { get; set; }
            public string Cantidad { get; set; }
            public string Barcode { get; set; }
        }

        public class IpAddress
        {
            public static string NetWork = "OPPFILM_WS";
            public static string initIpAddress = "192.168.30.1";
            public static string endIpAddress = "192.168.30.254";
        }

        public string GenCodInvFis { get; set; }

        private string _dato;

        public string Dato
        {
            get { return _dato; }
            set
            {
                if (_dato != value)
                {
                    _dato = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
