using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunChicken.models
{
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class Player:NotificationObject
    {
        private string playerName;
        private int lives;
        private bool isChecked;
        private string avatar;

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                RaisePropertyChange("PlayerName");
            }
        }
        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                RaisePropertyChange("Lives");
            }
        }
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                RaisePropertyChange("IsChecked");
            }
        }

        public int Position { get; set; }

        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                RaisePropertyChange("Avatar");
            }
        }

        public bool IsOut { get; set; }
        /// <summary>
        /// 是否被其他玩家超越
        /// </summary>
        public bool IsExceeded { get; set; }

    }
}
