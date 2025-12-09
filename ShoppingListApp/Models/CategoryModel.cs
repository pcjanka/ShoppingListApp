using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ShoppingListApp.Models
{
    public class CategoryModel : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";

        private ObservableCollection<ProductModel> _products = new();
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private bool expanded = false;

        [XmlIgnore]
        public bool Expanded
        {
            get => expanded;
            set
            {
                if (expanded != value)
                {
                    expanded = value;
                    OnPropertyChanged(nameof(Expanded));
                }
            }
        }

        [field: XmlIgnore]
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
