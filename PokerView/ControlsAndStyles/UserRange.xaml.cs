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
    /// Логика взаимодействия для UserRange.xaml
    /// </summary>
    public partial class UserRange : UserControl
    {
        public UserRange()
        {
            InitializeComponent();

            ChoicePair += OnPairChanged;

            ChoiceSuitPair += OnSuitPairChanged;

            SuitRangeChanged += OnSuitRangeChanged;

            controlsForCurrent.Add(currentSlider);
            controlsForCurrent.Add(currentTextBox);
            controlsForCurrent.Add(currentButton);

            controlsForSelected.Add(selectedSlider);
            controlsForSelected.Add(selectedTextBox);
            controlsForSelected.Add(selectedButton);

            currentTextBox.Text = $"{currentSlider.Value}";
            selectedTextBox.Text = $"{currentSlider.Value}";
        }

        public ChoiceButton SelectedPair { get => range.SelectedButton; }
        public SuitButton SelectedSuitPair { get => suitRange.SelectedButton; }

        public HashSet<ChoiceButton> Range { get => range.SelectedButtons; }

        public string PairRangeLabel { get => rangeLabel.Content.ToString(); set => rangeLabel.Content = value; }
        public string SuitRangeLabel { get => suitRangeLabel.Content.ToString(); set => suitRangeLabel.Content = value; }

        private List<Control> controlsForSelected = new List<Control>();
        private List<Control> controlsForCurrent = new List<Control>();

        public static RoutedEvent ChoicePairEvent = EventManager.RegisterRoutedEvent("ChoicePair",
           RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserRange));

        public event RoutedEventHandler ChoicePair
        {
            add { AddHandler(ChoicePairEvent, value); }
            remove { RemoveHandler(ChoicePairEvent, value); }
        }

        public static RoutedEvent ChoiceSuitPairEvent = EventManager.RegisterRoutedEvent("ChoiceSuitPair",
           RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserRange));

        public event RoutedEventHandler ChoiceSuitPair
        {
            add { AddHandler(ChoiceSuitPairEvent, value); }
            remove { RemoveHandler(ChoiceSuitPairEvent, value); }
        }


        public static RoutedEvent SuitRangeEvent = EventManager.RegisterRoutedEvent("SuitRangeChanged",
          RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserRange));

        public event RoutedEventHandler SuitRangeChanged
        {
            add { AddHandler(SuitRangeEvent, value); }
            remove { RemoveHandler(SuitRangeEvent, value); }
        }


        private void Selected_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in suitRange.SelectedButtons)
            {
                item.Probability = (int)selectedSlider.Value;

            }
            if (selectedSlider.Value == 0)
            {
                suitRange.SelectedButtons.Clear();
            }

            OnSuitRangeChanged(sender, null);
        }

        private void Current_Button_Click(object sender, RoutedEventArgs e)
        {
            suitRange.SelectedButton.Probability = (int)currentSlider.Value;

            if (currentSlider.Value == 0)
            {
                suitRange.SelectedButtons.Remove(suitRange.SelectedButton);
            }

            suitRange.SelectedButton.State = ButtonState.Selected;

            selectedSlider.Value = (int)suitRange.AverageProbability;
            selectedTextBox.Text = $"{suitRange.AverageProbability:f3}";

            suitRange.CancelSelectedButton();

            OnSuitPairChanged(sender, null);

            OnSuitRangeChanged(sender, null);
        }

        bool textblockChange = false;

        private void selectedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!textblockChange)
                selectedTextBox.Text = $"{e.NewValue}";
            textblockChange = false;
        }

        private void currentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!textblockChange)
                currentTextBox.Text = $"{e.NewValue}";
            textblockChange = false;
        }

        private void selectedTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            selectedSlider.Value = ParseNewValue(selectedTextBox, e);
        }
        private void currentTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            currentSlider.Value = ParseNewValue(currentTextBox, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            range.Owner = this;
            suitRange.Owner = this;
            controlsForSelected.ForEach(x => x.IsEnabled = false);
            controlsForCurrent.ForEach(x => x.IsEnabled = false);

        }

        private void OnSuitRangeChanged(object sender, RoutedEventArgs e)
        {

            selectedSlider.Value = (int)suitRange.AverageProbability;
            selectedTextBox.Text = $"{suitRange.AverageProbability:f3}";

            range.SelectedButton.HandState = GetPairState();
        }

        public void OnSuitPairChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedSuitPair == null)
            {
                controlsForCurrent.ForEach(x => x.IsEnabled = false);
                currentSlider.Value = Probabilities.Min;
            }
            else
            {
                controlsForCurrent.ForEach(x => x.IsEnabled = true);
                currentSlider.Value = SelectedSuitPair.Probability;
            }
        }

        public void OnPairChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedPair == null)
            {
                suitRange.Clear();

                suitRange.IsEnabled = false;
                controlsForCurrent.ForEach(x => x.IsEnabled = false);
                controlsForSelected.ForEach(x => x.IsEnabled = false);
            }
            else
            {
                suitRange.CancelSelectedButton();

                suitRange.Refresh(SelectedPair);
                selectedTextBox.Text = $"{suitRange.AverageProbability:f3}";
                suitRange.IsEnabled = true;
                controlsForCurrent.ForEach(x => x.IsEnabled = false);
                controlsForSelected.ForEach(x => x.IsEnabled = true);
            }
        }

        private int ParseNewValue(TextBox textBox, TextCompositionEventArgs e)
        {
            if (textBox.Text.IndexOfAny(new char[] { ',', '.' }) != -1)
            {
                textBox.Text = "";
            }
            int prob;
            if (!int.TryParse(textBox.Text + e.Text, out prob) || (prob > 100 || prob < 0))
            {
                e.Handled = true;
                double dprob;
                double.TryParse(textBox.Text, out dprob);
                return (int)dprob;
            }
            else { textblockChange = true; }

            return prob;
        }

        private List<SuitPairState> GetPairState()
        {
            List<SuitPairState> pairState = new List<SuitPairState>();

            foreach (SuitButton item in suitRange.SelectedButtons)
            {
                if (item.IsEnabled && item.State != ButtonState.Disable)
                {
                    pairState.Add(new SuitPairState(item.LeftSuit, item.RightSuit, item.Probability));
                }

            }
            return pairState;
        }

    }
}
