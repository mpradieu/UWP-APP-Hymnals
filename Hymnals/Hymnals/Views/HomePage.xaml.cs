using System;

using Hymnals.ViewModels;

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
    }
}
