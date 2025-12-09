using System.ComponentModel;
using System.Xml.Serialization;

namespace ShoppingListApp.Models
{
    public class ProductModel : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "pcs";
        public string Store { get; set; } = "Any";
        public bool Optional { get; set; } = false;

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        private bool bought = false;
        public bool Bought
        {
            get => bought;
            set
            {
                if (bought != value)
                {
                    bought = value;
                    OnPropertyChanged(nameof(Bought));
                }
            }
        }

        [field: XmlIgnore]
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
