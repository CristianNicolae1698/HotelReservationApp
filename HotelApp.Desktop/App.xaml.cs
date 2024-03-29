﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static ServiceProvider serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<CheckInForm>();
            services.AddTransient<ISQLDataAccess, SQLDataAccess>();
            services.AddTransient<ISQLiteDataAccess, SQLiteDataAccess>();
            

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config=builder.Build();
            

            services.AddSingleton(config);

            string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
            if (dbChoice == "sql")
            {
                services.AddTransient<IDatabaseData, SqlData>();
            }
            else if (dbChoice == "sqlite")
            {
                services.AddTransient<IDatabaseData, SQLiteData>();
            }
            
            else
            {
                //Fallback / Default value
                services.AddTransient<IDatabaseData, SqlData>();
            }

            serviceProvider = services.BuildServiceProvider();
            var mainWindow=serviceProvider.GetService<MainWindow>();

            mainWindow.Show();





        }
    }
}
