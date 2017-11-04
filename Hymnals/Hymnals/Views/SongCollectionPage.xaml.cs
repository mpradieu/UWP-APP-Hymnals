using System;

using Hymnals.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hymnals.Views
{
    public sealed partial class SongCollectionPage : Page
    {
        private SongCollectionViewModel ViewModel
        {
            get { return DataContext as SongCollectionViewModel; }
        }

        public SongCollectionPage()
        {
            InitializeComponent();
            Loaded += SongCollectionPage_Loaded;
        }

        private async void SongCollectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
