using ATM.Kiosk;
using BankATM.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var culture = new CultureInfo("es-DO");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencySymbol = "RD$";
            CultureInfo.DefaultThreadCurrentUICulture.NumberFormat.CurrencySymbol = "RD$";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppServices.Initialize();
            Application.Run(new LoginForm());
        }
    }
}
