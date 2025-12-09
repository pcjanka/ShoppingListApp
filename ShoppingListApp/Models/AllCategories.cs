using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ShoppingListApp.Models
{
    public class AllCategories
    {
        private static AllCategories instance;
        public static AllCategories Instance => instance ??= new AllCategories();

        public ObservableCollection<CategoryModel> Categories { get; set; } = new();

        private readonly string filePath =
            Path.Combine(FileSystem.AppDataDirectory, "categories.xml");

        public class ProductDto
        {
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Store { get; set; }
            public bool Optional { get; set; }
            public bool Bought { get; set; }
            public int Quantity { get; set; }
        }

        public class CategoryDto
        {
            public string Name { get; set; }
            public List<ProductDto> Products { get; set; } = new();
        }

        public AllCategories()
        {
            Load();

            if (Categories.Count == 0)
            {
                Categories.Add(new CategoryModel { Name = "Fruits" });
                Save();
            }

            foreach (var c in Categories)
                c.Expanded = false;

            Subscribe();
            SortAllCategories();
        }

        private void Subscribe()
        {
            Categories.CollectionChanged += Categories_CollectionChanged;

            foreach (var c in Categories)
            {
                c.Products.CollectionChanged += Products_CollectionChanged;
                foreach (var p in c.Products)
                    p.PropertyChanged += Product_PropertyChanged;
            }
        }

        private void Categories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CategoryModel c in e.NewItems)
                {
                    c.Products.CollectionChanged += Products_CollectionChanged;
                    foreach (var p in c.Products)
                        p.PropertyChanged += Product_PropertyChanged;
                }
            }

            Save();
            MainThread.BeginInvokeOnMainThread(SortAllCategories);
        }

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ProductModel p in e.NewItems)
                    p.PropertyChanged += Product_PropertyChanged;

            if (e.OldItems != null)
                foreach (ProductModel p in e.OldItems)
                    p.PropertyChanged -= Product_PropertyChanged;

            Save();
            MainThread.BeginInvokeOnMainThread(SortAllCategories);
        }

        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();

            if (sender is ProductModel p && e.PropertyName == nameof(ProductModel.Bought))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MoveProduct(p);
                    Save();
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(SortAllCategories);
            }
        }

        private void MoveProduct(ProductModel p)
        {
            var cat = Categories.FirstOrDefault(c => c.Products.Contains(p));
            if (cat == null) return;

            cat.Products.CollectionChanged -= Products_CollectionChanged;

            try
            {
                cat.Products.Remove(p);

                if (p.Bought)
                {
                    cat.Products.Add(p);
                }
                else
                {
                    int index = cat.Products.TakeWhile(x => !x.Bought).Count();
                    cat.Products.Insert(index, p);
                }
            }
            finally
            {
                cat.Products.CollectionChanged += Products_CollectionChanged;
            }
        }

        private void SortAllCategories()
        {
            foreach (var cat in Categories)
            {
                var sorted = cat.Products
                    .OrderBy(x => x.Bought)
                    .ThenBy(x => x.Name)
                    .ToList();

                if (sorted.Count == cat.Products.Count &&
                    sorted.Zip(cat.Products, (a, b) => ReferenceEquals(a, b)).All(x => x))
                    continue;
                cat.Products.CollectionChanged -= Products_CollectionChanged;

                try
                {
                    var newCollection = new ObservableCollection<ProductModel>(sorted);
                    newCollection.CollectionChanged += Products_CollectionChanged;
                    cat.Products = newCollection;
                }
                finally
                {
                    cat.Products.CollectionChanged -= Products_CollectionChanged;
                    cat.Products.CollectionChanged += Products_CollectionChanged;
                }
            }
            Save();
        }



        public void AddCategory(string name)
        {
            var c = new CategoryModel { Name = name };
            c.Products.CollectionChanged += Products_CollectionChanged;
            Categories.Add(c);
            Save();
        }

        public void Save()
        {
            try
            {
                var dto = Categories.Select(cat => new CategoryDto
                {
                    Name = cat.Name,
                    Products = cat.Products.Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Unit = p.Unit,
                        Store = p.Store,
                        Optional = p.Optional,
                        Bought = p.Bought,
                        Quantity = p.Quantity
                    }).ToList()
                }).ToList();

                XmlSerializer ser = new(typeof(List<CategoryDto>));
                using FileStream fs = new(filePath, FileMode.Create);
                ser.Serialize(fs, dto);
            }
            catch { }
        }

        public void Load()
        {
            try
            {
                if (!File.Exists(filePath)) return;

                XmlSerializer ser = new(typeof(List<CategoryDto>));
                using FileStream fs = new(filePath, FileMode.Open);
                var dto = ser.Deserialize(fs) as List<CategoryDto>;
                if (dto == null) return;

                Categories.Clear();

                foreach (var cDto in dto)
                {
                    var cat = new CategoryModel { Name = cDto.Name };

                    foreach (var pDto in cDto.Products)
                    {
                        var p = new ProductModel
                        {
                            Name = pDto.Name,
                            Unit = pDto.Unit,
                            Store = pDto.Store,
                            Optional = pDto.Optional,
                            Bought = pDto.Bought,
                            Quantity = pDto.Quantity
                        };

                        cat.Products.Add(p);
                    }

                    Categories.Add(cat);
                }
            }
            catch { }
        }
    }
}
