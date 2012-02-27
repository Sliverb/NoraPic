using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone;
using System.Windows.Media.Imaging;

namespace NoraPic
{
    public partial class MainPage : PhoneApplicationPage
    {
        //The camera chooser used to capture a picture.
        CameraCaptureTask ctask;

        BitmapImage CapturedImage;

       // private const String ApplicationStatus = "ProgramStatus";

        //String CurrentStatus;

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
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void ctask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                CapturedImage = new BitmapImage();
                CapturedImage.SetSource(e.ChosenPhoto);

                string imgSize = (e.ChosenPhoto.Length / 1024).ToString();
                string Height = CapturedImage.PixelHeight.ToString();
                string Width = CapturedImage.PixelWidth.ToString();

                MessageBox.Show("Size: " + imgSize + Environment.NewLine + "Dim: " + Height + "x" + Width);

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

        private void StoreImageItem(BitmapImage CapturedImage)
        {

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