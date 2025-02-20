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
using DreamScape.Data;
using DreamScape.Data.Models;
using DreamScape.Data.Utility;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Admin.Sub_Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateItemsPage : Page
    {
        private List<Item>? allItems;
        private Item? selectedItem;
        public UpdateItemsPage()
        {
            this.InitializeComponent();

            // Load the items from the database
            LoadAllItems();
        }

        private void LoadAllItems()
        {
            using (AppDbContext db = new AppDbContext())
            {
                allItems = db.Items
                    .OrderBy(item => item.Name)
                    .ToList();
            }

            if (allItems == null)
            {
                allItemsListView.ItemsSource = new List<Item>();
            }
            else
            {
                allItemsListView.ItemsSource = allItems;
            }
        }

        private void allItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item? item = (Item)allItemsListView.SelectedItem;
            if (item != null)
            {
                selectedItem = item;
                SetItemInfo(item);
            }
        }

        private void SetItemInfo(Item item)
        {
            selectItemTbl.Visibility = Visibility.Collapsed;
            updateItemGrid.Visibility = Visibility.Visible;

            itemEditTb.Text = item.Name;
            descriptionEditTb.Text = item.Description;

            itemTypeCb.SelectedItem = itemTypeCb.Items.OfType<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == item.Type);

            itemRarityCb.SelectedItem = itemRarityCb.Items.OfType<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == item.Rarity.ToString());

            if (string.IsNullOrEmpty(item.AdditionalEffects))
            {
                additionalEffectsEditTb.Text = "";
            }
            else
            {
                additionalEffectsEditTb.Text = item.AdditionalEffects;
            }

        }

        private void seachAllItems_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = seachAllItems.Text;
            if (allItems == null)
            {
                LoadAllItems();
            }
            else if (string.IsNullOrEmpty(searchQuery))
            {
                allItemsListView.ItemsSource = allItems;
            }
            else
            {
                allItemsListView.ItemsSource = allItems
                    .Where(item => item.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(item => item.Name)
                    .ToList();
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
            {
                return;
            }
            else if (string.IsNullOrEmpty(itemEditTb.Text) || string.IsNullOrEmpty(descriptionEditTb.Text))
            {
                Helper.ShowPopup("Error", "1 or more fields empty.", XamlRoot);
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                Item? itemToChange = db.Items
                    .Where(item => item.Id == selectedItem.Id)
                    .FirstOrDefault();

                if (itemToChange == null)
                {
                    Helper.ShowPopup("Error", "Item not found.", XamlRoot);
                    return;
                }

                itemToChange.Name = itemEditTb.Text;
                itemToChange.Description = descriptionEditTb.Text;
                itemToChange.Type = (itemTypeCb.SelectedItem as ComboBoxItem)?.Content.ToString();

                itemToChange.Rarity = Enum.TryParse<Rarity>((itemRarityCb.SelectedItem as ComboBoxItem)?.Content.ToString(), out var rarity)
                                      ? rarity : Rarity.Common;

                itemToChange.AdditionalEffects = additionalEffectsEditTb.Text;

                db.UpdateRange(itemToChange);
                db.SaveChanges();
            }
            LoadAllItems();
            return;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(itemCreateTb.Text) || string.IsNullOrEmpty(descriptionCreateTb.Text))
            {
                Helper.ShowPopup("Error", "1 or more fields empty.", XamlRoot);
                return;
            }

            string itemType = (itemTypeCreateCb.SelectedItem as ComboBoxItem)?.Content.ToString();
            string itemRarityStr = (itemRarityCreateCb.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(itemType) || string.IsNullOrEmpty(itemRarityStr) || !Enum.TryParse<Rarity>(itemRarityStr, out var itemRarity))
            {
                Helper.ShowPopup("Error", "Invalid item type or rarity.", XamlRoot);
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                Item newItem = new Item()
                {
                    Name = itemCreateTb.Text,
                    Description = descriptionCreateTb.Text,
                    Type = itemType,
                    Rarity = itemRarity,
                    AdditionalEffects = additionalEffectsCreateTb.Text
                };

                db.Items.Add(newItem);
                db.SaveChanges();
            }

            LoadAllItems(); // Refresh the list after adding

            // Clear input fields
            itemCreateTb.Text = "";
            descriptionCreateTb.Text = "";
            itemTypeCreateCb.SelectedIndex = -1;
            itemRarityCreateCb.SelectedIndex = -1;
            additionalEffectsCreateTb.Text = "";

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Item item = (sender as Button).CommandParameter as Item;
            if (item != null)
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Items.RemoveRange(item);
                    // Remove all player items with the item id
                    db.PlayerItems.RemoveRange(db.PlayerItems.Where(pi => pi.ItemId == item.Id));
                    db.SaveChanges();
                }
                LoadAllItems();
            }
        }
    }
}
