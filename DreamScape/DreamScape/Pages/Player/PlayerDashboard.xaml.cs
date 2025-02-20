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
using DreamScape.Data.Models;
using DreamScape.Data;
using Microsoft.EntityFrameworkCore;
using DreamScape.Data.Utility;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Player
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerDashboard : Page
    {
        private List<PlayerItem> PlayerItems = new List<PlayerItem>();
        private User? User;
        public PlayerDashboard()
        { 
            this.InitializeComponent();
            if (Session.Instance.User == null)
            {
                Frame.Navigate(typeof(LoginPage)); // If not logged in, return to login page.
            }
            else
            {
                User = Session.Instance.User;
            }

            LoadMyItems();
            LoadAllItems();
        }

        private void LoadMyItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                PlayerItems = db.PlayerItems
                    .Where(playerItem => playerItem.UserId == User.Id)
                    .Include(playerItem => playerItem.Item)
                    .OrderBy(playeritem => playeritem.Item.Name)
                    .ToList();
            }

            myItemsLv.ItemsSource = PlayerItems;
        }

        private void LoadAllItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                List<Item> items = db.Items
                    .OrderBy(item => item.Name)
                    .ToList();
                availableItemsLv.ItemsSource = items;
            }
        }


        private void myItemsLv_ItemClick(object sender, ItemClickEventArgs e)
        {
            PlayerItem selectedPlayerItem = (PlayerItem)e.ClickedItem;
            ItemDescriptionWindow descriptionWindow = new ItemDescriptionWindow(selectedPlayerItem.Id, true);
            descriptionWindow.Title = selectedPlayerItem.Item.Name;
            descriptionWindow.Activate();
        }

        private void searchMyItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? searchQuery = searchMyItems.Text;
            if (string.IsNullOrEmpty(searchQuery))
            {
                myItemsLv.ItemsSource = PlayerItems;
            }

            myItemsLv.ItemsSource = PlayerItems
                .Where(playerItem => playerItem.Item.Name
                .IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Session.Instance.Logout(Frame, this.XamlRoot);
        }

        private void availableItemsLv_ItemClick(object sender, ItemClickEventArgs e)
        {
            Item selectedItem = (Item)e.ClickedItem;
            ItemDescriptionWindow descriptionWindow = new ItemDescriptionWindow(selectedItem.Id, false);
            descriptionWindow.Title = selectedItem.Name;
            descriptionWindow.Activate();
        }

        private void searchAvailableItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? searchQuery = searchAvailableItems.Text;
            using (AppDbContext db = new AppDbContext())
            {
                List<Item> items = db.Items.ToList();
                if (string.IsNullOrEmpty(searchQuery))
                {
                    availableItemsLv.ItemsSource = items;
                }
                availableItemsLv.ItemsSource = items
                    .Where(item => item.Name
                    .IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
        }
    }
}
