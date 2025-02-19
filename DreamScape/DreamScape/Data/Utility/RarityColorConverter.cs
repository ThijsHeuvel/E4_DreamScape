using DreamScape.Data.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data.Utility
{
    public class RarityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Rarity rarity)
            {
                return rarity switch
                {
                    Rarity.Common => new SolidColorBrush(Colors.LightGray),
                    Rarity.Uncommon => new SolidColorBrush(Colors.Green),
                    Rarity.Rare => new SolidColorBrush(Colors.Blue),
                    Rarity.Epic => new SolidColorBrush(Colors.Purple),
                    Rarity.Legendary => new SolidColorBrush(Colors.Orange),
                    Rarity.Mythic => new SolidColorBrush(Colors.Yellow),
                    _ => new SolidColorBrush(Colors.White),
                };
            }

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
