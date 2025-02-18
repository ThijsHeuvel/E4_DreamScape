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
using DreamScape.Universal;
using DreamScape.Data;
using DreamScape.Data.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DreamScape
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private string? email;
        private string? password;
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void createLink_Click(Microsoft.UI.Xaml.Documents.Hyperlink sender, Microsoft.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void emailBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login();
            }
        }

        private void passBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login();
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            email = emailBox.Text;
            password = passBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Helper.ShowPopup("Error", "Please fill in all fields.", this.XamlRoot);
                return;
            }

            using (AppDbContext db = new AppDbContext())
            {
                User? user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    Helper.ShowPopup("Error", "Invalid Credentials.", this.XamlRoot); // Say "Invalid Credentials" instead of "Email is already in use." against brute force attacks
                    return;
                }

                if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                {
                    Helper.ShowPopup("Error", "Invalid Credentials.", this.XamlRoot); // Say "Invalid Credentials" instead of "Email is already in use." against brute force attacks
                    return;
                }

                Session.Instance.User = user;
                Helper.ShowPopup("Success", "Login Successful! Logging in as " + Session.Instance.User.Role.ToString(), this.XamlRoot);
            }
        }


    }
}
    