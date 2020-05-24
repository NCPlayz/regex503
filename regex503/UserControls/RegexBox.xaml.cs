using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Text.RegularExpressions;

namespace regex503.UserControls
{
    /// <summary>
    /// Interaction logic for RegexBox.xaml
    /// </summary>
    
    public class RegexChangedEventArgs : RoutedEventArgs
    {
        public string err;
        
        public RegexChangedEventArgs(RoutedEvent routedEvent, string err) : base(routedEvent)
        {
            this.err = err;
        }
    }

    public delegate void RegexChangedEventHandler(object sender, RegexChangedEventArgs e);

    public partial class RegexBox : UserControl
    {
        public static readonly RoutedEvent RegexChangedEvent = EventManager.RegisterRoutedEvent(
            "RegexChanged", RoutingStrategy.Bubble, typeof(RegexChangedEventHandler), typeof(RegexBox));

        public RegexBox()
        {
            InitializeComponent();
        }

        public event RegexChangedEventHandler RegexChanged
        {
            add { AddHandler(RegexChangedEvent, value);  }
            remove { RemoveHandler(RegexChangedEvent, value); }
        }

        public string Text
        { 
            get
            {
                //TextRange range = new TextRange(this.TextBox.Document.ContentStart, this.TextBox.Document.ContentEnd);
                //return range.Text;
                return this.TextBox.Text;
            }
        }

        private void Regex_Validate()
        {
            //MessageBox.Show(this.Text);
            RegexChangedEventArgs newEventArgs = new RegexChangedEventArgs(RegexBox.RegexChangedEvent, "");
            try
            {
                Regex.Match("", this.Text);
            }
            catch (ArgumentException err)
            {
                newEventArgs.err = err.Message;
            }

            RaiseEvent(newEventArgs);
        }

        private void RegexBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            this.Regex_Validate();
        }

        private void RegexBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.Regex_Validate();
        }

        private void RegexBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            this.Regex_Validate();
        }
    }
}
