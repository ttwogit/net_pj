using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace net_pj
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FoodOrderPage : Page
    {
        public ObservableCollection<UIElementData> ProductList { get; set; } = new ObservableCollection<UIElementData>();

        public FoodOrderPage()
        {
            this.InitializeComponent();
            this.Loaded += FoodOrderPage_Loaded;
        }

        private async void FoodOrderPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {

                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appx:///Assets/products.json"));
                string json = await FileIO.ReadTextAsync(file);

                var items = JsonConvert.DeserializeObject<List<UIElementData>>(json);
                ProductList.Clear();
                foreach (var product in items)
                {
                var card = new Item();
                card.BindData(product);
                ProductGrid.Items.Add(card);
            }
        }

    }
}