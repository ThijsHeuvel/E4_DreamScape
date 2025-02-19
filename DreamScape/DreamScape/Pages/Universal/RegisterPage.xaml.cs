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

namespace DreamScape.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private string? email;
        private string? password;
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            MakeAccount();
        }

        private void passBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                MakeAccount();
            }
        }

        private void emailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                MakeAccount();
            }
        }

        private void MakeAccount()
        {
            email = emailBox.Text;
            password = passBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Helper.ShowPopup("Error", "Please fill in all fields.", this.XamlRoot);
                return;
            }

            if (!Helper.EmailChecker(email))
            {
                Helper.ShowPopup("Error", "Please enter a valid email address.", this.XamlRoot);
                return;
            }

            if (password.Length < 8)
            {
                Helper.ShowPopup("Error", "Password must be at least 8 characters long.", this.XamlRoot);
                return;
            }

            // Check if email is already in use
            using (AppDbContext db = new AppDbContext())
            {
                if (db.Users.Any(user => user.Email == email))
                {
                    Helper.ShowPopup("Error", "Email is already in use.", this.XamlRoot);
                    return;
                }

                string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

                User newUser = new User
                {
                    Email = email,
                    Password = hashedPassword,
                    Role = Role.Player
                };

                db.Users.Add(newUser);
                db.SaveChanges();
            }

            Helper.ShowPopup("Success", "Account created successfully!", this.XamlRoot);
            Frame.Navigate(typeof(LoginPage));
            return;
        }


    }
}
