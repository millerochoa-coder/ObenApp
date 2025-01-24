using ObenApp.App_Code;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConsultationStatusPallet : ContentPage
{
    public ConsultationStatusPallet()
    {
        InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    DataTable tblInfoPallet = new DataTable();

    private void btnConsultar_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtBarcode.Text))
        {
            tblInfoPallet = CatalogAccess.EjecutarConsultaDataTable("[spConsultStatePalletXLocation]",
                new Parametro("@BarcodePallet", txtBarcode.Text));

            if (tblInfoPallet.Rows.Count > 0)
            {
                string InfoPallet = "";
                //BarcodeMPME barcode = new BarcodeMPME();
                //barcode.CodPallet = tblInfoPallet.Rows[0]["Codsec"].ToString();
                InfoPallet = $"Pallet: {tblInfoPallet.Rows[0]["Pallet"]}\nPeso: {tblInfoPallet.Rows[0]["Peso_Neto"]}\nReferencia: {tblInfoPallet.Rows[0]["Referencia"]}\nOV: {tblInfoPallet.Rows[0]["OV"]}\nCliente: {tblInfoPallet.Rows[0]["Cliente"]}\nUbicación: {tblInfoPallet.Rows[0]["Bodega"]}";

                txtInfoBarcode.Text = InfoPallet;
            }
        }
        else
        {
            DisplayAlert("Información", "Debe realizar una lectura de código de barras", "OK");
        }
    }
}