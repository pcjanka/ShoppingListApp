using ShoppingListApp.Models;

namespace ShoppingListApp.Views;

public partial class StoreFilterPage : ContentPage
{
    AllCategories data;

    public StoreFilterPage()
    {
        InitializeComponent();
        data = AllCategories.Instance;

        var stores = data.Categories.SelectMany(c => c.Products).Select(p => p.Store).Distinct().ToList();
        storePicker.ItemsSource = stores;
    }

    private void StorePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var store = storePicker.SelectedItem?.ToString();
        if (store == null) return;

        var filtered = data.Categories.SelectMany(c => c.Products).Where(p => p.Store == store).ToList();
        productsView.ItemsSource = filtered;
    }
}
