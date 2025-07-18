using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace net_pj
{
    public sealed partial class OrderPage : Page
    {
        public ObservableCollection<UIElementData> ProductList { get; set; } = new ObservableCollection<UIElementData>();

        public OrderPage()
        {
            this.InitializeComponent();
            this.Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            await LoadProductsByTypeAsync("GameTime");
        }

        private async Task LoadProductsByTypeAsync(string typeFilter)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/products.json"));
            var json = await FileIO.ReadTextAsync(file);
            var items = JsonConvert.DeserializeObject<ObservableCollection<UIElementData>>(json);

            ProductList.Clear();
            int count = 0;

            foreach (var product in items.Where(p => p.type?.ToString() == typeFilter))
            {
                count++;
                ProductList.Add(product);
            }   
        }
        private async void NavView_SelectionChanged(Windows.UI.Xaml.Controls.NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string selectedType = selectedItem.Tag?.ToString();
                await LoadProductsByTypeAsync(selectedType);
            }
        }
    }
}
