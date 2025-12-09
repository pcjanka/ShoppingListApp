using ShoppingListApp.Models;

namespace ShoppingListApp.Views;

public partial class ProductView : ContentView
{
    public ProductView() => InitializeComponent();

    private void Minus_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ProductModel p && p.Quantity > 0) p.Quantity--;
    }

    private void Plus_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ProductModel p) p.Quantity++;
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ProductModel p)
        {
            var parent = AllCategories.Instance.Categories.FirstOrDefault(c => c.Products.Contains(p));
            if (parent != null)
            {
                parent.Products.Remove(p);
                AllCategories.Instance.Save();
            }
        }
    }
}
