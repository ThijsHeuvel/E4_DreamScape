using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    class Helper
    {
        public static bool EmailChecker(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Examines the domain part and normalizes it
                string DomainMapper(Match match)
                {
                    IdnMapping idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }

                email = Regex.Replace(email,
                                      @"(@)(.+)$",
                                      DomainMapper,
                                      RegexOptions.None,
                                      TimeSpan.FromMilliseconds(200));

            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                // Validates and returns true if matches otherwise false.
                return Regex.IsMatch(email,
                                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                                    RegexOptions.IgnoreCase,
                                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static async void ShowPopup(string title, string message, XamlRoot xamlRoot)
        {
            var errorDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok",
                XamlRoot = xamlRoot,
            };

            await errorDialog.ShowAsync();
        }
    }
}
