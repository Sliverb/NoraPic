// Copyright (c) Adam Nathan.  All rights reserved.
// Modified by Bayo Olatunji.
using System.Windows.Media;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;
using AdvSetting;

namespace NoraPic.Includes
{
    public static class Settings
    {
        // The App data
        public static readonly Setting<ObservableCollection<string>> Favourites =
            new Setting<ObservableCollection<string>>("Favourites", new ObservableCollection<string>());

        // User settings
        public static readonly Setting<bool> EnableLocation =
            new Setting<bool>("EnableLocation", false);
        // Main Default View
        // 0 -> favourites; 1 -> recents ; 2 -> none
        public static readonly Setting<int> MainDefaultView =
            new Setting<int>("MainDefaultView", 0);
    }
}