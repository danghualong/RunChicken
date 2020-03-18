using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RunChicken.models;

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
        private ObservableCollection<Player> players = null;
        private List<SideCard> sideCards = new List<SideCard>();
        private List<HoleCard> holeCards = new List<HoleCard>();
        private List<Player> ExceedPlayers = new List<Player>();
        private int currentPlayerIndex;
        private int CurrentPlayerIndex
        {
            get
            {
                return currentPlayerIndex;
            }
            set
            {
                currentPlayerIndex = value;
                OnCurrentPlayerChanged();
            }
        }

        public event Action<Player> CurrentPlayerChanged;
        public event Action<Player> PlayerWon;

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
            ShowPlayers();
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
            sideCards.Clear();
            holeCards.ForEach(p =>
            {
                p.CardUnfolded -= Card_CardUnfolded;
            });
            holeCards.Clear();
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
                card.Avatar = string.Empty;
                card.Index = i;
                var group = new TransformGroup();
                group.Children.Add(new RotateTransform(15 * i, 0,radius));
                group.Children.Add(new TranslateTransform(0, -radius));
                card.RenderTransform = group;
                grid.Children.Add(card);
                sideCards.Add(card);
            }

            var panel = new UniformGrid();
            panel.Rows = 3;
            panel.Columns = 4;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;
            var paddingH = width / 2 - radius + cardHeight + 5;
            var paddingV = height / 2 - radius + cardHeight + 5;
            panel.Margin = new Thickness(paddingH, paddingV, paddingH, paddingV);
            for (var i = 0; i < HoleCharacters.Count; i++)
            {
                HoleCard card = new HoleCard();
                card.Tag = i;
                card.Character = HoleCharacters[i];
                card.FrontSide = 0;
                card.Margin = new Thickness(10);
                card.CardUnfolded += Card_CardUnfolded;
                panel.Children.Add(card);
                holeCards.Add(card);
            }
            grid.Children.Add(panel);
        }

        private void Card_CardUnfolded(string obj)
        {
            var nextCard = getNextCard();
            if (nextCard == null)
            {
                return;
            }
            string nextCardText = nextCard.Character;
            if (string.Equals(obj, nextCardText))
            {
                var player = this.players[CurrentPlayerIndex];
                var currentSideCard = sideCards.FirstOrDefault(p=>p.Index==player.Position);
                currentSideCard.Avatar = string.Empty;
                nextCard.Avatar = player.Avatar;
                player.Position = nextCard.Index;
                ExceedPlayers.ForEach(p =>
                {
                    var tmpPlayer=players.FirstOrDefault(t => t.Position == p.Position);
                    tmpPlayer.Lives--;
                    p.Lives--;
                    if (p.Lives <= 0)
                    {
                        p.IsOut = true;
                        var card = sideCards.FirstOrDefault(t => t.Index == p.Position);
                        card.Avatar = string.Empty;
                    }
                });
                var list = players.Where(p => !p.IsOut).ToList();
                if (list.Count == 1)
                {
                    if (PlayerWon != null)
                    {
                        PlayerWon(list[0]);
                    }
                }
            }
            else
            {
                SwitchCurrentPlayer();
            }
            ExceedPlayers.Clear();
        }

        public void SetPlayers(ObservableCollection<Player> players)
        {
            this.players = players;
            int count = players.Count;
            var sep = SideCharacters.Count / count;
            for(int i = 0; i < count; i++)
            {
                players[i].Position = sep * i+6;
                var sideCard=sideCards.FirstOrDefault(p => p.Index == players[i].Position);
                if (sideCard != null)
                {
                    sideCard.Avatar = players[i].Avatar;
                }
            }
            this.CurrentPlayerIndex = 0;


        }

        public void SwitchCurrentPlayer()
        {
            var index=this.CurrentPlayerIndex+1;
            this.CurrentPlayerIndex = index % this.players.Count;
        }

        private SideCard getNextCard()
        {
            var player = this.players[CurrentPlayerIndex];
            var cardIndex = player.Position;
            for (var i = 0; i < SideCharacters.Count; i++)
            {
                var nextCard = this.sideCards[(cardIndex + 1 + i) % (SideCharacters.Count)];
                if (!nextCard.PlayerCovered)
                {
                    return nextCard;
                }
                else
                {
                    var tmpPlayer=players.FirstOrDefault(p => p.Position == nextCard.Index);
                    ExceedPlayers.Add(tmpPlayer);
                }
            }
            return null;
        }

        private void ShowPlayers()
        {
            int count = players.Count;
            for (int i = 0; i < count; i++)
            {
                var sideCard = sideCards.FirstOrDefault(p =>p.Index == players[i].Position);
                if (sideCard != null)
                {
                    sideCard.Avatar = players[i].Avatar;
                }
            }
        }

        private void OnCurrentPlayerChanged()
        {
            if (CurrentPlayerChanged != null)
            {
                CurrentPlayerChanged(this.players[CurrentPlayerIndex]);
            }
        }

        public void Reset()
        {
            InitBoard();
            for (var i = 0; i < players.Count; i++)
            {
                players[i].IsOut = false;
            }
            SetPlayers(this.players);
        }
    }
}
