//using Android.Renderscripts;
using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Cryptography;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ReceiptFinishedProduct : ContentPage
{
    ObservableCollection<BarcodeMPME> BarcodeLista = new ObservableCollection<BarcodeMPME>();
    DataTable TblLocation = new DataTable();
    DataTable TblPalletPosition = new DataTable();
    DataTable TblLocationBlock = new DataTable();
    DataTable TblPalletList = new DataTable();
    DataTable CoilList = new DataTable();
    public string Rta;

    public ReceiptFinishedProduct()
    {
        InitializeComponent();
        BindingContext = this;
        InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
        CargarDatos();
        lblPallet.IsVisible = false;
        lblLocation.IsVisible = false;
        lblCoilNum.IsVisible = false;
        bvSeparator.IsVisible = false;
        lvBarcode.ItemSelected += lvBarcode_ItemSelected;
        pkrLocationOV.IsVisible = false;
        lblNumbersPalletIn.IsVisible = false;
    }

    private void CargarDatos()
    {
        TblLocation = CatalogAccess.EjecutarConsultaDataTable("[spcellarlocationListXType]",
            new Parametro("@Type", 5));

        TblLocationBlock = CatalogAccess.EjecutarConsultaDataTable("[spBlockLocation]");

        if (TblLocation.Rows.Count > 0)
        {
            string Location;
            foreach (DataRow row in TblLocation.Rows)
            {
                Location = row["CodBodega"].ToString() + " - " + row["CodLocalizacion"].ToString() + " - " + row["Localizacion"].ToString();
                pkrLocalizacion.Items.Add(Location.ToString());
            }
        }

        if (TblLocationBlock.Rows.Count > 0)
        {
            string BlockLocation;
            foreach (DataRow row in TblLocationBlock.Rows)
            {
                BlockLocation = row["Codsec"].ToString() + " - " + row["Bloque"].ToString();
                pkrBloque.Items.Add(BlockLocation);
            }
        }
    }

    private void lvBarcode_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }
        else
        {
            BarcodeMPME SelectedItem = (BarcodeMPME)e.SelectedItem;
            string selectedItem = SelectedItem.CodBarcode;
            ((ListView)sender).SelectedItem = null;
            ShowCoil(selectedItem.ToString());
        }
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (BarcodeLista.Count > 0)
        {
            if (pkrLocalizacion.SelectedIndex < 0)
            {
                await DisplayAlert("Información", "Debe seleccionar una localización", "OK");
                return;
            }
            else if (pkrLocationOV.SelectedIndex < 0)
            {
                await DisplayAlert("Información", "Debe seleccionar una ubicacion para la OV y el Pallet", "OK");
                return;
            }
            try
            {
                bool answer = await DisplayAlert("Confirmación", "¿Está seguro de guardar los datos?", "Sí", "No");

                if (answer)
                {
                    string[] CodBod = pkrLocalizacion.SelectedItem.ToString().Trim().Split('-');
                    string CodBodeg = CodBod[0].Trim();
                    string CodLoca = CodBod[1].Trim();

                    string[] CodBlock = pkrBloque.SelectedItem.ToString().Trim().Split('-');
                    string CodsecBlock = CodBlock[0].Trim();

                    string[] CodLocation = pkrLocationOV.SelectedItem.ToString().Trim().Split('-');
                    string CodsecLocation = CodLocation[0].Trim();

                    foreach (var row in BarcodeLista)
                    {
                        try
                        {
                            Rta = CatalogAccess.EjecutarComandoEscalar("[sptrasladoTestPt]",
                                new Parametro("@fkPosition", int.Parse(CodsecLocation.ToString())),
                                new Parametro("@fkUser", int.Parse(clsGlobal.LoggedUser.codsec.ToString())),
                                new Parametro("@fkBlock", int.Parse(CodsecBlock.ToString())),
                                new Parametro("@fkPallet", int.Parse(row.CodBarcode.ToString())),
                                new Parametro("@fkSalesOrder", int.Parse(row.OV.ToString())));
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", ex.ToString(), "OK");
                        }
                    }

                    if (Rta == "OK")
                    {
                        BarcodeLista.Clear();
                        lvBarcode.ItemsSource = BarcodeLista;
                        await DisplayAlert("Información", "Datos guardados correctamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Información", Rta, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (pkrBloque.SelectedIndex < 0)
        {
            DisplayAlert("Información", "Debe seleccionar un bloque para la localización del Pallet", "OK");
            txtBarcode.Text = "";
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            TblPalletList = CatalogAccess.EjecutarConsultaDataTable("[spReceiverProdTerminadoFilmPendDetPallet_Buscar]",
                new Parametro("@BarcodePallet", txtBarcode.Text.ToString()),
                new Parametro("@Option", 1));

            string[] Block = pkrBloque.SelectedItem.ToString().Trim().Split('-');
            int CodsecBlock = int.Parse(Block[0].Trim());

            TblPalletPosition = CatalogAccess.EjecutarConsultaDataTable("[spPositionXBlock]",
                    new Parametro("@CodsecBlock", CodsecBlock));

            if (TblPalletList.Rows.Count > 0 && TblPalletPosition.Rows.Count > 0)
            {
                if (BarcodeLista.Count > 0)
                {
                    int countduplicado = (from item in BarcodeLista
                                          where item.CodBarcode == txtBarcode.Text
                                          select item).Count();

                    if (countduplicado >= 1)
                    {
                        DisplayAlert("Información", "Ya existe en la Lista", "OK");
                        txtBarcode.Text = "";
                        return;
                    }
                    else
                    {
                        if (!ValidateUniqueOV(TblPalletList.Rows[0]["OV"].ToString(), BarcodeMPME.CurrentOV.ToString().Trim()))
                        {
                            txtBarcode.Text = "";
                            return;
                        }
                    }
                }

                foreach (DataRow row in TblPalletPosition.Rows)
                {
                    pkrLocationOV.Items.Add(row["CodPosition"].ToString() + " - " + row["Position"].ToString());
                }

                BarcodeLista.Add(new BarcodeMPME { CodBarcode = TblPalletList.Rows[0]["Pallet"].ToString(), OV = TblPalletList.Rows[0]["Ov"].ToString(), CodPallet = TblPalletList.Rows[0]["CodsecPallet"].ToString(), NetWeightPallet = TblPalletList.Rows[0]["Peso_Pallet"].ToString() });
                BarcodeMPME.CurrentOV = TblPalletList.Rows[0]["OV"].ToString();

                lblPallet.IsVisible = true;
                lblLocation.IsVisible = true;
                lblCoilNum.IsVisible = true;
                bvSeparator.IsVisible = true;
                lvBarcode.ItemsSource = BarcodeLista;
                txtBarcode.Text = "";
                txtBarcode.Focus();

                pkrLocationOV.IsVisible = true;

                CountMaterialInPT(BarcodeMPME.CurrentOV.ToString());
            }
            else
            {
                DisplayAlert("Información", "Código no encontrado", "OK");
                txtBarcode.Text = "";
                txtBarcode.Focus();
                return;
            }
        }
    }

    private void ShowCoil(string BarcodePallet)
    {
        CoilList = CatalogAccess.EjecutarConsultaDataTable("[spReceiverProdTerminadoFilmPendDetPallet_Buscar]",
            new Parametro("@BarcodePallet", BarcodePallet.ToString()),
            new Parametro("@Option", 2));

        if (CoilList.Rows.Count > 0)
        {
            List<string> coilList = new List<string>();
            foreach (DataRow p in CoilList.Rows)
            {
                coilList.Add(p["Bobinas"].ToString());
            }
            string content = string.Join("\n", coilList);
            DisplayAlert("Información", "Bobina(s) por pallet: " + content.ToString(), "Ok");
        }
    }

    private bool ValidateUniqueOV(string inputOV, string currentOV)
    {
        bool response = true;
        while (inputOV == currentOV)
        {
            continue;
        }
        while (inputOV != currentOV)
        {
            response = false;
            DisplayAlert("Información", "No se puede capturar un Pallet con una OV diferente al inicial", "OK");
            break;
        }
        return response;
    }

    private void CountMaterialInPT(string OV)
    {
        DataTable TblTemp = CatalogAccess.EjecutarConsultaDataTable("[spCountMaterialInPTXCE]",
            new Parametro("@OV", OV));

        if (TblTemp.Rows.Count > 0)
        {
            lblNumbersPalletIn.Text = TblTemp.Rows[0]["IngresadoA_PT"].ToString();
            lblNumbersPalletIn.IsVisible = true;
        }
    }
}