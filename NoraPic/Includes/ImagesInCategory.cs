using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using NoraPic.Model;
using System.ComponentModel;

namespace NoraPic.Includes
{
    public class ImagesInCategory : ObservableCollection<ImageItem> , INotifyPropertyChanged
    {
        public ImagesInCategory(string category)
        {
            Key = category;
        }

        private string _Key;
        public string Key
        {
            get { return _Key; }
            set
            {
                if (_Key != value)
                {
                    _Key = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
