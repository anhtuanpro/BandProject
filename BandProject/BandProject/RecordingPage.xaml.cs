using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class RecordingPage : Page
    {
        // declare variables
        public StoreWithDataContract store = new StoreWithDataContract();
        public List<Sport> sportList = new List<Sport>();
        private String FileName = "Sports";
   
        public RecordingPage()
        {
            this.InitializeComponent();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
           // store.deleteData(FileName);
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
            //Frame.Navigate(typeof(MainPage));
        }

        public static void reloadList(List<Sport> sportList, ListView listview)
        {
            if (sportList.Count() != 0)
            {
                for (int i = 0; i < sportList.Count; i++)
                {
                    //ListViewItemModel.AddItem(sportList.ElementAt(i).name);
                    if(!sportList[i].istrained)
                            listview.Items.Add(sportList.ElementAt(i));
                }
            }
        }
        private void AddSport_Click(object sender, RoutedEventArgs e)
        {
           
            Sport newSport = new Sport();
            newSport.name = SportName.Text;
            if (checkSport(sportList, newSport))
                AddStatus.Text = "This sport existed";
            else
            {
                // add to list<sport>
                sportList.Add(newSport);
                // add item on listview
                listview.Items.Add(newSport);
                // store list<sport> for update
                store.SaveData(sportList, FileName);
            }
            SportName.Text = "";
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Sport selectedItem = (sender as ListView).SelectedItem as Sport;
            if (!selectedItem.istrained)
            {
                Frame.Navigate(typeof(Gestures), selectedItem.name);
            }
            else
            {
               // RecordingPage.NotifyUser("This sport was learned");
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            Sport myobject = (sender as Button).DataContext as Sport;
            listview.Items.Remove(myobject);
            
            for (int i=0; i< sportList.Count; i++)
                {
                    if(sportList.ElementAt(i).name == myobject.name)
                    {
                        sportList.RemoveAt(i);
                        
                    }
                }
            //}
            store.SaveData(sportList, FileName);
           // get name of gestures file
           string gesturefilename= myobject.name + "-" + "Gestures";
            // delete list samples file and data samples file
            List<Gesture> ges = store.LoadDataGesture(gesturefilename);
            for(int i=0; i< ges.Count; i++)
            {
                string DataSampleName = ges[i].nameSport + "-" + ges[i].nameGesture + ".txt";
                string SampleFileName = ges[i].nameSport + "-" + ges[i].nameGesture;
                store.deleteData(DataSampleName);
                store.deleteData(SampleFileName);
            }
            // delete gestures file
            store.deleteData(gesturefilename);
        }
        // check whether or not the new name of sport is duplicated
        private bool checkSport(List<Sport> list, Sport item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.ElementAt(i).name == item.name)
                    return true;
            }
            return false;
        }

        private void learn_Click(object sender, RoutedEventArgs e)
        {
            Button learn = (sender as Button) as Button;
            Sport item = (sender as Button).DataContext as Sport;
            learn.Content = "learned";
            item.istrained = true;
            for(int i=0; i< sportList.Count; i++)
            {
                if (sportList[i].name == item.name)
                    sportList[i].istrained = true;
            }
            store.SaveData(sportList, FileName);
        }
    }
}
