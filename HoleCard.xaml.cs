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
    public partial class HoleCard : UserControl
    {
        private static bool IsLocked = false;
        public HoleCard()
        {
            InitializeComponent();
            this.SizeChanged += HoleCard_SizeChanged;
        }

        private void HoleCard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var padding = 3f;
            var width = this.ActualWidth-2*padding;
            var height = this.ActualHeight-2 * padding;
            var unit = width / 2;
            var sqrt3 = Math.Sqrt(3);
            if (sqrt3 * width> 2 * height)
            {
                unit =height / sqrt3;
            }
            var xPadding = width/2-unit;
            var yPadding = height / 2 - sqrt3 * unit/2;
            var points = new PointCollection();
            points.Add(new Point(xPadding+0, yPadding+sqrt3 / 2 * unit));
            points.Add(new Point(xPadding+unit/2, yPadding + 0));
            points.Add(new Point(xPadding+unit*3 / 2, yPadding + 0));
            points.Add(new Point(xPadding+2*unit, yPadding + sqrt3 / 2 * unit));
            points.Add(new Point(xPadding+unit * 3 / 2, yPadding + sqrt3 * unit));
            points.Add(new Point(xPadding+unit/ 2, yPadding + sqrt3 * unit));
            pg.Points = points;
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

        public static readonly DependencyProperty FrontSideProperty = DependencyProperty.Register("FrontSide", typeof(int), typeof(HoleCard), new PropertyMetadata(-1, new PropertyChangedCallback(FrontSideChanged)));

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

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }
            FrontSide = 1;
            var worker = new ThreadService.CountDownWorker(2);
            worker.WorkCompleted += Worker_WorkCompleted;
            worker.Start();
            IsLocked = true;
        }

        private void Worker_WorkCompleted()
        {
            FrontSide = 0;
            IsLocked = false;
        }
    }
}
