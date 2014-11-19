using System;
using System.Collections.Generic;
using System.Text;
using MyToolkit.Mvvm;
using MyToolkit.Command;

namespace wallabag.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        public Models.Article Model { get; set; }
        
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

        public bool Favourite
        {
            get { return Model.Favourite; }
            set
            {
                if (value != Model.Favourite)
                {
                    Model.Favourite = value;
                    RaisePropertyChanged(() => Favourite);
                }
            }
        }

        public bool Read
        {
            get { return Model.Read; }
            set
            {
                if (value != Model.Read)
                {
                    Model.Read = value;
                    RaisePropertyChanged(() => Read);
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

        public DateTime PublishedDate
        {
            get { return Model.PublishedDate; }
            set
            {
                if (value != Model.PublishedDate)
                {
                    Model.PublishedDate = value;
                    RaisePropertyChanged(() => PublishedDate);
                }
            }
        }

        public ArticleViewModel() { Model = new Models.Article(); }
        public ArticleViewModel(Models.Article M) { Model = M; }
    }
}
