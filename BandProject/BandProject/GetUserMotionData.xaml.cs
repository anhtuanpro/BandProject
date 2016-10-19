using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
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
    public sealed partial class GetUserMotionData : Page
    {
        private string GestureFileName;
        private string sportname;
        private StoreWithDataContract store = new StoreWithDataContract();
        private List<string> ModelFileName = new List<string>();
        private List<Point> unknown = new List<Point>();
        private List<Point> unknownNormalized = new List<Point>();
        private List<List<Point>> models = new List<List<Point>>();
        private List<Gesture> gestureList = new List<Gesture>();
        //
        private SpeechSynthesizer synthesizer;
        //private IBandClient bandClient;
        public GetUserMotionData()
        {
            this.InitializeComponent();
            synthesizer = new SpeechSynthesizer();
            // enable back button to come back privious page
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Sport item = e.Parameter as Sport;
            if (item != null)
            {
                sportname = item.name;
                GestureFileName = sportname + "-" + "Gestures";
                gestureList = store.LoadDataGesture(GestureFileName);
                for (int i = 0; i < gestureList.Count; i++)
                {
                    // name of file contaning list point of samples
                    string filename = sportname + "-" + gestureList[i].nameGesture + ".txt";
                    ModelFileName.Add(filename);
                    report.Text += filename;
                }
                // read model data from txt 
                for (int j = 0; j < ModelFileName.Count; j++)
                {
                    List<Point> listpoint = new List<Point>();
                    try
                    {
                        listpoint = await ReadFileTxt(ModelFileName[j]);
                    }
                    catch (Exception ex)
                    {
                        StatusMessage.Text = ex.ToString();
                    }
                    models.Add((listpoint));
                }
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
            //Frame.Navigate(typeof(TrackingPage));
        }
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Stop.IsEnabled = true;
            this.Start.IsEnabled = false;
            StatusMessage.Text = "";
            // load list point stored in previous collections
            //ListPoint = store.LoadDataPoint(FileName);
            unknown.Clear();
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
                                 Clock.Text = myWatch.Elapsed.ToString();
                                 point.setGyroscope(readings.AccelerationX, readings.AccelerationY, readings.AccelerationZ);
                                // point.SampleName = samplename;
                                 unknown.Add(point);
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

        //public List<Point> Normalise(List<Point> inputList)
        //{
        //    List<Point> newList = new List<Point>();
        //    double maxAccX = -10E+2;
        //    double minAccX = 10E+10;
        //    double maxAccY = -10E+2;
        //    double minAccY = 10E+10;
        //    double maxAccZ = -10E+2;
        //    double minAccZ = 10E+10;
        //    double maxGyX = -10E+2;
        //    double minGyX = 10E+10;
        //    double maxGyY = -10E+2;
        //    double minGyY = 10E+10;
        //    double maxGyZ = -10E+2;
        //    double minGyZ = 10E+10;

        //    for (int i = 0; i < inputList.Count; i++)
        //    {
        //        //Accelerometer data
        //        if (maxAccX < inputList[i].accX)
        //            maxAccX = inputList[i].accX;
        //        if (minAccX > inputList[i].accX)
        //            minAccX = inputList[i].accX;
        //        if (maxAccY < inputList[i].accY)
        //            maxAccY = inputList[i].accY;
        //        if (minAccY > inputList[i].accY)
        //            minAccY = inputList[i].accY;
        //        if (maxAccZ < inputList[i].accZ)
        //            maxAccZ = inputList[i].accZ;
        //        if (minAccZ > inputList[i].accZ)
        //            minAccZ = inputList[i].accZ;
        //        //Gyroscope data
        //        if (maxGyX < inputList[i].gyX)
        //            maxGyX = inputList[i].gyX;
        //        if (minGyX > inputList[i].gyX)
        //            minGyX = inputList[i].gyX;
        //        if (maxGyY < inputList[i].gyY)
        //            maxGyY = inputList[i].gyY;
        //        if (minGyY > inputList[i].gyY)
        //            minGyY = inputList[i].gyY;
        //        if (maxGyZ < inputList[i].gyZ)
        //            maxGyZ = inputList[i].gyZ;
        //        if (minGyZ > inputList[i].gyZ)
        //            minGyZ = inputList[i].gyZ;
        //    }

        //    for (int i = 0; i < inputList.Count; i++)
        //    {
        //        double newAccX = (inputList[i].accX - minAccX) * 10 / (maxAccX - minAccX);
        //        double newAccY = (inputList[i].accY - minAccY) * 10 / (maxAccY - minAccY);
        //        double newAccZ = (inputList[i].accZ - minAccZ) * 10 / (maxAccZ - minAccZ);
        //        double newGyX = (inputList[i].gyX - minGyX) * 10 / (maxGyX - minGyX);
        //        double newGyY = (inputList[i].gyY - minGyY) * 10 / (maxGyY - minGyY);
        //        double newGyZ = (inputList[i].gyZ - minGyZ) * 10 / (maxGyZ - minGyZ);
        //        //Set Point with 6 parameters
        //        Point newPoint = new Point();
        //        newPoint.setAccelerometter(newAccX, newAccY, newAccZ);
        //        newPoint.setGyroscope(newGyX, newGyY, newGyZ);
        //        newList.Add(newPoint);
        //    }

        //    return newList;
        //}

        public double GetDifference(List<Point> inputList, List<Point> sampleList)
        {
            double sum = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                double min = 10E+10;
                for (int k = 0; k < sampleList.Count; k++)
                {
                    double distance =
                        (inputList[i].accX - sampleList[k].accX) * (inputList[i].accX - sampleList[k].accX) +
                        (inputList[i].accY - sampleList[k].accY) * (inputList[i].accY - sampleList[k].accY) +
                        (inputList[i].accZ - sampleList[k].accZ) * (inputList[i].accZ - sampleList[k].accZ) +
                        (inputList[i].gyX - sampleList[k].gyX) * (inputList[i].gyX - sampleList[k].gyX) +
                        (inputList[i].gyY - sampleList[k].gyY) * (inputList[i].gyY - sampleList[k].gyY) +
                        (inputList[i].gyZ - sampleList[k].gyZ) * (inputList[i].gyZ - sampleList[k].gyZ);
                    if (min > distance)
                        min = distance;
                }
                sum += min;

            }
            // Console.WriteLine(sum / drawing.Count);
            //report.Text += sum.ToString();
            return sum / inputList.Count;
        }

        private void Recoginize()
        {
           // unknownNormalized = Normalise(unknown);
            double min;
            int position = 0;
            // initial value of min
            min = GetDifference(unknown, models[0]);
            for (int i = 0; i < models.Count; i++)
            {
                double diff = GetDifference(unknown, models[i]);
                if (min > diff)
                {
                    min = diff;
                    position = i;
                }
                //StatusMessage.Text += diff.ToString() + "|";
                
            }
            string GestureName = gestureList[position].nameGesture;
            StatusMessage.Text += "That is "+ GestureName;
            Speak(GestureName);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.Start.IsEnabled = true;
            Recoginize();
        }
       

        public async Task<List<Point>> ReadFileTxt(string filename)
        {
            string fullPath = Path.GetFullPath(filename);
            string[] lines;
            string content;
            List<Point> model = new List<Point>();
            StorageFolder documentsLibrary = await KnownFolders.GetFolderForUserAsync(null /* current user */, KnownFolderId.DocumentsLibrary);
            StorageFile file = (StorageFile)await documentsLibrary.TryGetItemAsync(filename);
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
                        string[] args = lines[i].Split('\t', ' ', ',');
                        Point newPoint = new Point();
                        newPoint.accX = Double.Parse(args[0]);
                        newPoint.accY = Double.Parse(args[1]);
                        newPoint.accZ = Double.Parse(args[2]);
                        newPoint.gyX = Double.Parse(args[3]);
                        newPoint.gyY = Double.Parse(args[4]);
                        newPoint.gyZ = Double.Parse(args[5]);
                        model.Add(newPoint);
                    }
                      
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
                    StatusMessage.Text = model.Count.ToString();
                }

            }
            return model;
        }
        ///
        private async void Speak(string text)
        {
            if (media.CurrentState.Equals(MediaElementState.Playing))
            {
                media.Stop();
            }
            else
            {
                if (!String.IsNullOrEmpty(text))
                {
                    try
                    {
                        SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(text);

                        // Set the source and start playing the synthesized 
                        // audio stream.
                        media.AutoPlay = true;
                        media.SetSource(synthesisStream, synthesisStream.ContentType);
                        media.Play();
                    }
                    catch (System.IO.FileNotFoundException)
                    {
                        // If media player components are unavailable, 
                        // (eg, using a N SKU of windows), we won't be able 
                        // to start media playback. Handle this gracefully 
                        var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components unavailable");
                        await messageDialog.ShowAsync();
                    }
                    catch (Exception)
                    {
                        // If the text is unable to be synthesized, throw 
                        // an error message to the user.
                        media.AutoPlay = false;
                        var messageDialog = new Windows.UI.Popups.MessageDialog("Unable to synthesize text");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }
    }
}
