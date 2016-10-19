using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BandProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrackingPage : Page
    {
        // declare variables
        public StoreWithDataContract store = new StoreWithDataContract();
        public List<Sport> sportList = new List<Sport>();
        private String FileName = "Sports";
        //---------------------------------------------------------------------------------//
        public TrackingPage()
        {
            this.InitializeComponent();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            sportList = store.LoadData(FileName);
            reloadList(sportList, listview);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
            else
              Frame.Navigate(typeof(MainPage));
        }

        public static void reloadList(List<Sport> sportList, ListView listview)
        {
            if (sportList.Count() != 0)
            {
                for (int i = 0; i < sportList.Count; i++)
                {
                    //ListViewItemModel.AddItem(sportList.ElementAt(i).name);
                    if(sportList[i].istrained)
                    listview.Items.Add(sportList.ElementAt(i));
                }
            }
        }
        
        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Sport selectedItem = (sender as ListView).SelectedItem as Sport;
            Frame.Navigate(typeof(GetUserMotionData), selectedItem);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Button remove = (sender as Button) as Button;
            Sport item = (sender as Button).DataContext as Sport;
            //remove.Content = "learned";
            //item.istrained = true;
            for (int i = 0; i < sportList.Count; i++)
            {
                if (sportList[i].name == item.name)
                    sportList[i].istrained = false;
            }
            store.SaveData(sportList, FileName);
            listview.Items.Remove(item);
        }
    }
}
