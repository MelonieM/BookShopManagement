<<<<<<< HEAD
﻿using System.Windows;

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
=======
﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace BookShopManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

}
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
