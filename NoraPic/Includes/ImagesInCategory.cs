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

namespace NoraPic.Includes
{
    public class ImagesInCategory : ObservableCollection<ImageItem>
    {
        public ImagesInCategory(string category)
        {
            Key = category;
        }

        public string Key { get; set; }

    }
}
