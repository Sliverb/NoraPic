﻿#pragma checksum "C:\Users\baolatun\Documents\Visual Studio 2010\Projects\NoraPic\NoraPic\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "82FFFD4507EFD9CF83E6309FB83C18FA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace NoraPic {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button PicButton;
        
        internal System.Windows.Controls.TextBlock AllPhotos;
        
        internal System.Windows.Controls.TextBlock EmptyFavouriteBlock;
        
        internal System.Windows.Controls.ItemsControl WrappedImages;
        
        internal System.Windows.Controls.TextBlock AllFavs;
        
        internal System.Windows.Controls.TextBlock EmptyRecentBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/NoraPic;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PicButton = ((System.Windows.Controls.Button)(this.FindName("PicButton")));
            this.AllPhotos = ((System.Windows.Controls.TextBlock)(this.FindName("AllPhotos")));
            this.EmptyFavouriteBlock = ((System.Windows.Controls.TextBlock)(this.FindName("EmptyFavouriteBlock")));
            this.WrappedImages = ((System.Windows.Controls.ItemsControl)(this.FindName("WrappedImages")));
            this.AllFavs = ((System.Windows.Controls.TextBlock)(this.FindName("AllFavs")));
            this.EmptyRecentBlock = ((System.Windows.Controls.TextBlock)(this.FindName("EmptyRecentBlock")));
        }
    }
}

