using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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

namespace ControlsAndStyles
{
    /// <summary>
    /// Логика взаимодействия для CardButton.xaml
    /// </summary>
    public partial class CardButton : UserControl
    {

        public event RoutedEventHandler Checked
        {
            add { AddHandler(CheckEvent, value); }
            remove { RemoveHandler(CheckEvent, value); }
        }
        public event RoutedEventHandler Unchecked
        {
            add { AddHandler(UncheckEvent, value); }
            remove { RemoveHandler(UncheckEvent, value); }
        }

        public static RoutedEvent CheckEvent = EventManager.RegisterRoutedEvent("Checked",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardButton));

        public static RoutedEvent UncheckEvent = EventManager.RegisterRoutedEvent("Unchecked",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardButton));

        public bool? IsChecked { get => button.IsChecked; set => button.IsChecked = value; }
        public ControlTemplate ButtonTemplate { get => button.Template; set => button.Template = value; }
        public Style ButtonStyle { get => button.Style; set => button.Style = value; }

        public string Text { get => label.Content.ToString(); set => label.Content = value; }

        public int CardCode
        {
            get; set;
        }

        public PairType Type { get; set; }

        public Brush MyBackground { get => button.Background; set => button.Background = value; }

        private void button_Checked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CheckEvent, this));
        }

        private void button_Unchecked(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UncheckEvent, this));
        }

        public CardButton()
        {
            InitializeComponent();
        }
    }
}
