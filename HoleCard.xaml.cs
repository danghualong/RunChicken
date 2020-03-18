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

namespace RunChicken
{
    /// <summary>
    /// HoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class HoleCard : UserControl
    {
        public HoleCard()
        {
            InitializeComponent();
        }

        public int FrontSide
        {
            get
            {
                return (int)GetValue(FrontSideProperty);
            }
            set
            {
                SetValue(FrontSideProperty, value);
            }
        }

        public static readonly DependencyProperty FrontSideProperty = DependencyProperty.Register("FrontSide", typeof(int), typeof(HoleCard), new PropertyMetadata(0, new PropertyChangedCallback(FrontSideChanged)));

        private static void FrontSideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as HoleCard;
            int typeId = (int)e.NewValue;
            if(typeId==0)
            {
                obj.img.Visibility = Visibility.Visible;
                obj.tb1.Visibility = Visibility.Collapsed;
            }
            else
            {
                obj.img.Visibility = Visibility.Collapsed;
                obj.tb1.Visibility = Visibility.Visible;
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

        public static readonly DependencyProperty CharacterProperty = DependencyProperty.Register("Character", typeof(string), typeof(HoleCard), new PropertyMetadata(string.Empty, new PropertyChangedCallback(CharacterChanged)));

        private static void CharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as HoleCard;
            string character = (string)e.NewValue;
            obj.tb1.Text = character;
        }
    }
}
