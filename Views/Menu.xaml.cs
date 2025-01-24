using ObenApp.App_Code;
using ObenApp.Interfaces;
using System.Data;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Menu : ContentPage
{
    public Menu()
    {
        InitializeComponent();
        CargarPermiso();
    }

    DataTable TblPermisosU = new DataTable();

    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            bool exit = await DisplayAlert("Salir", "¿Desea cerrar sesión?", "Sí", "No");
            if (exit)
            {
                // Cerrar la ventana
                //System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                await Navigation.PopAsync();
            }
        });

        return true; // Consumir el evento de retroceso
    }

    private void CargarPermiso()
    {
        TblPermisosU = CatalogAccess.EjecutarConsultaDataTable("[spGMenuOpcionesUsuarioBPSApp_Buscar]",
            new Parametro("@fkUsuario", clsGlobal.LoggedUser.codsec));
        if (TblPermisosU.Rows.Count > 0)
        {
            foreach (DataRow T in TblPermisosU.Rows)
            {
                //----------- a almacen ---------------------

                btnProductionReceipt.IsVisible = PermisoBuscar(1, 1);
                btnConsultationBarcode.IsVisible = PermisoBuscar(1, 2);
                btnConsultationPallet.IsVisible = PermisoBuscar(1, 3);
                btnWarehouseReceipt.IsVisible = PermisoBuscar(1, 4);
                btnPhysicalInventory.IsVisible = PermisoBuscar(1, 5);
                btnInventoryConsultation.IsVisible = PermisoBuscar(1, 6);
                btnReceiptFinishedProduct.IsVisible = PermisoBuscar(1, 7);
                btnThermoformingReceipt.IsVisible = PermisoBuscar(1, 8);
                btnThermoformedDispatch.IsVisible = PermisoBuscar(1, 9);
                btnMaterialDelivery.IsVisible = PermisoBuscar(1, 10);
                btnConsultationBarcodeInventory.IsVisible = PermisoBuscar(1, 11);
                //BtonConsultaPalletEstado.IsVisible = PermisoBuscar(1, 12);
                //btnFacturacion.IsVisible = PermisoBuscar(1, 13);
                //validacion pallet bobina cem 14
                btnPhysicalInventoryPt.IsVisible = PermisoBuscar(1, 15);
                btnCreatePositonAndBlock.IsVisible = PermisoBuscar(1, 16);
                btnProfileAdministration.IsVisible = PermisoBuscar(1, 17);
            }
        }
    }

    private bool PermisoBuscar(int MenuOp, int OpcionBuscar)
    {
        bool EstadoPermiso = false;
        DataRow[] busca_Permiso;
        busca_Permiso = TblPermisosU.Select("IdMenuO ='" + MenuOp.ToString() + "' AND IdOpcion = '" + OpcionBuscar.ToString() + "'");
        if (busca_Permiso.Length > 0)
        {
            EstadoPermiso = Convert.ToBoolean(busca_Permiso[0]["Estado"]);
        }
        else
        {
            EstadoPermiso = false;
        }
        return EstadoPermiso;
    }

    private void btnProductionReceipt_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProductionReceipt());
    }

    private void btnConsultationBarcode_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ConsultationBarcode());
    }

    private void btnConsultationPallet_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ConsultationPallet());
    }

    private void btnWarehouseReceipt_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new WarehouseReceipt());
    }

    private void btnPhysicalInventory_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PhysicalInventory());
    }

    private void btnInventoryConsultation_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new InventoryConsultation());
    }

    private void btnReceiptFinishedProduct_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ReceiptFinishedProduct());
    }

    private void btnThermoformingReceipt_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ThermoformingReceipt());
    }

    private void btnThermoformedDispatch_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ThermoformedDispatch());
    }

    private void btnMaterialDelivery_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MaterialDelivery());
    }

    private void btnConsultationBarcodeInventory_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ConsultationBarcodeInventory());
    }

    private void btnConsultationStatusPallet_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ConsultationStatusPallet());
    }

    private void btnBilling_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new Billing());
    }

    private void btnApprovePalletCEMToPT_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ApprovePalletCEMToPT());
    }

    private void btnPhysicalInventoryPt_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SavePalletInformation());
    }

    private async void btnCreatePositonAndBlock_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new CreatePositionAndBlock());
        var profilePage = App.Current.Handler.MauiContext.Services.GetService<CreatePositionAndBlock>();
        await Navigation.PushAsync(profilePage);
    }

    private async void btnProfileAdministration_Clicked(object sender, EventArgs e)
    {
        var profilePage = App.Current.Handler.MauiContext.Services.GetService<ProfileAdministration>();
        await Navigation.PushAsync(profilePage);
    }
}