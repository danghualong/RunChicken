using RunChicken.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunChicken
{
    /// <summary>
    /// HoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class SideCard : UserControl,INotifyPropertyChanged
    {
        private DoubleAnimation blinkAnimation;
        private Storyboard storyBoard;
        static SideCard()
        {
            
        }
        public SideCard()
        {
            InitializeComponent();
            this.SizeChanged += HoleCard_SizeChanged;
            blinkAnimation = new DoubleAnimation()
            {
                To = 0.6,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new TimeSpan(0, 0, 0, 2, 0)
            };
            storyBoard = new Storyboard();
            Storyboard.SetTargetProperty(blinkAnimation, new PropertyPath(Image.OpacityProperty));
            storyBoard.Children.Add(blinkAnimation);
        }

        private void HoleCard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
        /// <summary>
        /// 当前牌的索引
        /// </summary>
        public int Index
        {
            get;set;
        }

        public bool PlayerCovered
        {
            get
            {
                return !string.IsNullOrEmpty(PlayerImagePath);
            }
        }

        private bool isActiveCard;
        public bool IsActiveCard
        {
            get
            {
                return isActiveCard;
            }
            set
            {
                isActiveCard = value;
                RaisePropertyChange("IsActiveCard");
            }
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                storyBoard.Begin(img,true);
            }
            else
            {
                storyBoard.Stop(img);
            }
        }

        public void SetPlayer(Player player)
        {
            if (player == null)
            {
                this.PlayerImagePath = string.Empty;
            }
            else
            {
                player.Position = this.Index;
                this.PlayerImagePath = player.Avatar;
            }
        }

        public string PlayerImagePath
        {
            get
            {
                return (string)GetValue(PlayerImagePathProperty);
            }
            set
            {
                SetValue(PlayerImagePathProperty, value);
            }
        }

        public static readonly DependencyProperty PlayerImagePathProperty = DependencyProperty.Register("PlayerImagePath", typeof(string), typeof(SideCard), new PropertyMetadata(null, new PropertyChangedCallback(PlayerImagePathChanged)));

        private static void PlayerImagePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SideCard;
            string imgPath = (string)e.NewValue;
            if(string.IsNullOrEmpty(imgPath))
            {
                //obj.img.Opacity =0;
                //obj.img.Source = null;
                obj.img.Visibility = Visibility.Collapsed;
                obj.tb1.Visibility = Visibility.Visible;
            }
            else
            {
                //obj.img.Opacity = 1;
                obj.img.Source = new BitmapImage(new Uri(imgPath));
                obj.img.Visibility = Visibility.Visible;
                obj.tb1.Visibility = Visibility.Collapsed;
            }
        }
        public string Character
        {
            get
            {
                return (string)GetValue(CharacterProperty);
            }
            set
            {
                SetValue(CharacterProperty, value);
            }
        }

        public static readonly DependencyProperty CharacterProperty = DependencyProperty.Register("Character", typeof(string), typeof(SideCard), new PropertyMetadata(string.Empty, new PropertyChangedCallback(CharacterChanged)));

        private static void CharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SideCard;
            string character = (string)e.NewValue;
            obj.tb1.Text = character;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
