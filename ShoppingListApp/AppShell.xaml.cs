namespace ShoppingListApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.StoreFilterPage), typeof(Views.StoreFilterPage));
    }
}
