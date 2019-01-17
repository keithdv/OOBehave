using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OOBehave.Autofac;
using System.Reflection;
using System.Threading;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IContainer container;

        public MainWindow()
        {
            InitializeComponent();

            // Please don't do it this way - use MVVM

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new OOBehave.Autofac.OOBehaveCoreModule(OOBehave.Autofac.Portal.Local));
            builder.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            container = builder.Build();

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var validate = container.Resolve<ISimpleValidateObject>();
            await validate.CheckAllRules();
            DataContext = validate;
        }
    }
}
