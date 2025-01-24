using Google.Apis.Sheets.v4;
using ObenApp.App_Code;
using ObenApp.Interfaces;
using ObenApp.Models;
using ObenApp.Services;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Data;

namespace ObenApp.Views;

public partial class SavePalletInformation : ContentPage, IUploadGoogleSheet
{
	public string SpreadsheetId { get; set; }
	public string SheetName { get; set; }
	public SheetsService sheetsService { get; set; }

    private readonly IAudioManager _audioManager;
    IUploadGoogleSheet uploadGoogleSheet = new SavePalletInformation();

    public SavePalletInformation()
	{
		InitializeComponent();
        lblUsuario.Text = clsGlobal.LoggedUser.ToString();
		LoadData();
		PhysicalInventoryListing = new ObservableCollection<PhysicalInventoryListing>();
    }

	public SavePalletInformation(IAudioManager audioManager)
	{
		_audioManager = audioManager;
    }

	public ObservableCollection<PhysicalInventoryListing> PhysicalInventoryListing { get; set; }
	DataTable TblTypesValues = new DataTable();

    private void OnBarcodeScanned(string barcode)
	{
		if (IsValidBarcode(barcode))
		{
			var successSound = _audioManager.CreatePlayer("resources/raw/yamete-kudasai.mp3");
			successSound.Play();
		}
		else
		{
			var errorSound = _audioManager.CreatePlayer("resources/raw/yamete-kudasai.mp3");
			errorSound.Play();
		}
	}

	private bool IsValidBarcode(string barcode) => (!string.IsNullOrEmpty(barcode));

	private void LoadData()
	{
		TblTypesValues = CatalogAccess.EjecutarConsultaDataTable("sp_TypesValues_Inventory");

		if(TblTypesValues.Rows.Count > 0)
		{
			string? Value;

			foreach (DataRow T in TblTypesValues.Rows)
			{
				Value = T["Types"].ToString();
				pkrTypeValues.Items.Add(Value);

			}
		}
	}

	private async Task SaveItemsAsync(ObservableCollection<PhysicalInventoryListing> Collection)
	{
        foreach(PhysicalInventoryListing Item in Collection)
        {
			string response = CatalogAccess.EjecutarComandoEscalar("sp_SavePalletInformationInventory",
				new Parametro("@NumberCode", Item.Code),
                new Parametro("@TypeValue", Item.Value),
                new Parametro("@FKUser", clsGlobal.LoggedUser.codsec));

			if (response.Contains("Error"))
			{
                await DisplayAlert("Error", response, "Ok");
            }
        }
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
	{
		try
		{
            if (swWarningManualTake.IsToggled)
                return;

			if (txtBarcodePallet.Text.Length < 7 && (pkrTypeValues.SelectedItem.ToString() == "Pallet" || pkrTypeValues.SelectedItem.ToString() == "Bobina"))
			{
				DisplayAlert("Error", "Hay menos de 7 caracteres para leer", "Ok");
				txtBarcodePallet.Text = string.Empty;
				return;
			}
			else
			{
                DisplayAlert("Error", "Debe elegir una opción de valor para guardar la información", "Ok");
				txtBarcodePallet.Text = string.Empty;
            }

            PhysicalInventoryListing.Add(new PhysicalInventoryListing { Code = txtBarcodePallet.Text, Value = pkrTypeValues.SelectedItem.ToString() });

            cvPhysicalInventoryListing.ItemsSource = PhysicalInventoryListing;
        }
		catch (Exception ex)
		{
			DisplayAlert("Error", "Ha ocurrido una excepción " + ex.Message, "Ok");
		}
	}

    private async void btnSavePallet_Clicked(object sender, EventArgs e)
    {
		try
		{
			if (PhysicalInventoryListing.Count <= 0)
				await DisplayAlert("Error", "La lista esta vacía, debe capturar al menos un código de barra", "Ok");

			await SaveItemsAsync(PhysicalInventoryListing);
        }
		catch (Exception ex)
		{
			await DisplayAlert("Error", "Ha ocurrido una excepción: " + ex.Message, "Ok");
		}
    }

	private async void UploadDataGoogleSheet()
	{
        string[,] datos = new string[,]
		{
			{ "Nombre", "Edad", "Ciudad" },
			{ "Juan", "25", "Madrid" },
			{ "Ana", "30", "Barcelona" }
		};

        uploadGoogleSheet.SpreadsheetId = SpreadsheetId;
		uploadGoogleSheet.SheetName = SheetName;
		uploadGoogleSheet.sheetsService = sheetsService;

		uploadGoogleSheet.InitializeUploadGoogleSheet();
		await uploadGoogleSheet.WriteDataAsync(datos);
	}
}