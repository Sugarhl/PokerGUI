using ControlsAndStyles;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        ClientUtilits client = new ClientUtilits("127.0.0.1", 8080);

        Response response;
        bool successfulRecive = true;

        public MainWindow()
        {
            InitializeComponent();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;


        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (successfulRecive)
            {
                playerWinrate.Content = response.PlayerWinrate;
                enemyWinrate.Content = response.EnemyWinrate;
            }
            else
            {
                MessageBox.Show("Не удалось получить ответ сервера.");
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Request request = new Request(tableBoard.Table, playerRange.Range, enemyRange.Range);

            client.SendString(request.ToString());

            successfulRecive = client.ReciveResponse(out response);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0 & e.PreviousSize.Height == 0)
                return;

            double widthCoef = e.NewSize.Width / e.PreviousSize.Width;
            double heigthCoef = e.NewSize.Height / e.PreviousSize.Height;
        }

        private void UserRange_Loaded(object sender, RoutedEventArgs e)
        {
            if (!client.OpenConnection())
            {
                MessageBox.Show("Не удалось подключиться к серверу.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            if (tableBoard.Table.Count < 3 && tableBoard.Table.Count > 1)
            {
                MessageBox.Show("На доске может не менее 3 и не более 5 карт");
                return;
            }

            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show("Расчет шансов еще идет. Подождите пожалуйста.");
                return;
            }
            backgroundWorker.RunWorkerAsync();
        }
    }
}
