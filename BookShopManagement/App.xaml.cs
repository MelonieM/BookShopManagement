using System.Windows;

namespace BookShopManagement
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show Login Window instead of Main Window
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}