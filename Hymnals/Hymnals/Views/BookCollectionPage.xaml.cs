using System;

using Hymnals.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hymnals.Views
{
    public sealed partial class BookCollectionPage : Page
    {
        private BookCollectionViewModel ViewModel
        {
            get { return DataContext as BookCollectionViewModel; }
        }

        public BookCollectionPage()
        {
            InitializeComponent();
            Loaded += BookCollectionPage_Loaded;
        }

        private async void BookCollectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
