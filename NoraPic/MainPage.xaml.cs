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
using ExifLib;
using System.Windows.Navigation;
using NoraPic.Includes;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NoraPic
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Final resolution values
        const int img43Width = 718;
        const int img43Height = 538;
        const int img169Width = 717;
        const int img169Height = 403;

        const int thumb43Width = 160;
        const int thumb43Height = 120;
        const int thumb169Width = 160;
        const int thumb169Height = 90;

        //The camera chooser used to capture a picture.
        CameraCaptureTask ctask;

        Stream capturedImage;
        Stream imgThumbnail;
        int imgAngle;
        int wid = 0;
        int hei = 0;
         

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
            if (DataContext == null)
                DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //Create new event handler for capturing a photo
            ctask = new CameraCaptureTask();
            ctask.Completed += new EventHandler<PhotoResult>(ctask_Completed);

            //CurrentStatus = "Start";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Show some context when no notes available
            showHideTips(Settings.Favourites.Value.Count);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Load all images from the DB to memory (observable collection)
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadCollectionsFromDatabase();
            }
            
            // Load all the Favourite images to an observable collection
            if (!App.ViewModel.IsAllFavLoaded)
            {
                App.ViewModel.LoadAllFavs();
            }

            // Load the favourites in the main panorama
            if (!App.ViewModel.IsMainFavLoaded)
            {
                App.ViewModel.LoadMainFavs();
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

        #region Helpers

        void ctask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                e.ChosenPhoto.Position = 0;
                JpegInfo imgInfo = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);

                switch (imgInfo.Orientation)
                {
                    case ExifOrientation.TopLeft:
                    case ExifOrientation.Undefined:
                        imgAngle = 0;
                        break;
                    case ExifOrientation.TopRight:
                        imgAngle = 90;
                        break;
                    case ExifOrientation.BottomRight:
                        imgAngle = 180;
                        break;
                    case ExifOrientation.BottomLeft:
                        imgAngle = 270;
                        break;
                }

                if (imgAngle > 0d)
                {
                    Stream resizedImage = ResizeStream(e.ChosenPhoto);
                    Stream resizedThumb = imgThumbnail;
                    capturedImage = RotateStream(resizedImage, imgAngle);
                    imgThumbnail = RotateStream(resizedThumb, imgAngle);
                }
                else
                {
                    capturedImage = ResizeStream(e.ChosenPhoto);
                }

                string imgSize = (capturedImage.Length / 1024).ToString();
                long memoryRemaining = (long) DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage");
                MessageBox.Show("Mem: " + (memoryRemaining/1048576) + Environment.NewLine + "Size: " + imgSize + "kb" + Environment.NewLine + "Dim: " + wid + "x" + hei);
                StoreImageItem();
                showHideTips(App.ViewModel.MainFavImages.Count);
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

            capturedImage = null;
            imgThumbnail = null;
        }

        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);        

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }

            wid = wbTarget.PixelWidth;
            hei = wbTarget.PixelHeight;
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
        }

        private Stream ResizeStream(Stream stream)
        {
            stream.Position = 0;
           
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);
            
            double height = wbSource.PixelHeight;
            double width = wbSource.PixelWidth;

            int aspectRatio = (int) (Math.Round((width / height), 0));

            MessageBox.Show("Aspect Ratio: " + aspectRatio);

            int imgWidth;
            int imgHeight;
            int thumbWidth;
            int thumbHeight;

            if (aspectRatio == 1)
            {
                imgWidth = img43Width;
                imgHeight = img43Height;  
                thumbWidth = thumb43Width;
                thumbHeight = thumb43Height;
            }
            else
            {
                imgWidth = img169Width;
                imgHeight = img169Height;
                thumbWidth = thumb169Width;
                thumbHeight = thumb169Height;
            }

            imgThumbnail = new MemoryStream();
            wbSource.SaveJpeg(imgThumbnail, thumbWidth, thumbHeight, 0, 85);

            MemoryStream targetStream = new MemoryStream();
            wbSource.SaveJpeg(targetStream, imgWidth, imgHeight, 0, 85);
            return targetStream;
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
            TempImageItem.SaveImage(capturedImage, imgThumbnail);

            App.ViewModel.AddImageItem(TempImageItem);
            App.ViewModel.AddFavImage(TempImageItem.thumbPath);
        }

        private void showHideTips(int listCount)
        {
            if (listCount == 0)
            {
                EmptyRecentBlock.Visibility = System.Windows.Visibility.Visible;
                EmptyFavouriteBlock.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                EmptyRecentBlock.Visibility = System.Windows.Visibility.Collapsed;
                EmptyFavouriteBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion

        #region Event Handlers


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            ctask.Show();
        }

        private void AllPhotos_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Pictures.xaml", UriKind.Relative));
        }


        private void AllFavourite_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Pictures.xaml?id=2", UriKind.Relative));
        }

        #endregion

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
       
        
        if (imgInfo.ThumbnailData != null)
        {
            imgThumbnail = new MemoryStream(imgInfo.ThumbnailData);
            BitmapImage omg = new BitmapImage();
            omg.SetSource(imgThumbnail);
            MessageBox.Show("Thumbnail Size: " + imgInfo.ThumbnailSize / 1024 + Environment.NewLine + "Dim: " + omg.PixelWidth + "x" + omg.PixelHeight);
        }
        else
        {
            // create thumbnail
        }
       
        **/
    }
}