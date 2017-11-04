using System;

using Hymnals.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hymnals.Views
{
    public sealed partial class BookCollectionDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(BookCollectionDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public BookCollectionDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BookCollectionDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
