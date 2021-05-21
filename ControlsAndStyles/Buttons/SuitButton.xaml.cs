using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SuitButton.xaml
    /// </summary>

    public partial class SuitButton : UserControl
    {
        public SuitButton()
        {
            InitializeComponent();
            button.Fill = new LinearGradientBrush(ButtonColors.DisableColor, 0);

        }

        public SuitButtonType Type { get; set; }
        public SuitPairState PairState = new SuitPairState();

        public void Reset()
        {
            State = ButtonState.Disable;
            Probability = 0;
        }


        ButtonState state;


        LinearGradientBrush brush = new LinearGradientBrush();

        ButtonState previosState;
        public ButtonState State
        {
            get => state;
            set
            {
                switch (value)
                {
                    case ButtonState.Enable:
                        brush.GradientStops = ButtonColors.ActiveColor;
                        button.Fill = brush;
                        break;
                    case ButtonState.Disable:
                        brush.GradientStops = ButtonColors.DisableColor;
                        button.Fill = brush;
                        break;
                    case ButtonState.Selected:
                        brush.GradientStops = ButtonColors.ChooseColor;
                        button.Fill = brush;
                        break;
                }

                if (state != ButtonState.Selected)
                    previosState = state;
                state = value;
            }
        }

        public Suits LeftSuit { get => PairState.LeftSuit; set => PairState.LeftSuit = value; }
        public Suits RightSuit { get => PairState.RightSuit; set => PairState.RightSuit = value; }

        public string LeftCard
        {
            get => leftCard; set
            {
                leftCard = value;
                Text = $"{LeftCard}{Utilits.SuitsToString[LeftSuit]} " +
                    $"{RightCard}{Utilits.SuitsToString[RightSuit]}";
            }
        }
        public string RightCard
        {
            get => rightCard; set
            {
                rightCard = value;
                Text = $"{LeftCard}{Utilits.SuitsToString[LeftSuit]} " +
                    $"{RightCard}{Utilits.SuitsToString[RightSuit]}";
            }
        }

        public string Text { get => label.Content.ToString(); set => label.Content = value; }

        private string leftCard;
        private string rightCard;

        public int Probability
        {
            get => PairState.Probability; set
            {
                if (value == 0)
                {
                    if (State == ButtonState.Selected)
                    {
                        previosState = ButtonState.Disable;
                    }
                    else { State = ButtonState.Disable; }
                }
                else
                {
                    State = ButtonState.Enable;
                }
                PairState.Probability = value;

                if (State == ButtonState.Enable || (State == ButtonState.Selected && previosState != ButtonState.Disable))
                {
                    button.Fill.Opacity = 0.2 + PairState.Probability * 0.8 / 100;
                }
                else { button.Fill.Opacity = 1; }
            }
        }


        public void AcceptChanges()
        {
            State = previosState;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Text = $"{LeftCard}{Utilits.SuitsToString[LeftSuit]} " +
                $"{RightCard}{Utilits.SuitsToString[RightSuit]}";
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as SuitButton;

            if (button == null) { return; }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Probability == Probabilities.Min)
                {
                    Probability = Probabilities.Max;
                }
                State = ButtonState.Enable;
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                State = ButtonState.Disable;

            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                State = State == ButtonState.Selected ? previosState : ButtonState.Selected;
            }
        }
    }
}
