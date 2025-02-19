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
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Microsoft.EntityFrameworkCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape.Pages.Player
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemDescriptionWindow : Window
    {
        private PlayerItem? selectedPlayerItem;
        private Item selectedItem;
        public ItemDescriptionWindow(int selectedItemId)
        {
            this.InitializeComponent();
            using (AppDbContext db = new AppDbContext())
            {
                selectedPlayerItem = db.PlayerItems
                    .Where(playeritem => playeritem.Id == selectedItemId)
                    .Include(playeritem => playeritem.Item)
                    .FirstOrDefault();

            }

            if (selectedPlayerItem == null)
            {
                this.Close();
                return;
            }

            selectedItem = selectedPlayerItem.Item;

            itemNameTbl.Text = $"{selectedItem.Name} ({selectedItem.Type})";
            rarityTbl.Text = selectedItem.Rarity.ToString();
            itemDescTbl.Text = selectedItem.Description;
            additionalEffectsTbl.Text = selectedItem.AdditionalEffects;
        }
    }
}
