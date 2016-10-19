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
    public sealed partial class Samples : Page
    {
        // declare variables
        private StoreWithDataContract store = new StoreWithDataContract();
        private List<Sample> sampleList = new List<Sample>();
        private string FileName;
        private string sportname;
        private string gesturename;
        public Samples()
        {
            this.InitializeComponent();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Gesture ges = e.Parameter as Gesture;
            if (ges != null)
            {
                sportname = ges.nameSport;
                gesturename = ges.nameGesture;
                // store.deleteData(FileName);
                FileName = sportname + "-" + gesturename;
                sampleList = store.LoadDataSample(FileName);
                reloadListSample(sampleList, listview, sportname, gesturename);
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            //Frame rootFrame = Window.Current.Content as Frame;
            //if (rootFrame != null && rootFrame.CanGoBack)
            //{
            //    e.Handled = true;
            //    rootFrame.GoBack();
            //}
            Frame.Navigate(typeof(Gestures), sportname);
        }

        public void reloadListSample(List<Sample> List, ListView listview, string SportName, string GestureName)
        {
            if (List.Count() != 0)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    //ListViewItemModel.AddItem(sportList.ElementAt(i).name);
                   if(List.ElementAt(i).SportName == SportName && List.ElementAt(i).GestureName == GestureName)
                            listview.Items.Add(List.ElementAt(i));
                }
            }
        }
        public int nElement(List<Sample> List, string SportName, string GestureName)
        {
            int n = 0;
            for (int i = 0; i < List.Count; i++)
            {
                //ListViewItemModel.AddItem(sportList.ElementAt(i).name);
                if (List.ElementAt(i).SportName == SportName && List.ElementAt(i).GestureName == GestureName)
                    n++;
            }
            return n;
        }
        private void AddSample_Click(object sender, RoutedEventArgs e)
        {

            Sample item = new Sample();
            item.SportName = sportname;
            item.GestureName = gesturename;
            item.SampleName = SampleName.Text;
            // add to list<sport>
            sampleList.Add(item);
            // add item on listview
            listview.Items.Add(item);
            // store list<sport> for update
            store.SaveDataSample(sampleList, FileName);
            //GestureName.Text = "";
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sample obj = (sender as ListView).SelectedItem as Sample;
            Frame.Navigate(typeof(GetDataPage), obj);

        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {

            Sample myobject = (sender as Button).DataContext as Sample;
            listview.Items.Remove(myobject);

            for (int i = 0; i < sampleList.Count; i++)
            {
                if (sampleList.ElementAt(i).SampleName == myobject.SampleName)
                {
                    sampleList.RemoveAt(i);
                  //  AddStatus.Text = "clear";

                }
            }
            // re-store sample list in file
            store.SaveDataSample(sampleList, FileName);
            // remove points of this sample in list point file
            string DataSampleName = myobject.SportName + "-" + myobject.GestureName + ".txt";
            List<Point> list = new List<Point>();
            list = store.LoadDataPoint(DataSampleName);
           // AddSample.Content = list.Count;
            //int count = 0;
            for (int j = 0; j < list.Count; j++)
            {
                if (list.ElementAt(j).SampleName.CompareTo(myobject.SampleName) == 0)
                {
                    list.RemoveAt(j);
                    j--;
                   // count++;
                }
            }
           // AddStatus.Text = "clear" + count;
            // restore list point into file
            store.SaveDataPoint(list, DataSampleName);
        }

    }
}
