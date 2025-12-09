using ShoppingListApp.Models;

namespace ShoppingListApp.Views;

public partial class CategoriesPage : ContentPage
{
    AllCategories data;

    public CategoriesPage()
    {
        InitializeComponent();
        data = AllCategories.Instance;
        BindingContext = data;
    }

    private async void AddCategory_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Category", "Enter category name:");

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "Category name is required.", "OK");
            return;
        }

        data.AddCategory(name);
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("New Product", "Enter product name:");
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "Product name is required.", "OK");
            return;
        }

        string unit = await DisplayPromptAsync("Unit", "Enter unit (e.g. pcs, kg, l):");
        if (string.IsNullOrWhiteSpace(unit))
        {
            await DisplayAlert("Error", "Unit is required.", "OK");
            return;
        }

        string qtyStr = await DisplayPromptAsync("Quantity", "Enter quantity (number):",
                                                 keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(qtyStr) || !int.TryParse(qtyStr, out int qty) || qty <= 0)
        {
            await DisplayAlert("Error", "Quantity must be a number greater than 0.", "OK");
            return;
        }

        string store = await DisplayPromptAsync("Store", "Enter store (Biedronka, Selgros):", placeholder: "Any");

        if (string.IsNullOrWhiteSpace(store))
            store = "Any"; 

        bool optional = (await DisplayActionSheet("Optional?", "No", "Yes")) == "Yes";

        string categoryName = await DisplayActionSheet(
            "Choose category:",
            "Cancel",
            null,
            data.Categories.Select(c => c.Name).ToArray()
        );

        if (categoryName == "Cancel" || string.IsNullOrWhiteSpace(categoryName))
        {
            await DisplayAlert("Error", "You must choose a category.", "OK");
            return;
        }

        ProductModel product = new ProductModel
        {
            Name = name,
            Unit = unit,
            Quantity = qty,
            Store = store,
            Optional = optional
        };

        CategoryModel category = data.Categories.First(c => c.Name == categoryName);
        category.Products.Add(product);
    }

    private async void OpenStoreFilter_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(StoreFilterPage));
    }
}
