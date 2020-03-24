using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RunChicken.models;
using RunChicken.util;

namespace RunChicken
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private ObservableCollection<Player> players;
        public ObservableCollection<Player> Players
        {
            get
            {
                return players;
            }
            set
            {
                players = value;
                RaisePropertyChange("Players");
            }
        }

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
                RaisePropertyChange("CurrentPlayer");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            board.CurrentPlayerChanged += Board_CurrentPlayerChanged;
            board.PlayerWon += Board_PlayerWon;
            InitPlayers();
            board.Players = players;
            board.SetTextBags(GetWords());
            this.DataContext = this;
        }

        private void Board_PlayerWon(Player obj)
        {
            var result=MessageBox.Show(string.Format("恭喜{0}！是否再来一局？", obj.PlayerName),"恭贺",MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                InitPlayers();
                board.Players = players;
                board.SetTextBags(GetWords());
            }
            else
            {
                this.Close();
            }
        }

        private void Board_CurrentPlayerChanged(Player obj)
        {
            this.CurrentPlayer = obj;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void InitPlayers()
        {
            var playerList = new ObservableCollection<Player>();
            var strLives=ConfigHelper.GetConfig("Lives","1");
            var lives = int.Parse(strLives);
            playerList.Add(new Player() { PlayerName = "党语萱", Lives = lives, Avatar= "pack://application:,,,/RunChicken;component/imgs/dyx.jpg" });
            playerList.Add(new Player() { PlayerName = "党秉宸", Lives = lives, Avatar = "pack://application:,,,/RunChicken;component/imgs/dbc.jpg" });
            Players = playerList;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            var dr=window.ShowDialog();
            if (dr.Value)
            {
                InitPlayers();
                board.Players = players;
                board.SetTextBags(GetWords());
            }
        }

        private List<string> GetWords()
        {
            var words=ConfigHelper.GetConfig("TextPool","");
            List<string> wordVector = ConfigHelper.GetWordVector(words);
            var count = wordVector.Count;
            if(wordVector==null || count < ConfigHelper.TEXT_NUM)
            {
                return ConfigHelper.DEFAULT_WORDS;
            }
            Random rand = new Random();
            for(int i = 0; i < ConfigHelper.TEXT_NUM;i++)
            {
                var index=rand.Next(i, count);
                var tmp = wordVector[i];
                wordVector[i] = wordVector[index];
                wordVector[index] = tmp;
            }
            return wordVector.GetRange(0, ConfigHelper.TEXT_NUM);
        }
    }
}
