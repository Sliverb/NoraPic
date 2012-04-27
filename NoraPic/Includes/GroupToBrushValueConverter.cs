// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections;

namespace NoraPic.Includes
{

    public class JumpListItemBackgroundConverter : DependencyObject, IValueConverter
    {
        private static readonly SolidColorBrush _phoneAccentBrush = (SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"];
        private static readonly SolidColorBrush _phoneChromeBrush = (SolidColorBrush)Application.Current.Resources["PhoneChromeBrush"];

        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IList group = value as IList;
            object result = null;

            if (group != null)
            {
                if (group.Count == 0)
                {
                    result = _phoneChromeBrush;
                }
                else
                {
                    result = _phoneAccentBrush;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class JumpListItemForegroundConverter : DependencyObject, IValueConverter
    {
        private static readonly SolidColorBrush _phoneForeGroundBrush = (SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"];
        private static readonly SolidColorBrush _phoneDisabledBrush = (SolidColorBrush)Application.Current.Resources["PhoneDisabledBrush"];

        
        

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IList group = value as IList;
            object result = null;

            if (group != null)
            {
                if (group.Count == 0)
                {
                    result = _phoneDisabledBrush;
                }
                else
                {
                    result = _phoneForeGroundBrush;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    
}
