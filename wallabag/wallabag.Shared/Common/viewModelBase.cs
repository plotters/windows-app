using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace wallabag.Common
{
    public class viewModelBase : ViewModelBase
    {
        public ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }
    }
}
