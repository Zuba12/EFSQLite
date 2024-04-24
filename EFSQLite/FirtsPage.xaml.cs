using EFSQLite.Models;

namespace EFSQLite;

public partial class FirtsPage : ContentPage
{
    public FirtsPage()
    {
        InitializeComponent();
    }

    private void Vytvor(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewPage1());
    }

    private void OFaktury(object sender, EventArgs e)
    {
        Navigation.PushAsync(new OFaktury());
    }

    private void PFaktury(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewPage2());
    }
}

