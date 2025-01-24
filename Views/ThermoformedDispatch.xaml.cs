using ObenApp.App_Code;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ThermoformedDispatch : ContentPage
{
	public ThermoformedDispatch()
	{
		InitializeComponent();
		lblUsuario.Text = clsGlobal.LoggedUser.ToString();
	}
}