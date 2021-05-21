using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlsAndStyles
{
    /// <summary>
    /// Логика взаимодействия для TableBoard.xaml
    /// </summary>
    public partial class TableBoard : UserControl
    {
        public TableBoard()
        {
            InitializeComponent();
        }

        private List<CardButton> buttons = new List<CardButton>();

        public List<int> Table
        {
            get
            {
                var quevry = from button in buttons
                             select button.CardCode;
                return quevry.ToList();
            }
        }

        private void Check(object sender, RoutedEventArgs e)
        {

            buttons.Add(e.Source as CardButton);

            if (buttons.Count == 5)
            {
                foreach (CardButton item in grid.Children)
                {
                    if (!buttons.Contains(item))
                    {
                        item.IsEnabled = false;
                    }
                }
            }
        }
        private void Uncheck(object sender, RoutedEventArgs e)
        {
            if (buttons.Count == 5)
            {
                foreach (CardButton item in grid.Children)
                {
                    if (!buttons.Contains(item))
                    {
                        item.IsEnabled = true;
                    }
                }
            }
            buttons.Remove(e.Source as CardButton);
        }
    }
}
