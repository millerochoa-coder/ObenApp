using ObenApp.App_Code;
using ObenApp.Interfaces;
using ObenApp.Services;

namespace ObenApp.Views;

public partial class CreatePositionAndBlock : ContentPage
{
	ICreatePositionAndBlockService _createPositionAndBlockService;

	public CreatePositionAndBlock(ICreatePositionAndBlockService createPositionAndBlockService)
	{
		InitializeComponent();
		lblUsuario.Text = clsGlobal.LoggedUser.ToString();
		_createPositionAndBlockService = createPositionAndBlockService;
	}

    private void btnSave_Clicked(object sender, EventArgs e)
    {
		if(pkrCreationType.SelectedIndex < 0 || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtFloor.Text))
		{
			ShowMessage("Información", "Debe llenar los campos faltantes");
			return;
		}

		ShowMessage("Confirmación", _createPositionAndBlockService.SavePositionOrBlock(pkrCreationType.SelectedItem.ToString(), txtName.Text.ToUpper(), int.Parse(txtFloor.Text.ToString())));
    }

	private void ShowMessage(string title, string message)
	{
		DisplayAlert(title, message, "Ok");
	}
}