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
                App.ViewModel.LoadData();
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

        private void takePhoto_imageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            PicButton.Opacity = 0.5;
        }
        private void takePhoto_imageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            PicButton.Opacity = 1.0;
        }
        private void takePhoto_imageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PicButton.Opacity = 0.5;
        }
        private void takePhoto_imageButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PicButton.Opacity = 1.0;
            //CurrentStatus = "Chooser";
            ctask.Show();
        }

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
    }
}