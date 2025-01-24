using ObenApp.Extensions;
using ObenApp.Interfaces;
using ObenApp.Models;
using System.Collections.ObjectModel;

namespace ObenApp.Views;

public partial class ProfileAdministration : ContentPage
{
	private readonly IProfileAdministrationService _profileAdministrationService;
    private ObservableCollection<Models.ProfileAdministration> UsersList = new ObservableCollection<Models.ProfileAdministration>();

    public ProfileAdministration(IProfileAdministrationService profileAdministrationService)
	{
		InitializeComponent();
        _profileAdministrationService = profileAdministrationService ?? throw new ArgumentNullException(nameof(profileAdministrationService));
        GetUsers();
    }

    private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
    {
		var user = UsersList.Where(e => e.Name.Contains(txtUser.Text, StringComparison.OrdinalIgnoreCase)).OrderBy(e => e.Name);
		cvUsers.ItemsSource = user.ToList();
    }

    private void cvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault();
        ((CollectionView)sender).SelectedItem = null;
        
        if (selectedItem != null)
        {
            var selectedUser = (Models.ProfileAdministration)selectedItem;
            FillSelectedUser(selectedUser.Name);
        }
    }

    private void FillSelectedUser(string user) => lblSelectedUser.Text = user;

    private void GetUsers() => UsersList = _profileAdministrationService.SearchUser();

    private void GetOptions() => pkrOptions.ItemsSource = _profileAdministrationService.GetOptions();
}