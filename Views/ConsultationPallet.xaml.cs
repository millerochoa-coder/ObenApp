using ObenApp.App_Code;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConsultationPallet : ContentPage
{
    public ConsultationPallet()
    {
        InitializeComponent();
        LblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    DataTable TblBl = new DataTable();

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
            TblBl = CatalogAccess.EjecutarConsultaDataTable("[spPalletAuditorycellarInt]",
                new Parametro("@Pcode", txtBarcode.Text.ToString()));

            if (TblBl.Rows.Count > 0)
            {
                foreach (DataRow T in TblBl.Rows)
                {
                    txtInfoBarcode.Text = T["plt_codsec"].ToString() + "\n" + T["plt_code"].ToString() + "\n" + T["plt_productCode"].ToString() + "\n" + T["plt_salesOrderNumber"].ToString() + "\n" + T["plt_customerName"].ToString() + "\n" + T["plt_netWeight"].ToString() + "\n" + T["plt_grossWeight"].ToString() + "\n" + T["plt_diference"].ToString() + "\n" + T["plt_message"].ToString();
                }
            }
            else
            {
                await DisplayAlert("Error", "Código no encontrado", "Ok");
                txtBarcode.Text = "";
                txtBarcode.Focus();
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
}