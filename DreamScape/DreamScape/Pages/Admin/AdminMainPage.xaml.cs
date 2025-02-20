using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DreamScape.Pages.Admin.Sub_Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Page
    {
        public AdminMainPage()
        {
            this.InitializeComponent();

            adminNv.SelectedItem = 0;
            adminFrame.Navigate(typeof(UpdateItemsPage));
        }

        private void adminNv_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItemBase selectedItem = args.SelectedItemContainer;

            if (selectedItem != null)
            {
                string selectedItemTag = ((string)selectedItem.Tag);
                switch (selectedItemTag)
                {
                    case "Items":
                        adminFrame.Navigate(typeof(UpdateItemsPage));
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
