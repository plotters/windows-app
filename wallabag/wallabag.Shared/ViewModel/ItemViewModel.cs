using System;
using System.Collections.Generic;
using System.Text;

namespace wallabag.ViewModel
{
    public class ItemViewModel : Common.viewModelBase
    {
        private Models.Item _Model;
        public Models.Item Model
        {
            get { return _Model; }
            set { Set(() => Model, ref _Model, value); }
        }

        public int Id
        {
            get { return Model.Id; }
            set
            {
                if (Model.Id != value && value != null)
                {
                    Model.Id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }
        public string Title
        {
            get { return Model.Title; }
            set
            {
                if (Model.Title != value && value != null)
                {
                    Model.Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        public Uri Url
        {
            get { return new Uri(Model.Url); }
            set
            {
                if (Model.Url != value.ToString() && value != null)
                {
                    Model.Url = value.ToString();
                    RaisePropertyChanged("Url");
                }
            }
        }
        public bool IsRead
        {
            get { return Model.IsRead == 1; }
            set
            {
                if ((Model.IsRead == 1) != value && value != null)
                {
                    int tmp = 0; if (value) tmp = 1;
                    Model.IsRead = tmp;
                    RaisePropertyChanged("IsRead");
                }
            }
        }
        public bool IsFavourite
        {
            get { return Model.IsFavourite == 1; }
            set
            {
                if ((Model.IsFavourite == 1) != value && value != null)
                {
                    int tmp = 0; if (value) tmp = 1;
                    Model.IsFavourite = tmp;
                    RaisePropertyChanged("IsFavourite");
                }
            }
        }
        public string Content
        {
            get { return Model.Content; }
            set
            {
                if (Model.Content != value && value != null)
                {
                    Model.Content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }
        public int UserId
        {
            get { return Model.UserId; }
            set
            {
                if (Model.UserId != value && value != null)
                {
                    Model.UserId = value;
                    RaisePropertyChanged("UserId");
                }
            }
        }
    }
}
