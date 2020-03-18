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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunChicken
{
    /// <summary>
    /// HoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class SideCard : UserControl
    {
        public SideCard()
        {
            InitializeComponent();
            this.SizeChanged += HoleCard_SizeChanged;
        }

        private void HoleCard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        public int Index
        {
            get;set;
        }
        
        public string Avatar
        {
            get
            {
                return (string)GetValue(AvatarProperty);
            }
            set
            {
                SetValue(AvatarProperty, value);
            }
        }

        public static readonly DependencyProperty AvatarProperty = DependencyProperty.Register("Avatar", typeof(string), typeof(SideCard), new PropertyMetadata(null, new PropertyChangedCallback(AvatarChanged)));

        private static void AvatarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        public bool PlayerCovered
        {
            get
            {
                return !string.IsNullOrEmpty(Avatar);
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
    }
}
