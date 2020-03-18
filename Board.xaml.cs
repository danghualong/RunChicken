using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Board.xaml 的交互逻辑
    /// </summary>
    public partial class Board : UserControl
    {
        private static int SideEpochs = 2;
        private List<string> TextBags = null;
        private List<string> SideCharacters = new List<string>();
        private List<string> HoleCharacters = new List<string>();
        
        public Board()
        {
            InitializeComponent();
            TextBags = new List<string>() { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
            SideCharacters = new List<string>();
            for (var i = 0; i < SideEpochs; i++)
            {
                SideCharacters.AddRange(TextBags);
            }
            HoleCharacters = new List<string>();
            HoleCharacters.AddRange(TextBags);
            this.SizeChanged += Board_SizeChanged;
            InitBoard();
        }

        private void Board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            deployCards();
        }

        private void shuffleCards(List<string> cards)
        {
            Random rand = new Random();
            var count = cards.Count;
            for (var i = 0; i < count - 1; i++)
            {
                var index = rand.Next(count - i);
                var tmp = cards[index];
                cards[index] = cards[count - i - 1];
                cards[count - i - 1] = tmp;
            }
        }

        public void InitBoard()
        {
            shuffleCards(SideCharacters);
            shuffleCards(HoleCharacters);
            deployCards();
        }

        private void deployCards()
        {
            var cardHeight = 120;
            var cardWidth = 80;
            grid.Children.Clear();
            var width = this.ActualWidth;
            var height = this.ActualHeight;
            var radius = (width- cardHeight) / 2;
            if (width > height)
            {
                radius = (height- cardHeight) / 2;
            }
            for (var i = 0; i < SideCharacters.Count; i++)
            {
                SideCard card = new SideCard();
                card.Width = cardWidth;
                card.Height = cardHeight;
                card.RenderTransformOrigin = new Point(0.5, 0.5);
                card.Character = SideCharacters[i];
                card.PlayerCovered = 0;
                card.Tag = i;
                var group = new TransformGroup();
                group.Children.Add(new RotateTransform(15 * i, 0,radius));
                group.Children.Add(new TranslateTransform(0, -radius));
                card.RenderTransform = group;
                grid.Children.Add(card);
            }

            var panel = new UniformGrid();
            panel.Rows = 3;
            panel.Columns = 4;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;
            panel.Margin = new Thickness(width/2-radius+cardHeight,height/2-radius+cardHeight, width / 2 - radius + cardHeight, height / 2 - radius + cardHeight);
            for (var i = 0; i < HoleCharacters.Count; i++)
            {
                HoleCard card = new HoleCard();
                card.Tag = i;
                card.Character = HoleCharacters[i];
                card.FrontSide = 0;
                card.Margin = new Thickness(10);
                panel.Children.Add(card);
            }
            grid.Children.Add(panel);
        }
    }
}
