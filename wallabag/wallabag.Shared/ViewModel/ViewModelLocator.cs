/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:wallabag"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace wallabag.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ItemPageViewModel>();
            SimpleIoc.Default.Register<SettingsPageViewModel>();
        }

        // -----------------------------------
        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }
        public ItemPageViewModel ItemPage { get { return ServiceLocator.Current.GetInstance<ItemPageViewModel>(); } }
        public SettingsPageViewModel SettingsPage { get { return ServiceLocator.Current.GetInstance<SettingsPageViewModel>(); } }
    }
}