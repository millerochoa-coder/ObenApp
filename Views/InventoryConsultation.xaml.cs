using ObenApp.App_Code;
using System.Collections.ObjectModel;
using System.Data;
using static ObenApp.Controller.BarcodeMPME;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InventoryConsultation : ContentPage
{
    public InventoryConsultation()
    {
        InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    DataTable TblBl = new DataTable();
    ObservableCollection<UserModel> Usr_List = new ObservableCollection<UserModel>();

    private async void btnConsultar_Clicked(object sender, EventArgs e)
    {
        if (txtBarcode.Text == null)
        {
            await DisplayAlert("Información", "Debe realizar una lectura", "OK");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            TblBl = CatalogAccess.EjecutarConsultaDataTable("[spInvFisicoDetXRawMat_Buscar]",
                new Parametro("@CodInventario", txtBarcode.Text.ToString()));

            if (TblBl.Rows.Count > 0)
            {
                foreach (DataRow T in TblBl.Rows)
                {
                    string[,] GuardarDatos = new string[1, 5];
                    GuardarDatos[0, 0] = T["CodMaterial"].ToString();
                    GuardarDatos[0, 1] = T["Nombre"].ToString();
                    GuardarDatos[0, 2] = T["UM"].ToString();
                    GuardarDatos[0, 3] = T["Cantidad"].ToString();
                    GuardarDatos[0, 4] = T["Barcode"].ToString();

                    this.Usr_List.Add(new UserModel { Codigo = "Codigo", Nombre = "Nombre", UM = "UM", Cantidad = "Cantidad", Barcode = "Barcode" });

                    this.Usr_List.Add(new UserModel { Codigo = GuardarDatos[0, 0].ToString(), Nombre = GuardarDatos[0, 1].ToString(), UM = GuardarDatos[0, 2], Cantidad = GuardarDatos[0, 3].ToString(), Barcode = GuardarDatos[0, 4].ToString() });

                    listx.ItemsSource = Usr_List;
                }
            }
            else
            {
                await DisplayAlert("Información", "Código no encontrado", "Ok");
            }
        }
        else
        {
            await DisplayAlert("Información", "Debe realizar una lectura", "OK");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
    }

    private async void listx_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            bool answer = await DisplayAlert("Confirmación", "Está seguro de eliminar el código " + ((UserModel)e.Item).Codigo.ToString() + " " + ((UserModel)e.Item).Nombre.ToString() + " " + ((UserModel)e.Item).UM.ToString() + " " + ((UserModel)e.Item).Cantidad.ToString() + " " + ((UserModel)e.Item).Barcode.ToString(), "Sí", "No");

            if (answer == true)
            {
                Usr_List.RemoveAt(e.ItemIndex);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}