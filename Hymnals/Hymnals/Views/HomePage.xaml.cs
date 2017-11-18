using Hymnals.DataLayer;
using Hymnals.ViewModels;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Hymnals.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel
        {
            get { return DataContext as HomeViewModel; }
        }

        public HomePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            using (var db = new HymnalsContext())
            {
                Shelves.ItemsSource = db.Shelves.ToList();
            }
        }
    }
}
