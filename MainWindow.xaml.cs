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
            InitPlayers();
            board.CurrentPlayerChanged += Board_CurrentPlayerChanged;
            board.PlayerWon += Board_PlayerWon;
            board.Players = Players;
            board.Reset();
            this.DataContext = this;
        }

        private void Board_PlayerWon(Player obj)
        {
            var result=MessageBox.Show(string.Format("恭喜{0}！是否再来一局？", obj.PlayerName),"恭贺",MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                board.Reset();
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
            players = new ObservableCollection<Player>();
            players.Add(new Player() { PlayerName = "党语萱", Lives = 1,Avatar= "pack://application:,,,/RunChicken;component/imgs/dyx.jpg" });
            players.Add(new Player() { PlayerName = "党秉宸", Lives = 1, Avatar = "pack://application:,,,/RunChicken;component/imgs/dbc.jpg" });
        }
    }
}
