using ShoppingListApp.Models;

namespace ShoppingListApp.Views;

public partial class CategoryView : ContentView
{
    public CategoryView() => InitializeComponent();

    private void Expand_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is CategoryModel c) c.Expanded = !c.Expanded;
    }
}
