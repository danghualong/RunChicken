using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using System.Windows.Shapes;
using RunChicken.util;

namespace RunChicken
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            TextPool = ConfigHelper.GetConfig("TextPool","");
            Lives = ConfigHelper.GetConfig("Lives", "1");
        }
        private string textPool;
        public string TextPool
        {
            get
            {
                return textPool;
            }
            set
            {
                textPool = value;
                RaisePropertyChange("TextPool");
            }
        }

        private string lives;
        public string Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                RaisePropertyChange("Lives");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var list = ConfigHelper.GetWordVector(TextPool);
            if (list.Count < ConfigHelper.TEXT_NUM)
            {
                MessageBox.Show(string.Format("词汇表至少包括{0}个不同的字",ConfigHelper.TEXT_NUM));
                return;
            }
            int tmpLives = 1;
            var livesResult = int.TryParse(Lives, out tmpLives);
            if (!livesResult)
            {
                MessageBox.Show("鸡毛数请输入1-4的整数值");
                return;
            }
            if (tmpLives < 1)
            {
                tmpLives = 1;
            }
            if (tmpLives > 4)
            {
                tmpLives = 4;
            }
            string errorMsg = string.Empty;
            errorMsg = ConfigHelper.SaveConfig("Lives", tmpLives.ToString());
            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                return;
            }
            //保存为配置
            errorMsg = ConfigHelper.SaveConfig("TextPool", TextPool);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
