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
        
        public int PlayerCovered
        {
            get
            {
                return (int)GetValue(PlayerCoveredProperty);
            }
            set
            {
                SetValue(PlayerCoveredProperty, value);
            }
        }

        public static readonly DependencyProperty PlayerCoveredProperty = DependencyProperty.Register("PlayerCovered", typeof(int), typeof(SideCard), new PropertyMetadata(-1, new PropertyChangedCallback(PlayerCoveredChanged)));

        private static void PlayerCoveredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SideCard;
            int typeId = (int)e.NewValue;
            if(typeId==0)
            {
                obj.img.Opacity =0;
                obj.tb1.Visibility = Visibility.Visible;
            }
            else
            {
                obj.img.Opacity = 0.7;
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
    }
}
