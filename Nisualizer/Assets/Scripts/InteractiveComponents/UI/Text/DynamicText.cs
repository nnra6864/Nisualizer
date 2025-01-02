using System;

namespace InteractiveComponents.UI.Text
{
    public class DynamicText
    {
        public string Text;
        public float? Interval;
        public Func<string> Func;

        public DynamicText(string text, Func<string> func, float? interval)
        {
            Text = text;
            Func = func;
            Interval = interval;
        }
    }
}