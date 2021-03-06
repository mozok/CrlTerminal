﻿using CrlTerminal.Views;
using System.Windows;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using Prism.Unity;
using CrlTerminal.Models;

namespace CrlTerminal
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(YOUR_MODULE));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterTypeForNavigation<SpecList>();
            Container.RegisterTypeForNavigation<TalonRegistry>();
            Container.RegisterTypeForNavigation<MyTalonsView>();

            Container.RegisterInstance<IUsersService>(new UsersService());
        }

    }

    //public static class UnityExtensions
    //{
    //    public static void RegisterTypeForNavigation<T>(this IUnityContainer container, string name)
    //    {
    //        container.RegisterType(typeof(object), typeof(T), name);
    //    }
    //}
}
