using ObenApp.App_Code;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConsultationBarcodeInventory : ContentPage
{
    public ConsultationBarcodeInventory()
    {
        InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    private async void btnConosultar_Clicked(object sender, EventArgs e)
    {
        if (txtBarcode.Text == null)
        {
            await DisplayAlert("Informaci�n", "Debe realizar una lectura", "OK");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            string Consulta = CatalogAccess.EjecutarComandoEscalar("[spRawMatBarcodeMPME_Buscar]",
                new Parametro("@Barcode", txtBarcode.Text.ToString()));

            txtInfoBarcode.Text = Consulta.ToString();

            if (string.IsNullOrEmpty(Consulta.ToString()))
            {
                await DisplayAlert("Informaci�n", "C�digo no encontrado", "Ok");
                txtBarcode.Text = "";
                txtBarcode.Focus();
                //TxtIinfoBarcode.Text = "C�digo no encontrado";
            }
        }
        else
        {
            await DisplayAlert("Informaci�n", "Debe realizar una lectura", "OK");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
    }

    private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}