using ObenApp.App_Code;
using ObenApp.Controller;
using System.Net;
using System.Net.NetworkInformation;

namespace ObenApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }

    [Obsolete]
    protected override bool OnBackButtonPressed()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            bool exit = await DisplayAlert("Salir", "¿Desea salir de la aplicación?", "Sí", "No");
            if (exit)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        });
        return true;
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtPassword.Text) || txtPassword.Text == null)
            {
                if (txtPassword.IsPassword == true)
                {
                    txtPassword.IsPassword = false;
                    imgSeePassword.Source = "icon_visible90.png";
                    imgSeePassword.HeightRequest = 30;
                    txtPassword.CursorPosition = txtPassword.Text.Length;
                }
                else
                {
                    txtPassword.IsPassword = true;
                    imgSeePassword.Source = "icon_ocultar90.png";
                    txtPassword.CursorPosition = txtPassword.Text.Length;
                }
            }
        }
        catch (Exception)
        {

        }
    }

    private async void btnContinue_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUser.Text) && string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Ingrese un usuario y contraseña", "Ok");
            txtUser.Text = "";
            txtPassword.Text = "";
            txtPassword.IsPassword = true;
            return;
        }
        if (string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPassword.Text.ToString()))
        {
            await DisplayAlert("Error", "Ingrese un usuario", "Ok");
            txtUser.Text = "";
            txtPassword.Text = "";
            txtPassword.IsPassword = true;
            return;
        }
        if (!string.IsNullOrEmpty(txtUser.Text.ToString()) && string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Ingrese una contraseña", "Ok");
            txtUser.Text = "";
            txtPassword.Text = "";
            txtPassword.IsPassword = true;
            return;
        }
        if (!string.IsNullOrEmpty(txtUser.Text.ToString()))
        {
            clsGlobal.LoggedUser = clsUser.Login(txtUser.Text.ToString(), txtPassword.Text.ToString());

            if (CheckInternetConnection())
            {
                if (clsGlobal.LoggedUser.codsec > 0)
                {
                    await Navigation.PushAsync(new Menu());
                    txtUser.Text = "";
                    txtPassword.Text = "";
                    txtPassword.IsPassword = true;
                }
                else
                {
                    Task Page; await DisplayAlert("Incorrecto", "Usuario no encontrado", "Ok");
                    txtUser.Text = "";
                    txtPassword.Text = "";
                }
            }
        }
    }

    public bool CheckInternetConnection()
    {
        activityIndicator.IsRunning = true;
        activityIndicator.IsVisible = true;

        var current = Connectivity.NetworkAccess;

        if (current == NetworkAccess.Internet)
        {
            // Verificar si el dispositivo está conectado a la red específica
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in interfaces)
            {
                IPInterfaceProperties properties = networkInterface.GetIPProperties();
                UnicastIPAddressInformationCollection unicastAddresses = properties.UnicastAddresses;
                foreach (UnicastIPAddressInformation unicastAddress in unicastAddresses)
                {
                    // Verificar la dirección IP para determinar si está dentro del rango específico
                    string ipAddress = unicastAddress.Address.ToString();
                    if (IsWithinRange(ipAddress, BarcodeMPME.IpAddress.initIpAddress, BarcodeMPME.IpAddress.endIpAddress))
                    {
                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;

                        return true;
                    }
                }
            }
            //Navigation.PushAsync(BuildPageViewModel.ValidateBuildPage(2));
            return false;
        }
        else
        {
            //Navigation.PushAsync(BuildPageViewModel.ValidateBuildPage(1));
        }

        activityIndicator.IsRunning = false;
        activityIndicator.IsVisible = false;

        return false;
    }

    public bool IsWithinRange(string ipAddress, string startIpAddress, string endIpAddress)
    {
        IPAddress startIp = IPAddress.Parse(startIpAddress);
        IPAddress endIp = IPAddress.Parse(endIpAddress);
        IPAddress ip = IPAddress.Parse(ipAddress);

        byte[] startBytes = startIp.GetAddressBytes();
        byte[] endBytes = endIp.GetAddressBytes();
        byte[] bytes = ip.GetAddressBytes();

        bool withinRange = true;

        for (int i = 0; i < startBytes.Length; i++)
        {
            if (bytes[i] < startBytes[i] || bytes[i] > endBytes[i])
            {
                withinRange = false;
                break;
            }
        }

        return withinRange;
    }
}