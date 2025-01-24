using ObenApp.App_Code;
using ObenApp.Controller;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MaterialDelivery : ContentPage
{
    public MaterialDelivery()
    {
        InitializeComponent();
    }

    public List<BarcodeMPME> listasolicitudes { get; set; }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        if (lvSolicitud.ItemsSource != null)
        {
            string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "MATERIAL DE EMPAQUE BOPP", "HOMOPOLIMERO", "MATERIAL DE EMPAQUE FABRICADO");
            if (action == "HOMOPOLIMERO")
            {
                await Navigation.PushAsync(new CreateCodeInventory());
            }
        }
    }

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        if (txtSolicitudEntrega.Text != null)
        {
            lblInfoSolicitud.Text = clsGlobal.LoggedUser.ToString();
            lblInfoRef.Text = "REF753";
            lblInfoEstado.Text = "En Proceso";

            listasolicitudes.Add(new BarcodeMPME { Cod = "123456", Material = "HOMOPOLIMERO", CantOrdenada = "55.00", CantEntregada = "45.00" });
            lvSolicitud.ItemsSource = listasolicitudes;
        }
        else
        {
            await DisplayAlert("Info", "Debe elegir una solicitud de material", "Ok");
        }
    }
}