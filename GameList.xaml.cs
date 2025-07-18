using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace net_pj
{

    public sealed partial class GameList : Page
    {
        public ObservableCollection<GameElementData> ProductList { get; set; } = new ObservableCollection<GameElementData>();

        public GameList()
        {
            this.InitializeComponent();
            this.Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            await LoadProductsByTypeAsync("");
        }
        private async Task LoadProductsByTypeAsync(string typeFilter)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/game.json"));
            var json = await FileIO.ReadTextAsync(file);
            var items = JsonConvert.DeserializeObject<ObservableCollection<GameElementData>>(json);

            ProductList.Clear();
            int count = 0;

            foreach (var product in items.Where(p => p.name?.IndexOf(typeFilter, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                count++;
                ProductList.Add(product);
            }
        }
        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            await LoadProductsByTypeAsync(SearchBox.Text);
        }
    }
}
