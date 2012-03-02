using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using NoraPic.Model;
using System.Device.Location;
using Microsoft.Phone;
using System.IO;
using Microsoft.Phone.Info;

namespace NoraPic
{
    public partial class MainPage : PhoneApplicationPage
    {
        //The camera chooser used to capture a picture.
        CameraCaptureTask ctask;

        WriteableBitmap CapturedImage;

        // In this case,since we're not working with a device, I'll just
        // set a default value.  If we cannot get the current location,
        // then we'll default to Redmond, WA.
        double latitude;
        double longitude;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //Create new event handler for capturing a photo
            ctask = new CameraCaptureTask();
            ctask.Completed += new EventHandler<PhotoResult>(ctask_Completed);

            //CurrentStatus = "Start";
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadCollectionsFromDatabase();
            }

            // When the page is instantiated, we'll begin the process
            // of acquiring the physical location of the user to 
            // add to the note's file name.  This is async, requiring
            // us to implement a __Completed event.
            try
            {
                GeoCoordinateWatcher myWatcher = new GeoCoordinateWatcher();

                var myPosition = myWatcher.Position;

                if (!myPosition.Location.IsUnknown)
                {
                    latitude = myPosition.Location.Latitude;
                    longitude = myPosition.Location.Longitude;
                }
         
            }
            catch
            {
                // Ignore ... for now
            } 
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void ctask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //CapturedImage = new WriteableBitmap();

                CapturedImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                //CapturedImage.Resize();

                string imgSize = (e.ChosenPhoto.Length / 1024).ToString();
                string Height = CapturedImage.PixelHeight.ToString();
                string Width = CapturedImage.PixelWidth.ToString();
                long memoryRemaining = (long) DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
                MessageBox.Show("Mem: " + memoryRemaining + "Size: " + imgSize + Environment.NewLine + "Dim: " + Height + "x" + Width);
                StoreImageItem();
                //CapturedImage = PictureDecoder.DecodeJpeg(e.ChosenPhoto);
                //myImage.Source = bmp;
                //myImage.Stretch = Stretch.Uniform;
                // swap UI element states
                //savePhotoButton.IsEnabled = true;
                //statusText.Text = "";
            }
            else
            {
                //savePhotoButton.IsEnabled = false;
                //statusText.Text = "Task Result Error: " + e.TaskResult.ToString();
                MessageBox.Show("Error");
            }

            CapturedImage = null;
        }

        private void PicButton_MouseEnter(object sender, MouseEventArgs e)
        {
            PicButton.Opacity = 0.5;
        }

        private void PicButton_MouseLeave(object sender, MouseEventArgs e)
        {
            PicButton.Opacity = 1.0;
        }

        private void PicButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PicButton.Opacity = 0.5;
        }

        private void PicButton_Click(object sender, RoutedEventArgs e)
        {
            PicButton.Opacity = 1.0;
            //CurrentStatus = "Chooser";
            ctask.Show();
        }

        private void Test_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("HI");
        }

        private void StoreImageItem()
        {
            ImageItem TempImageItem = new ImageItem();
            TempImageItem.ImageName = Guid.NewGuid().ToString() + ".jpg";
            TempImageItem.IsSync = false;
            TempImageItem.IsCached = true;
            TempImageItem.IsFav = true;
            TempImageItem.DateTaken = DateTime.Now;
            TempImageItem.Latitude = latitude;
            TempImageItem.Longitude = longitude;
            TempImageItem.Comments = "Test Comment";
            TempImageItem.SaveImage(CapturedImage);

            App.ViewModel.AddImageItem(TempImageItem);

        }

        /**
        private void Clearall_Event(object sender, RoutedEventArgs e)
        {
            if (Clearall.Content.Equals("Clear"))
            {
                WrappedImages.ItemsSource = null;
                WrappedImages.Width = 0;
                Clearall.Content = "Set";
            }
            else
            {
                WrappedImages.ItemsSource = App.ViewModel.Items;
                WrappedImages.UpdateLayout();
                Clearall.Content = "Clear";
            }

        }
        **/
    }
}