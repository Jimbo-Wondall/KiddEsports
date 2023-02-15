using Data_Management;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KiddEsports
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DBS_Builder builder = new DBS_Builder();
            //builder.DropDatabase();
            builder.CreateDatabase();
            if (builder.DoTablesExist() == false)
            {
                builder.BuildDatabaseTables();
                builder.SeedDatabaseTables();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AsyncStartup(1000, e);
        }
        private async void AsyncStartup(int minDelay, StartupEventArgs e)
        {
            LaunchScreen splash = new LaunchScreen();
            splash.Show();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            base.OnStartup(e);
            MainWindow main = new MainWindow();
            timer.Stop();
            int delayRemaining = minDelay - (int)timer.ElapsedMilliseconds;
            if (delayRemaining > 0) await Task.Delay(delayRemaining);

            await Task.Delay(minDelay);
            splash.Close();
        }
    }
}
