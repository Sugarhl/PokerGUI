using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SuitBoard.xaml
    /// </summary>
    public partial class SuitBoard : UserControl
    {
        public SuitBoard()
        {
            InitializeComponent();

            suitButtons[0, 0] = button00;
            suitButtons[0, 1] = button01;
            suitButtons[0, 2] = button02;
            suitButtons[0, 3] = button03;

            suitButtons[1, 0] = button10;
            suitButtons[1, 1] = button11;
            suitButtons[1, 2] = button12;
            suitButtons[1, 3] = button13;

            suitButtons[2, 0] = button20;
            suitButtons[2, 1] = button21;
            suitButtons[2, 2] = button22;
            suitButtons[2, 3] = button23;

            suitButtons[3, 0] = button30;
            suitButtons[3, 1] = button31;
            suitButtons[3, 2] = button32;
            suitButtons[3, 3] = button33;

        }

        SuitButton[,] suitButtons = new SuitButton[4, 4];

        HashSet<SuitButton> buttons = new HashSet<SuitButton>();

        SuitButton current = new SuitButton();

        HashSet<SuitButton> currentRange = new HashSet<SuitButton>();

        int activeButtons = 0;

        public UserRange Owner { get => owner; set { if (owner == null) { owner = value; } } }
        public double AverageProbability
        {
            get
            {
                double prob = 0;
                foreach (var item in SelectedButtons)
                {
                    prob += item.Probability;
                }
                if (SelectedButtons.Count != 0)
                {
                    return prob / SelectedButtons.Count;
                }
                else { return 0; }
            }
        }

        public SuitButton SelectedButton { get => selectedButton; set => selectedButton = value; }
        public HashSet<SuitButton> SelectedButtons { get => buttons; }

        private SuitButton selectedButton;

        private UserRange owner;


        bool longChoice = false;
        bool isAddAction = false;


        private void SuitButton_MouseEnter(object sender, MouseEventArgs e)
        {

            if (!longChoice || !(sender as SuitButton).IsEnabled) { return; }

            var button = e.Source as SuitButton;
            if (isAddAction)
            {
                if (!currentRange.Contains(button))
                    AddButton(button);
            }
            else if (button.State == ButtonState.Enable)
            {
                RemoveButton(button);
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = e.Source as SuitButton;

            if (button == null) { return; }

            if (e.LeftButton == MouseButtonState.Pressed)
                LeftClick(button);
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                RightClick(button);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                MiddleClick(button);
            }
        }

        private void MiddleClick(SuitButton button)
        {
            bool again = button == SelectedButton;
            CancelSelectedButton();
            if (!again)
            {
                SelectedButton = button;
            }
            else
            {
                button.AcceptChanges();
            }

            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoiceSuitPairEvent, this));
        }

        private void RightClick(SuitButton button)
        {
            CancelSelectedButton();
            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoiceSuitPairEvent, this));
            isAddAction = false;
            longChoice = true;

            if (SelectedButtons.Contains(button))
            {
                RemoveButton(button);
            }
        }

        private void LeftClick(SuitButton button)
        {
            CancelSelectedButton();

            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoiceSuitPairEvent, this));

            isAddAction = true;
            longChoice = true;

            if (!currentRange.Contains(button))
            {
                AddButton(button);
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((e.GetPosition(sender as SuitBoard).X <= 0 || e.GetPosition(sender as SuitBoard).X >= ActualWidth) ||
                    (e.GetPosition(sender as SuitBoard).Y <= 0 || e.GetPosition(sender as SuitBoard).Y >= ActualHeight))
            {
                if (longChoice)
                {
                    Grid_MouseUp(this, null);
                }
            }
        }

        public void CancelSelectedButton()
        {
            SelectedButton?.AcceptChanges();
            if (SelectedButton?.State == ButtonState.Enable)
            {
                SelectedButtons.Add(SelectedButton);
            }
            SelectedButton = null;
        }

        private void RemoveButton(SuitButton button)
        {
            button.Probability = Probabilities.Min;
            buttons.Remove(button);
        }

        private void AddButton(SuitButton button)
        {

            if (button.Probability == 0)
                button.Probability = Probabilities.Max;
            currentRange.Add(button);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            longChoice = false;
            isAddAction = false;
            foreach (var item in currentRange)
            {
                SelectedButtons.Add(item);
            }
            currentRange.Clear();

            owner.RaiseEvent(new RoutedEventArgs(UserRange.SuitRangeEvent, this));
        }

        public void Clear()
        {
            SelectedButtons.Clear();

            foreach (var item in suitButtons)
            {
                item.Reset();
            }
        }

        public void Refresh(ChoiceButton selected)
        {

            Clear();

            setButtons(selected);

            switch (selected.PairType)
            {
                case PairType.Different:
                    foreach (SuitButton button in grid.Children)
                    {
                        button.IsEnabled = (button.Type == SuitButtonType.Diagonal) ? false : true;
                        activeButtons = 12;
                    }
                    break;

                case PairType.Equal:
                    foreach (SuitButton button in grid.Children)
                    {
                        button.IsEnabled = (button.Type == SuitButtonType.Diagonal) ? true : false;
                        activeButtons = 4;
                    }
                    break;

                case PairType.Double:
                    foreach (SuitButton button in grid.Children)
                    {
                        button.IsEnabled = (button.Type != SuitButtonType.Top) ? false : true;
                        activeButtons = 6;
                    }
                    break;

                default:
                    foreach (SuitButton button in grid.Children)
                    {
                        button.IsEnabled = false;
                    }
                    break;

            }
        }

        private void setButtons(ChoiceButton selected)
        {
            foreach (var item in suitButtons)
            {
                item.LeftCard = selected.LeftCard;
                item.RightCard = selected.RightCard;
            }
            foreach (var item in selected.HandState)
            {
                suitButtons[Utilits.SuitToInt[item.LeftSuit], Utilits.SuitToInt[item.RightSuit]].Probability = item.Probability;
                if (item.Probability > Probabilities.Min)
                {
                    suitButtons[Utilits.SuitToInt[item.LeftSuit], Utilits.SuitToInt[item.RightSuit]].State = ButtonState.Enable;
                    SelectedButtons.Add(suitButtons[Utilits.SuitToInt[item.LeftSuit], Utilits.SuitToInt[item.RightSuit]]);
                }
            }
        }
    }
}
