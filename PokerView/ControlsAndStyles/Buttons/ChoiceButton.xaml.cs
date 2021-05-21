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
    /// Логика взаимодействия для ChoiceButton.xaml
    /// </summary>
    public partial class ChoiceButton : UserControl
    {
        public ChoiceButton()
        {
            InitializeComponent();
            button.Fill = brush;
        }


        UserHandState handState;

        LinearGradientBrush brush = new LinearGradientBrush(ButtonColors.DisableColor, 1);

        public List<SuitPairState> HandState
        {
            get => handState.PairState;
            set
            {
                if (State == ButtonState.Selected)
                {
                    previosState = (value.Count == 0) ? ButtonState.Disable : ButtonState.Enable;
                }
                else { State = (value.Count == 0) ? ButtonState.Disable : ButtonState.Enable; }
                handState.PairState = value;

                if (handState.PairState.Count != 0)
                {
                    double prop = GetOpacityCoef();
                    brush.Opacity = 0.2 + 0.8 * prop;
                }
                else
                {
                    brush.Opacity = 1;
                }
            }
        }



        public string LeftCard { get; set; }
        public string Text { get => label.Content.ToString(); }
        public string RightCard { get; set; }


        public PairType PairType { get; set; }

        ButtonState state;

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

                previosState = state;
                state = value;
            }
        }

        public void AcceptChanges()
        {
            State = previosState;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            label.Content = $"{LeftCard}{RightCard}";
            if (PairType == PairType.Different)
            {
                label.Content = $"{ label.Content}o";

            }
            if (PairType == PairType.Equal)
            {
                label.Content = $"{ label.Content}s";
            }
            State = ButtonState.Disable;
            handState = new UserHandState(LeftCard, RightCard);
        }
        private double GetOpacityCoef()
        {
            double prop = 0;
            foreach (var item in handState.PairState)
            {
                prop += item.Probability;
            }
            prop /= handState.PairState.Count*100;
            return prop;
        }
    }
}
