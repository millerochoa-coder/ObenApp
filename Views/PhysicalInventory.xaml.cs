using ObenApp.App_Code;
using ObenApp.Controller;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PhysicalInventory : ContentPage
{
    public PhysicalInventory()
    {
        InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
        CargarDatos();
        pkrListaActivo.Focused += MiPicker_Focused;
    }

    ObservableCollection<BarcodeMPME> CodInvFisGen = new ObservableCollection<BarcodeMPME>();
    DataTable Tbl1 = new DataTable();
    DataTable Tbl2 = new DataTable();
    int NRow = 1;

    public class DataItem
    {
        public string InfoCodigo { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
    }

    void MiPicker_Focused(object sender, FocusEventArgs e)
    {
        if (Tbl2.Rows.Count > 0)
        {
            CargarDatos();
        }
    }

    private void CargarDatos()
    {
        pkrListaActivo.Items.Clear();
        Tbl2 = CatalogAccess.EjecutarConsultaDataTable("[spInvFisicoListaActivo_Buscar]");

        if (Tbl2.Rows.Count > 0)
        {
            string ListaUser = "";
            foreach (DataRow T in Tbl2.Rows)
            {
                ListaUser = T["Codigo"].ToString() + " - " + T["Usuario"].ToString() + " - " + Convert.ToDateTime(T["Fecha"]).ToShortDateString();
                pkrListaActivo.Items.Add(ListaUser.ToString());
            }
        }
    }

    private async void btnCrearCod_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateCodeInventory());
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(pkrListaActivo.SelectedItem.ToString()) && !string.IsNullOrEmpty(txtCantidad.Text) && !string.IsNullOrEmpty(txtBarcode.Text))
        {
            await DisplayAlert("Error", "Debe seleccionar un campo", "Ok");
            return;
        }
        else if (!string.IsNullOrEmpty(pkrListaActivo.SelectedItem.ToString()) && !int.TryParse(txtCantidad.Text, out int cantidad) && !string.IsNullOrEmpty(txtBarcode.Text))
        {
            await DisplayAlert("Error", "La cantidad debe ser un valor numérico", "Ok");
            return;
        }
        else if (!string.IsNullOrEmpty(pkrListaActivo.SelectedItem.ToString()) && !string.IsNullOrEmpty(txtCantidad.Text) && !string.IsNullOrEmpty(txtBarcode.Text))
        {
            if (Tbl1.Rows.Count > 0)
            {
                bool respuesta = await DisplayAlert("Info", "¿Está seguro de guardar los datos?", "Sí", "No");
                if (respuesta)
                {
                    string[] CodInv = pkrListaActivo.SelectedItem.ToString().Trim().Split('-');
                    string Inv = CodInv[0].Trim();

                    CatalogAccess.EjecutarComando("[spInvFisicoDet_Insert]",
                        new Parametro("@InvFkCodInv", Inv.ToString()),
                        new Parametro("@InvCantidad", txtCantidad.Text.ToString()),
                        new Parametro("@InvBarcode", txtBarcode.Text.ToString()));

                    await DisplayAlert("Información", "Datos guardados correctamente", "Ok");
                    txtCantidad.Text = "";
                    txtBarcode.Text = "";
                }
                else
                {
                    await DisplayAlert("Error", "Error al guardar los datos", "Ok");
                }
            }
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(pkrListaActivo.SelectedItem.ToString()))
        {
            pkrListaActivo.IsEnabled = false;
        }
        else
        {
            pkrListaActivo.IsEnabled = false;
        }
    }

    private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtBarcode.Text.ToString()))
        {
            //await DisplayAlert("Mensaje", "Correcto", "ok");
            Tbl1 = CatalogAccess.EjecutarConsultaDataTable("[sprawmaterialxInvBarcode_Buscar]",
                new Parametro("@Barcode", txtBarcode.Text.ToString()));

            if (Tbl1.Rows.Count > 0)
            {
                lblCodigo.Text = Tbl1.Rows[0]["Codigo"].ToString();
                lblDescripcion.Text = Tbl1.Rows[0]["Descripcion"].ToString();
                //LblLote.Text = Tbl1.Rows[0]["Lote"].ToString();
                lblBarcode.Text = Tbl1.Rows[0]["Barcode"].ToString();
                lblUM.Text = Tbl1.Rows[0]["UM"].ToString();
                //gridLayout.RowDefinitions.Add(new RowDefinition()); CAMBIAR
                //gridLayout.RowDefinitions.Add(new RowDefinition()); CAMBIAR
                //gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
                //gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
                //gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
                //gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
                //gridLayout.Children.Add(LblInfoCodigo, 1, NRow);
                //gridLayout.Children.Add(LblInfoDescripcion, 2, NRow);
                //gridLayout.Children.Add(LblBarcode, 3, NRow);
                //gridLayout.Children.Add(LblUM, 4, NRow);
                NRow++;

                frmDataList.IsVisible = true;
            }

            var datalist = new List<DataItem>
            {
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
                new DataItem { InfoCodigo= "Valor1", Column2= "Valor2", Column3= "Valor3", Column4= "Valor3", Column5= "Valor3", Column6= "Valor3", Column7= "Valor3", Column8= "Valor3",},
            };

        }
    }
}