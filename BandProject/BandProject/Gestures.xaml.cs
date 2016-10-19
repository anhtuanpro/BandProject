using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Gestures : Page
    {
        // declare variables
        private StoreWithDataContract store = new StoreWithDataContract();
        private List<Gesture> gestureList = new List<Gesture>();
        private string FileName;
        private string sportname;
        public Gestures()
        {
            this.InitializeComponent();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string)
            {
                header.Text += " for " + e.Parameter.ToString();
                sportname = e.Parameter.ToString();
                FileName = sportname + "-" + "Gestures";
                gestureList = store.LoadDataGesture(FileName);
                reloadListGesture(gestureList, listview, sportname);
            }
        }
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            //Frame rootFrame = Window.Current.Content as Frame;
            //if (rootFrame != null && rootFrame.CanGoBack)
            //{
            //    e.Handled = true;
            //    rootFrame.GoBack();
            //}
            Frame.Navigate(typeof(RecordingPage));
        }

        public void reloadListGesture(List<Gesture> list, ListView listview, string SportName)
        {
            if (list.Count != 0)
               
            {
               // AddStatus.Text = SportName;
                for (int i = 0; i < list.Count; i++)
                {
                    //ListViewItemModel.AddItem(sportList.ElementAt(i).name);
                   if(list.ElementAt(i).nameSport == SportName)
                            listview.Items.Add(list.ElementAt(i));
                }
            }
        }
        private void AddGesture_Click(object sender, RoutedEventArgs e)
        {
            Gesture item = new Gesture();
            item.nameSport = sportname;
            item.nameGesture = GestureName.Text;
            if (checkGesture(gestureList, item))
                AddStatus.Text = "This gesture existed!";
            else
            {
                // add to list<sport>
                gestureList.Add(item);
                // add item on listview
                listview.Items.Add(item);
                // store list<sport> for update
                store.SaveDataGesture(gestureList, FileName);
                //AddStatus.Text = sportname + item.nameGesture;
            }
            GestureName.Text = "";
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gesture obj = (sender as ListView).SelectedItem as Gesture;
            Frame.Navigate(typeof(Samples), obj);

        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            
            Gesture myobject = (sender as Button).DataContext as Gesture;
            listview.Items.Remove(myobject);
            
            for (int i = 0; i < gestureList.Count; i++)
            {
                if (gestureList.ElementAt(i).nameGesture == myobject.nameGesture)
                {
                    gestureList.RemoveAt(i);

                }
            }
            //}
            store.SaveDataGesture(gestureList, FileName);
            // get 
            string samplefilename = myobject.nameSport + "-" + myobject.nameGesture;
            // delete list samples file and data samples file
            List<Sample> sam = store.LoadDataSample(samplefilename);
            for (int i = 0; i < sam.Count; i++)
            {
                string DataSampleName = sam[i].SportName + "-" + sam[i].GestureName + ".txt";
                string SampleFileName = sam[i].SportName + "-" + sam[i].GestureName;
                store.deleteData(DataSampleName);
                store.deleteData(SampleFileName);
            }
        }

        private void GestureName_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
           // AddStatus.Text = "";
        }

        private bool checkGesture(List<Gesture> list, Gesture item)
        {
            for (int i=0; i< list.Count; i++)
            {
                if (list.ElementAt(i).nameGesture == item.nameGesture && list.ElementAt(i).nameSport == item.nameSport)
                    return true;
            }
            return false;
        }
    }

}
