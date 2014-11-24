using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace wallabag.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        public Models.Item Model { get; set; }
        
        public string Title
        {
            get { return Model.Title; }
            set
            {
                if (value != Model.Title)
                {
                    Model.Title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        public string Content
        {
            get {
                var content =
                    "<html><head><link rel=\"stylesheet\" href=\"ms-appx-web:///Assets/css/wallabag.css\" type=\"text/css\" media=\"screen\" /></head>" +
                        Model.Content +
                    "</html>";
                return content; 
            }
            set
            {
                if (value != Model.Content)
                {
                    Model.Content = value;
                    RaisePropertyChanged(() => Content);
                }
            }
        }

        public Uri Url
        {
            get { return Model.Url; }
            set
            {
                if (value != Model.Url)
                {
                    Model.Url = value;
                    RaisePropertyChanged(() => Url);
                }
            }
        }

        //TODO: Implement the excerpt for the Windows version (and maybe the WP version)
        private string _Excerpt;
        public string Excerpt
        {
            get { return _Excerpt; }
            set { Set(() => Excerpt, ref _Excerpt, value); }
        }
        

        public ItemViewModel() { Model = new Models.Item(); }
        public ItemViewModel(Models.Item M) { Model = M; }
    }
}
