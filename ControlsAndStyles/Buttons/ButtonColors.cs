using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ControlsAndStyles
{
    public static class ButtonColors
    {
        public readonly static GradientStopCollection DisableColor = new GradientStopCollection() {
            new GradientStop(Color.FromArgb(255, 227 , 188, 245),0),
            new GradientStop(Color.FromArgb(148, 213 , 147, 244),1)
        };
        public readonly static GradientStopCollection ActiveColor = new GradientStopCollection() {
            new GradientStop(Color.FromArgb(255, 255, 242, 120),0),
            new GradientStop(Color.FromArgb(148, 255, 169, 89),1)
        };
        public readonly static GradientStopCollection ChooseColor = new GradientStopCollection() {
            new GradientStop(Color.FromArgb(255, 190 , 255, 150),0),
            new GradientStop(Color.FromArgb(148, 78 , 136, 73),1)
        };

    }
}
