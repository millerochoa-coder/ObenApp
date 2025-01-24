using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class WarehouseReceipt : ContentPage
{
    public ObservableCollection<BarcodeMPME> BarcodeLista { get; set; }
    public ObservableCollection<BarcodeMPME> chooserequest { get; set; }
    DataTable TblBl = new DataTable();
    DataTable TblBDet = new DataTable();

    public WarehouseReceipt()
    {
        InitializeComponent();
        CargarDatos();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
        DataTable Wfi = new DataTable();
        chooserequest = new ObservableCollection<BarcodeMPME>();
    }

    private void CargarDatos()
    {
        TblBl = CatalogAccess.EjecutarConsultaDataTable("[spcellarlocationListXType]",
                                                             new Parametro("@Type", "2"));

        if (TblBl.Rows.Count > 0)
        {
            string codeLocalizacion = "";
            foreach (DataRow T in TblBl.Rows)
            {
                codeLocalizacion = T["CodBodega"].ToString() + " - " + T["CodLocalizacion"].ToString() + " - " + T["Localizacion"].ToString();
                pkrLocalizacion.Items.Add(codeLocalizacion.ToString());
            }
        }
    }

    private async void btnConsultar_Clicked(object sender, EventArgs e)
    {
        pkrListaRequest.Items.Clear();
        if (txtBarcode.Text == null)
        {
            await DisplayAlert("Información", "Debe realizar una lectura", "Ok");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            if (pkrLocalizacion.SelectedIndex < 0)
            {
                await DisplayAlert("Información", "Debe seleccionar una localización", "OK");
                txtBarcode.Text = "";
                txtBarcode.Focus();
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
                {
                    string[] CodBod = pkrLocalizacion.SelectedItem.ToString().Trim().Split('-');
                    string CodBodeg = CodBod[0].Trim();
                    string CodLoca = CodBod[1].Trim();
                    TblBl = CatalogAccess.EjecutarConsultaDataTable("[spRawMatReceiverAppListaRQ_Buscar]",
                        new Parametro("@Barcode", txtBarcode.Text.ToString()),
                        new Parametro("@Bodega", CodBodeg.ToString()),
                        new Parametro("@Localizacion", CodLoca.ToString()));

                    if (TblBl.Rows.Count > 0)
                    {
                        foreach (DataRow T in TblBl.Rows)
                        {
                            pkrListaRequest.Items.Add(T["RQ"].ToString());
                        }

                        if (pkrListaRequest.Items.Count == 1)
                        {
                            TblBDet = CatalogAccess.EjecutarConsultaDataTable("[spRawMatReceiverAppV2_Buscar]",
                        new Parametro("@Barcode", txtBarcode.Text.ToString()),
                        new Parametro("@Bodega", CodBodeg.ToString()),
                        new Parametro("@Localizacion", CodLoca.ToString()));

                            if (TblBDet.Rows.Count > 0)
                            {
                                pkrListaRequest.IsEnabled = false;
                                if (Convert.ToBoolean(TblBDet.Rows[0]["EstadoRecibo"]) == false)
                                {
                                    pkrListaRequest.SelectedIndex = 0;
                                    //TxtIinfoBarcode.Text = TblBDet.Rows[0]["Informacion"].ToString();
                                    //LblInfo.Text = TblBDet.Rows[0]["CodRQ"].ToString();
                                    //LblInfo2.Text = TblBDet.Rows[0]["Barcode"].ToString();
                                    //LblInfo3.Text = TblBDet.Rows[0]["NumRQ"].ToString();
                                }
                                else
                                {
                                    await DisplayAlert("Infornmación", TblBDet.Rows[0]["Informacion"].ToString(), "Ok");
                                }

                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            if (pkrListaRequest.Items.Count > 1)
                            {
                                pkrListaRequest.IsEnabled = true;
                                lblInfo3.Text = "";
                                lblInfo.Text = "";
                                lblInfo2.Text = "";
                                txtInfoBarcode.Text = "";
                                pkrListaRequest.Focus();

                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Información", "Código no encontrado", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtBarcode.Text))
        {
            await DisplayAlert("Error", "Debe realizar una lectura", "Ok");
            txtBarcode.Text = "";
            txtBarcode.Focus();
            return;
        }
        else
        {
            try
            {
                bool answer = await DisplayAlert("Confirmación", "Está seguro de guardar los datos?", "Sí", "No");

                if (answer == true)
                {
                    if (!string.IsNullOrEmpty(txtInfoBarcode.Text.ToString()))
                    {
                        CatalogAccess.EjecutarComando("[sprawmaterialrequestMQReceiver]",
                                         new Parametro("@rwmrecodsec", lblInfo.Text.ToString()),
                                         new Parametro("@rwmredccode", lblInfo2.Text.ToString()),
                                         new Parametro("@rwredcfkUserReceiver", clsGlobal.LoggedUser.codsec.ToString()));

                        CatalogAccess.EjecutarComando("[sprawmaterialrequestMQReceiver_Cerrar]",
                                                new Parametro("@rwmrecodsec", lblInfo.Text.ToString()),
                                                new Parametro("@rwmrecode", lblInfo3.Text.ToString()),
                                                new Parametro("@rwredcfkUserReceiver", clsGlobal.LoggedUser.codsec.ToString()),
                                                new Parametro("@type", "G"));

                        if (CatalogAccess.UltimoError() == null)
                        {
                            txtBarcode.Text = "";
                            await DisplayAlert("Información", "Datos guardados correctamente", "OK");

                            lblInfo.Text = "";
                            lblInfo2.Text = "";
                            lblInfo3.Text = "";
                        }
                        else if (CatalogAccess.UltimoError() != null)
                        {
                            await DisplayAlert("Información", "Código ya registrado", "Ok");
                        }

                    }
                    else
                    {
                        await DisplayAlert("Información", "Debe realizar una lectura", "Ok");
                        //return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void pkrListaRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        pkrLocalizacion.IsEnabled = false;
    }

    private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue.ToString()))
        {
            txtInfoBarcode.Text = "";
            txtBarcode.Focus();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (pkrLocalizacion.IsEnabled == false)
        {
            pkrLocalizacion.IsEnabled = true;
        }
        else
        {
            pkrLocalizacion.IsEnabled = false;
        }
    }

    private void pkrLocalizacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pkrListaRequest.SelectedIndex > -1)
        {
            string[] CodBod = pkrLocalizacion.SelectedItem.ToString().Trim().Split('-');
            string CodBodeg = CodBod[0].Trim();
            string CodLoca = CodBod[1].Trim();
            string[] CodSelect = pkrListaRequest.SelectedItem.ToString().Trim().Split('-');
            string CodRQselect = CodSelect[0].Trim();

            TblBDet = CatalogAccess.EjecutarConsultaDataTable("[spRawMatReceiverAppV3_Buscar]",
                        new Parametro("@Barcode", txtBarcode.Text.ToString()),
                        new Parametro("@Bodega", CodBodeg.ToString()),
                        new Parametro("@Localizacion", CodLoca.ToString()),
                        new Parametro("@CodRQ", CodRQselect.ToString()));

            if (TblBDet.Rows.Count > 0)
            {
                if (Convert.ToBoolean(TblBDet.Rows[0]["EstadoRecibo"]) == false)
                {
                    txtInfoBarcode.Text = TblBDet.Rows[0]["Informacion"].ToString();
                    lblInfo.Text = TblBDet.Rows[0]["CodRQ"].ToString();
                    lblInfo2.Text = TblBDet.Rows[0]["Barcode"].ToString();
                    lblInfo3.Text = TblBDet.Rows[0]["NumRQ"].ToString();
                }
            }
        }
    }
}