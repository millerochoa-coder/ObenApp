using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CreateCodeInventory : ContentPage
{
    public CreateCodeInventory()
    {
        InitializeComponent();
        CargarDatos();
        LblUsuario.Text = clsGlobal.LoggedUser.ToString();

        if (!string.IsNullOrEmpty(txtCodigoInvGenerado.Text.ToString()))
        {
            this.CodInvFisGen.Add(new BarcodeMPME { GenCodInvFis = txtCodigoInvGenerado.Text.ToString() });
        }
    }

    DataTable TblCodInv = new DataTable();
    DataTable Tbl1 = new DataTable();
    DataTable TblB2 = new DataTable();
    DataTable TblClaseMat = new DataTable();
    DataTable TblTipoMat = new DataTable();
    ObservableCollection<BarcodeMPME> CodInvFisGen = new ObservableCollection<BarcodeMPME>();
    public static string data = "";
    public static bool RespuestaDatos = new bool();

    private void CargarDatos()
    {
        TblClaseMat = CatalogAccess.EjecutarConsultaDataTable("[spInvFisicoListaClaseMat_Buscar]");

        if (TblClaseMat.Rows.Count > 0)
        {
            string ListClaseMat = "";
            foreach (DataRow T in TblClaseMat.Rows)
            {
                ListClaseMat = T["ClaseMaterial"].ToString();
                pkrClaseMaterial.Items.Add(ListClaseMat.ToString());
            }
        }
    }

    private async void btnGenerarCod_Clicked(object sender, EventArgs e)
    {
        if (pkrTipoMaterial.SelectedItem.ToString() == null && pkrClaseMaterial.SelectedItem.ToString() == null)
        {
            await DisplayAlert("Error", "Rellene los campos en blanco", "Ok");
            //return;
        }
        else if (!string.IsNullOrEmpty(pkrTipoMaterial.SelectedItem.ToString()) && !string.IsNullOrEmpty(pkrClaseMaterial.SelectedItem.ToString()))
        {
            string consulta = CatalogAccess.EjecutarComandoEscalar("[spInvFisicoEnc_Insert]",
                new Parametro("@InvFkUser", clsGlobal.LoggedUser.codsec.ToString()),
                new Parametro("@InvFecha", Convert.ToDateTime(dpFecha.Date)),
                new Parametro("@InvRawMatType", 1), //PickTipoMat.SelectedItem
                new Parametro("@InvRawMatClase", 1));//PickClaseMat.SelectedItem

            if (Convert.ToInt32(consulta) != 0)
            {
                TblB2 = CatalogAccess.EjecutarConsultaDataTable("[spInvFisicoListaActivo_Buscar]");
                if (TblB2.Rows.Count > 0)
                {
                    foreach (DataRow T in TblB2.Rows)
                    {
                        string CodigoGen = "";
                        CodigoGen = T["Codigo"].ToString();
                        txtCodigoInvGenerado.Text = CodigoGen.ToString();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Código no encontrado", "Ok");
                }
            }
        }
        else
        {
            await DisplayAlert("Error", "Rellene los campos en blanco", "Ok");
        }
    }

    private void pkrClaseMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        pkrTipoMaterial.Items.Clear();
        if (!string.IsNullOrEmpty(pkrClaseMaterial.SelectedItem.ToString()))
        {
            string[] ClaseMat = pkrClaseMaterial.SelectedItem.ToString().Trim().Split('-');
            string CodClaseMat = ClaseMat[0].Trim();
            TblTipoMat = CatalogAccess.EjecutarConsultaDataTable("[spInvFisicoListaTipoMat_Buscar]",
                new Parametro("@CodClaseMat", CodClaseMat.ToString()));

            if (TblTipoMat.Rows.Count > 0)
            {
                string ListTipoMat = "";
                foreach (DataRow T in TblTipoMat.Rows)
                {
                    ListTipoMat = T["TipoMaterial"].ToString();
                    pkrTipoMaterial.Items.Add(ListTipoMat.ToString());
                }
            }
        }
    }
}