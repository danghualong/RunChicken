using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
        private List<string> TextBags = new List<string>() { "我", "们", "进", "入", "了", "埋", "在", "城", "市", "的", "输", "水" };
        private List<string> SideCharacters = new List<string>();
        private List<string> HoleCharacters = new List<string>();
        private List<SideCard> sideCards = new List<SideCard>();
        private List<HoleCard> holeCards = new List<HoleCard>();
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

        public Board()
        {
            InitializeComponent();
            InitCardCharacters();
            this.SizeChanged += Board_SizeChanged;
        }

        private void InitCardCharacters()
        {
            if(TextBags==null || TextBags.Count <= 0)
            {
                return;
            }
            SideCharacters = new List<string>();
            for (var i = 0; i < SideEpochs; i++)
            {
                SideCharacters.AddRange(TextBags);
            }
            HoleCharacters = new List<string>();
            HoleCharacters.AddRange(TextBags);
        }

        private void Board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutCards();
            LayoutPlayers();
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="cards"></param>
        private void ShuffleCards(List<string> cards)
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

        private void InitBoard()
        {
            ShuffleCards(SideCharacters);
            ShuffleCards(HoleCharacters);
            LayoutCards();
        }

        private void InitPlayers()
        {
            if (Players.Count <= 0)
            {
                return;
            }
            var sep = SideCharacters.Count / Players.Count;
            int index = 0;
            //设置每个玩家的位置
            foreach(var p in Players)
            {
                index++;
                p.IsOut = false;
                p.Position = (sep * index + 8) % this.SideCharacters.Count;
            }
            LayoutPlayers();
            this.CurrentPlayerIndex = 0;
            //UpdateActiveCard();
        }

        private void UpdateActiveCard()
        {
            sideCards.ForEach(p => p.SetActive(false));
            var index = this.Players[this.CurrentPlayerIndex].Position;
            var card=sideCards.FirstOrDefault(p => p.Index == index);
            card.SetActive(true);
        }
        /// <summary>
        /// 布局底牌和边牌
        /// </summary>
        private void LayoutCards()
        {
            var cardHeight = 120;
            var cardWidth = 100;
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
                card.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                card.Character = SideCharacters[i];
                card.SetPlayer(null);
                card.Index = i;
                var group = new TransformGroup();
                group.Children.Add(new RotateTransform(15 * i, 0,radius));
                group.Children.Add(new TranslateTransform(0, -radius));
                card.RenderTransform = group;
                grid.Children.Add(card);
                sideCards.Add(card);
            }

            var panel = new UniformGrid();
            panel.Background = new SolidColorBrush(Colors.Transparent);
            panel.Rows = 3;
            panel.Columns = 4;
            Panel.SetZIndex(panel, -1);
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.VerticalAlignment = VerticalAlignment.Center;
            var paddingH = width / 2 - radius + cardHeight+15;
            var paddingV = height / 2 - radius + cardHeight+15;
            panel.Margin = new Thickness(paddingH, paddingV, paddingH, paddingV);
            for (var i = 0; i < HoleCharacters.Count; i++)
            {
                HoleCard card = new HoleCard();
                card.Tag = i;
                card.Character = HoleCharacters[i];
                card.FrontSide = 0;
                card.Background = new SolidColorBrush(Colors.Transparent);
                card.Margin = new Thickness(6);
                card.CardUnfolded += Card_CardUnfolded;
                panel.Children.Add(card);
                holeCards.Add(card);
            }
            
            grid.Children.Add(panel);
        }
        /// <summary>
        /// 切换到下一个未被淘汰的玩家
        /// </summary>
        private void SwitchCurrentPlayer()
        {
            var index=(this.CurrentPlayerIndex+1)%this.Players.Count;
            while (this.Players[index].IsOut)
            {
                index= (index + 1) % this.Players.Count;
            }
            this.CurrentPlayerIndex = index;
        }
        /// <summary>
        /// 获取当前玩家下一个要跳到的边牌
        /// </summary>
        /// <returns></returns>
        private SideCard GetNextCard()
        {
            var player = this.Players[CurrentPlayerIndex];
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
                    var tmpPlayer=Players.FirstOrDefault(p => p.Position == nextCard.Index);
                    //ExceedPlayers.Add(tmpPlayer);
                    tmpPlayer.IsExceeded = true;
                }
            }
            return null;
        }
        /// <summary>
        /// 布局玩家
        /// </summary>
        private void LayoutPlayers()
        {
            foreach(var p in Players)
            {
                var sideCard = sideCards.FirstOrDefault(s => s.Index == p.Position);
                if (sideCard != null)
                {
                    sideCard.SetPlayer(p);
                }
            }
        }


        #region 接口方法
        public event Action<Player> CurrentPlayerChanged;
        private void OnCurrentPlayerChanged()
        {
            if (CurrentPlayerChanged != null)
            {
                CurrentPlayerChanged(this.Players[CurrentPlayerIndex]);
            }
        }

        public event Action<Player> PlayerWon;

        public ObservableCollection<Player> Players
        {
            get; set;
        }
        public void Reset()
        {
            InitBoard();
            InitPlayers();
        }
        /// <summary>
        /// 翻底牌之后的结果
        /// </summary>
        /// <param name="obj"></param>
        private void Card_CardUnfolded(string obj)
        {
            var nextCard = GetNextCard();
            if (nextCard == null)
            {
                return;
            }
            string nextCardText = nextCard.Character;

            //如果底牌和边牌一致
            if (string.Equals(obj, nextCardText))
            {
                var currentPlayer = this.Players[CurrentPlayerIndex];
                //当前边牌的头像消失
                var currentSideCard = sideCards.FirstOrDefault(p => p.Index == currentPlayer.Position);
                currentSideCard.SetPlayer(null);
                //下一张牌设置玩家头像
                nextCard.SetPlayer(currentPlayer);
                //未被淘汰的玩家数量
                int alivePlayerCount = 0;
                foreach (var p in Players)
                {
                    if (p.IsExceeded)
                    {
                        p.Lives--;
                        //当前用户生命结束，从边牌上移除
                        if (p.Lives <= 0)
                        {
                            p.IsOut = true;
                            var card = sideCards.FirstOrDefault(t => t.Index == p.Position);
                            card.SetPlayer(null);
                        }
                        p.IsExceeded = false;
                    }
                    alivePlayerCount += p.IsOut ? 0 : 1;
                }
                //其他玩家全部淘汰后，结束游戏
                if (alivePlayerCount == 1)
                {
                    if (PlayerWon != null)
                    {
                        PlayerWon(currentPlayer);
                    }
                }
            }
            else //如不一致，切换到下一个未被淘汰的玩家
            {
                SwitchCurrentPlayer();
            }
            //UpdateActiveCard();
        }
        /// <summary>
        /// 设置词袋
        /// </summary>
        /// <param name="textBags"></param>
        public void SetTextBags(List<string> textBags)
        {
            this.TextBags = textBags;
            InitCardCharacters();
            Reset();
        }
        #endregion
    }
}
