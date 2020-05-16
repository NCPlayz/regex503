using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace regex503
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test_TextChanged()
        {
            TextPointer start = this.TestBox.Document.ContentStart;

            TextRange allRange = new TextRange(start, this.TestBox.Document.ContentEnd);
            allRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            allRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

            while (start != null && start.CompareTo(TestBox.Document.ContentEnd) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    Match m = Regex.Match(start.GetTextInRun(LogicalDirection.Forward), RegexBox.Text);

                    TextRange toHighlight = new TextRange(start.GetPositionAtOffset(m.Index, LogicalDirection.Forward), start.GetPositionAtOffset(m.Index + m.Length, LogicalDirection.Backward));
                    toHighlight.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
                    toHighlight.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Black);

                    start = toHighlight.End;
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        // Overloads to avoid using TextChanged and recursing 
        private void TestBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.Test_TextChanged();
        }

        private void TestBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.Test_TextChanged();
        }

        private void TestBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            this.Test_TextChanged();
        }

        private void Regex_Validate()
        {
            try
            {
                Regex.Match("", this.RegexBox.Text);
            }
            catch (ArgumentException err)
            {
                this.Errors.Text = err.Message;
                return;
            }
            this.Errors.Text = "";

            // Update the text highlighting
            this.Test_TextChanged();
        }

        private void RegexBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.Regex_Validate();
        }

        private void RegexBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.Regex_Validate();
        }

        private void RegexBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            this.Regex_Validate();
        }
    }
}
