using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
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
    public sealed partial class GetDataPage : Page
    {
        private string FileName;
        private string sportname;
        private string gesturename;
        private string samplename;
       // private double time = 100;
        private StoreWithDataContract store = new StoreWithDataContract();
        private List<Point> ListPoint = new List<Point>();
        //private List<Point> AccList = new List<Point>();
        //private List<Point> GyList = new List<Point>();
        //private IBandClient bandClient;
        public GetDataPage()
        {
            this.InitializeComponent();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Sample sam = e.Parameter as Sample;
            if (sam != null)
            {
                sportname = sam.SportName;
                gesturename = sam.GestureName;
                samplename = sam.SampleName;
                FileName = sportname + "-" + gesturename + ".txt";
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
            //Gesture ges = new Gesture();
            //ges.nameGesture = gesturename;
            //ges.nameSport = sportname;
            //Frame.Navigate(typeof(Samples), ges);
        }
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Stop.IsEnabled = true;
            this.Start.IsEnabled = false;
            // load list point stored in previous collections
            ListPoint = store.LoadDataPoint(FileName);
            try
            {

                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                    this.StatusMessage.Text = "This app requires a Microsoft Band paired to your device. Also make sure that you have the latest firmware installed on your Band, as provided by the latest Microsoft Health app.";
                    return;
                }

                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {

                    int samplesReceivedAcc = 0; // the number of Accelerometer samples received
                    int samplesReceivedGyro = 0; // the number of Gyroscope samples received
                    Stopwatch myWatch = Stopwatch.StartNew();
                    // Subscribe to Accelerometer data.
                    bandClient.SensorManager.Accelerometer.ReadingChanged += (s, args) =>
                     bandClient.SensorManager.Gyroscope.ReadingChanged += (s1, args1) =>
                     {
                         Point point = new Point();
                         samplesReceivedAcc++;
                         if ((samplesReceivedAcc % 50) == 0)
                         {
                             // Only report occasional Accelerometer data
                             IBandAccelerometerReading readings = args.SensorReading;
                             CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                              {
                                  this.Accx.Text = readings.AccelerationX.ToString();
                                  this.Accy.Text = readings.AccelerationY.ToString();
                                  this.Accz.Text = readings.AccelerationZ.ToString();

                                  point.setAccelerometter(readings.AccelerationX, readings.AccelerationY, readings.AccelerationZ);
                              });


                         }
                         samplesReceivedGyro++;
                         if ((samplesReceivedGyro % 50) == 0)
                         {
                             // Only report occasional Gyroscope data
                             IBandGyroscopeReading readings = args1.SensorReading;
                             CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                              {
                                  this.Gyx.Text = readings.AccelerationX.ToString();
                                  this.Gyy.Text = readings.AccelerationY.ToString();
                                  this.Gyz.Text = readings.AccelerationZ.ToString();
                                  // myWatch.Restart();
                                  Clock.Text = myWatch.Elapsed.ToString();
                                  point.setGyroscope(readings.AccelerationX, readings.AccelerationY, readings.AccelerationZ);
                                  point.SampleName = samplename;
                                  ListPoint.Add(point);
                              });
                         }


                     };
                    await bandClient.SensorManager.Accelerometer.StartReadingsAsync();
                    await bandClient.SensorManager.Gyroscope.StartReadingsAsync();

                    // Receive sensor data for a while
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    // Stop the sensor subscriptions

                    await bandClient.SensorManager.Accelerometer.StopReadingsAsync();
                    await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
                }
            }
            catch (Exception ex)
            {
                this.StatusMessage.Text = ex.ToString();
            }
        }

        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            
            StatusMessage.Text = ListPoint.Count.ToString();
            store.SaveDataPoint(ListPoint, FileName);
            await WriteFileTxt(ListPoint, FileName);
            //ListPoint.Clear();
            //time = 0;
            //List<Point> a = new List<Point>();
            //a = await ReadFileTxt(FileName);
            StatusMessage.Text = ListPoint.Count.ToString() + "vectors is stored";
            this.Start.IsEnabled = true;
            //await WriteFileTxt(a, "test.txt");
        }
        private async Task WriteFileTxt(List<Point> list, string filename)
        {
            // create file
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.PicturesLibrary);
            try
            {
                StorageFile file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                if (file != null)
                {
                    try
                    {
                        // string userContent = InputTextBox.Text;
                        //StatusMessage.Text = list.Count.ToString();
                        if (list.Count !=0 )
                        {
                            String userContent="";
                            for(int i=0; i< list.Count; i++)
                            {
                                String line = String.Format("{0}, {1}, {2}, {3}, {4}, {5}", list.ElementAt(i).accX.ToString(),
                                    list.ElementAt(i).accY.ToString(), list.ElementAt(i).accZ.ToString(), list.ElementAt(i).gyX.ToString(), 
                                    list.ElementAt(i).gyY.ToString(), list.ElementAt(i).gyZ.ToString());
                                userContent += line + Environment.NewLine;
                            }
                            await FileIO.WriteTextAsync(file, userContent);
                           // rootPage.NotifyUser(String.Format("The following text was written to '{0}':{1}{2}", file.Name, Environment.NewLine, userContent), NotifyType.StatusMessage);
                        }
                        else
                        {
                           StatusMessage.Text="The list is empty, please collect something and then click 'Write' again.";
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        StatusMessage.Text = "File not exit";
                    }
                    catch (Exception ex)
                    {
                        // I/O errors are reported as exceptions.
                        StatusMessage.Text = ex.Message;
                        // rootPage.NotifyUser(String.Format("Error writing to '{0}': {1}", file.Name, ex.Message), NotifyType.ErrorMessage);
                    }
                }
                else
                {
                    StatusMessage.Text = "File not exit";
                }
                // rootPage.NotifyUser(String.Format("The file '{0}' was created.", rootPage.sampleFile.Name), NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                // I/O errors are reported as exceptions.
                // NotifyUser(String.Format("Error creating file '{0}': {1}", MainPage.filename, ex.Message), NotifyType.ErrorMessage);
                StatusMessage.Text = ex.Message;
            }
            // writefile

        }

        public async Task<List<Point>> ReadFileTxt(string filename)
        {
            //string fullPath = Path.GetFullPath(filename);
            string[] lines;
            string content;
            List<Point> model = new List<Point>();
            StorageFolder picturesLibrary = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.PicturesLibrary);
            StorageFile file = (StorageFile)await picturesLibrary.TryGetItemAsync(filename);
            if (file == null)
            {
                // If file doesn't exist, indicate users to use scenario 1
                // NotifyUser(String.Format("The file '{0}' does not exist. Use scenario one to create this file.", filename), NotifyType.ErrorMessage);
                StatusMessage.Text = "File not found";
            }
            else
            {
                try
                {
                    StatusMessage.Text = "reading";
                    content = await FileIO.ReadTextAsync(file);
                   // StatusMessage.Text = "1";
                    lines = content.Split('\n');
                   // StatusMessage.Text = "2";
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        string[] args = lines[i].Split(',',' ','\t');
                        Point newPoint = new Point();
                        //newPoint.accX = Double.Parse(args[0]);
                        //newPoint.accY = Double.Parse(args[1]);
                        //newPoint.accZ = Double.Parse(args[2]);
                        //newPoint.gyX = Double.Parse(args[3]);
                        //newPoint.gyY = Double.Parse(args[4]);
                        //newPoint.gyZ = Double.Parse(args[5]);
                        newPoint.setAccelerometter(double.Parse(args[0], CultureInfo.InvariantCulture), double.Parse(args[1], CultureInfo.InvariantCulture), double.Parse(args[2], CultureInfo.InvariantCulture));
                        newPoint.setGyroscope(double.Parse(args[3], CultureInfo.InvariantCulture), double.Parse(args[4], CultureInfo.InvariantCulture), double.Parse(args[5], CultureInfo.InvariantCulture));
                        model.Add(newPoint);
                    }
                  //  StatusMessage.Text = "3";
                }
                catch (FileNotFoundException)
                {
                    //rootPage.NotifyUserFileNotExist();
                    StatusMessage.Text = "File not found";
                }
                catch (Exception ex)
                {
                    // I/O errors are reported as exceptions.
                    //rootPage.NotifyUser(String.Format("Error reading from '{0}': {1}", file.Name, ex.Message), NotifyType.ErrorMessage);
                    StatusMessage.Text = ex.Message;
                }
                
            }
            return model;
        }
    }
}
