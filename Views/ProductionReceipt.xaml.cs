using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ProductionReceipt : ContentPage
{
    public ProductionReceipt()
    {
        InitializeComponent();
        CargarDatos();
        BarcodeLista = new ObservableCollection<BarcodeMPME>();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
    }

    public ObservableCollection<BarcodeMPME> BarcodeLista { get; set; }
    DataTable TblBl = new DataTable();

    private void CargarDatos()
    {
        TblBl = CatalogAccess.EjecutarConsultaDataTable("[spcellarlocationListXType]",
                                                             new Parametro("@Type", "1"));

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

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        if (txtBarcode.Text == null)
        {
            await DisplayAlert("Información", "Debe realizar una lectura", "OK");
            txtBarcode.Focus();
            return;
        }
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            string Rta = CatalogAccess.EjecutarComandoEscalar("[spRawMatReceiverProdValBarcode]",
                                                    new Parametro("@Barcode", txtBarcode.Text.ToString()));
            if (Rta == "OK")
            {
                if (BarcodeLista.Count > 0)
                {
                    int countduplciado = (from item in BarcodeLista
                                          where item.CodBarcode == txtBarcode.Text
                                          select item).Count();

                    if (countduplciado >= 1)
                    {
                        await DisplayAlert("Información", "Ya existe en la Lista", "OK");
                        return;
                    }
                }

                BarcodeLista.Add(new BarcodeMPME { CodBarcode = txtBarcode.Text.ToString() });
                lvBarcode.ItemsSource = BarcodeLista;
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            else
            {
                await DisplayAlert("Información", "Código no encontrado", "OK");
                txtBarcode.Text = "";
                txtBarcode.Focus();
                return;
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
        if (BarcodeLista.Count > 0)
        {
            if (pkrLocalizacion.SelectedIndex < 0)
            {
                await DisplayAlert("Información", "Debe seleccionar una localización", "OK");
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
                    foreach (BarcodeMPME item in BarcodeLista)
                    {
                        CatalogAccess.EjecutarComando("[spRawMatReceiverProductionAPP_Guardar]",
                            new Parametro("@Barcode", item.CodBarcode.ToString()),
                            new Parametro("@CodBodega", CodBodeg.ToString()),
                            new Parametro("@CodLocalizacion", CodLoca.ToString()),
                            new Parametro("@fkUser", clsGlobal.LoggedUser.codsec.ToString()));
                    }

                    BarcodeLista.Clear();
                    lvBarcode.ItemsSource = BarcodeLista;
                    await DisplayAlert("Información", "Datos guardados correctamente", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

    private async void lvBarcode_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            bool answer = await DisplayAlert("Confirmación", "Está seguro de eliminar el código " + ((BarcodeMPME)e.Item).CodBarcode.ToString() + "", "SÍ", "No");

            if (answer == true)
            {
                BarcodeLista.RemoveAt(e.ItemIndex);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}