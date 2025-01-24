using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ThermoformingReceipt : ContentPage
{
    public ObservableCollection<BarcodeMPME> PalletLista { get; set; }
    DataTable TblBl = new DataTable();
    DataTable TblDePallet = new DataTable();

    public ThermoformingReceipt()
    {
        InitializeComponent();
        CargarDatos();
        PalletLista = new ObservableCollection<BarcodeMPME>();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    private void CargarDatos()
    {
        TblBl = CatalogAccess.EjecutarConsultaDataTable("[spcellarlocationListXType]",
                                                             new Parametro("@Type", "5"));

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
        if (txtBarcode.Text == null)
        {
            await DisplayAlert("Información", "Debe realizar una lectura", "OK");
            txtBarcode.Focus();
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            TblDePallet = CatalogAccess.EjecutarConsultaDataTable("[spReceiverProdTerminadoPendDetPallet_Buscar]",
                                                    new Parametro("@BarcodePallet", txtBarcode.Text.ToString()));
            if (TblDePallet.Rows.Count > 0)
            {
                lblTxtNumRegistro.Text = TblDePallet.Rows.Count.ToString();
                foreach (DataRow D in TblDePallet.Rows)
                {
                    PalletLista.Add(new BarcodeMPME { CodPallet = D["Barcode"].ToString() });
                    lvPallet.ItemsSource = PalletLista;
                }
                lblMNumReg.Text = txtBarcode.Text;

                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            else
            {
                await DisplayAlert("Información", "Valide que el código de pallet exista o se encuentre registrado", "OK");
                txtBarcode.Text = "";
                PalletLista.Clear();
                lvPallet.ItemsSource = PalletLista;
                lblMNumReg.Text = "0";
                lblTxtNumRegistro.Text = "0";
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

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (lblMNumReg.Text.ToString() != "0")
        {
            if (PalletLista.Count > 0)
            {
                if (pkrLocalizacion.SelectedIndex < 0)
                {
                    await DisplayAlert("Información", "Debe seleccionar una localización", "Ok");
                    return;
                }

                bool answer = await DisplayAlert("Confirmación", "¿Está seguro de guardar los datos?", "Sí", "No");

                if (answer == true)
                {
                    string[] CodBod = pkrLocalizacion.SelectedItem.ToString().Trim().Split('-');
                    string CodBodeg = CodBod[0].Trim();
                    string CodLoca = CodBod[1].Trim();

                    CatalogAccess.EjecutarComando("[spReceiverProductionTPallet_Guardar]",
                        new Parametro("@BarcodePallet", lblMNumReg.Text.ToString()),
                        new Parametro("@CodBodega", CodBodeg.ToString()),
                        new Parametro("@CodLocalizacion", CodLoca.ToString()),
                        new Parametro("@fkUser", clsGlobal.LoggedUser.codsec.ToString()));

                    PalletLista.Clear();
                    lvPallet.ItemsSource = PalletLista;
                    await DisplayAlert("Información", lblMNumReg.Text + " Guardado correctamente", "Ok");
                    lblMNumReg.Text = "0";
                    lblTxtNumRegistro.Text = "0";
                }

            }
            else
            {
                await DisplayAlert("Error", "Debe consultar un pallet", "Ok");
            }
        }
    }

    private void btnLimpiar_Clicked(object sender, EventArgs e)
    {
        txtBarcode.Text = "";
        PalletLista.Clear();
        lvPallet.ItemsSource = PalletLista;
        lblMNumReg.Text = "0";
        lblTxtNumRegistro.Text = "0";
        txtBarcode.Focus();
    }
}