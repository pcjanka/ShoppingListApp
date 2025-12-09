using ShoppingListApp.Models;

namespace ShoppingList.Views;

public partial class ShoppingListPage : ContentPage
{
    AllCategories allCategories;

    public ShoppingListPage()
    {
        InitializeComponent();
        allCategories = new AllCategories();
        BindingContext = allCategories;
    }

    private async void AddCategory_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Category", "Enter category name:");
        if (!string.IsNullOrWhiteSpace(name))
        {
            allCategories.AddCategory(name);
        }
    }
}
