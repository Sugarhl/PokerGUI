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
    public partial class HandBoard : UserControl
    {
        public HandBoard()
        {
            InitializeComponent();
        }

        private bool longChoice = false;
        private bool isAddAction = false;

        private HashSet<ChoiceButton> selectedButtons = new HashSet<ChoiceButton>();

        private HashSet<ChoiceButton> currentRange = new HashSet<ChoiceButton>();

        private UserRange owner;
        public List<string> Hand
        {
            get
            {
                var quevry = from button in selectedButtons
                             select button.Text;
                return quevry.ToList();
            }
        }
        public UserRange Owner { get => owner; set { if (owner == null) { owner = value; } } }

        public ChoiceButton SelectedButton { get => selectedButton; set => selectedButton = value; }
        public HashSet<ChoiceButton> SelectedButtons { get => selectedButtons; }

        private ChoiceButton selectedButton;


        private void ChoiceButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!longChoice)
            {
                return;
            }

            var button = e.Source as ChoiceButton;
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

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (isAddAction)
            {
                foreach (var item in currentRange)
                {
                    SelectedButtons.Add(item);
                }
            }
            currentRange.Clear();
            longChoice = false;
            isAddAction = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = e.Source as ChoiceButton;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LeftClick(button);
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                MiddleClick(button);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                RightClick(button);
            }
        }

        private void RightClick(ChoiceButton button)
        {
            bool again = button == SelectedButton;
            CancelSelectedButton();
            if (!again)
            {
                SelectedButton = button;
                SelectedButton.State = ButtonState.Selected;
            }

            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoicePairEvent, button));
        }

        private void MiddleClick(ChoiceButton button)
        {
            CancelSelectedButton();
            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoicePairEvent, button));
            isAddAction = false;
            longChoice = true;

            if (SelectedButtons.Contains(button))
            {
                RemoveButton(button);
            }
        }

        private void LeftClick(ChoiceButton button)
        {
            CancelSelectedButton();
            owner.RaiseEvent(new RoutedEventArgs(UserRange.ChoicePairEvent, button));
            isAddAction = true;
            longChoice = true;

            if (!currentRange.Contains(button))
            {
                AddButton(button);
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((e.GetPosition(sender as HandBoard).X <= 0 || e.GetPosition(sender as HandBoard).X >= ActualWidth) ||
                    (e.GetPosition(sender as HandBoard).Y <= 0 || e.GetPosition(sender as HandBoard).Y >= ActualHeight))
            {
                if (longChoice)
                {
                    Grid_MouseUp(this, null);
                }
            }
        }


        private void CancelSelectedButton()
        {
            if (SelectedButton != null && SelectedButton.HandState.Count > 0)
            {
                selectedButtons.Add(selectedButton);
            }

            SelectedButton?.AcceptChanges();
            SelectedButton = null;
        }

        private void RemoveButton(ChoiceButton button)
        {
            button.HandState.Clear();
            button.State = ButtonState.Disable;
            SelectedButtons.Remove(button);
        }

        private void AddButton(ChoiceButton button)
        {
            if (button.HandState.Count == 0)
            {
                button.HandState = PairState.Defult(button.PairType);
            }
            currentRange.Add(button);
        }

    }
}
