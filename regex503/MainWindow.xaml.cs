using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using regex503.UserControls;

namespace regex503
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        bool highlighting = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Test_TextChanged()
        {
            if (this.highlighting)
            {
                return;
            }

            this.highlighting = true;
            TextPointer start = this.TestBox.Document.ContentStart;

            TextRange allRange = new TextRange(start, this.TestBox.Document.ContentEnd);
            allRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            allRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

            while (start != null && start.CompareTo(TestBox.Document.ContentEnd) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //MessageBox.Show(this.RegexBox.Text);
                    Match m = Regex.Match(start.GetTextInRun(LogicalDirection.Forward), this.RegexBox.Text);

                    TextRange toHighlight = new TextRange(start.GetPositionAtOffset(m.Index, LogicalDirection.Forward), start.GetPositionAtOffset(m.Index + m.Length, LogicalDirection.Backward));
                    toHighlight.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
                    toHighlight.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Black);

                    start = toHighlight.End;
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
            
            this.highlighting = false;
        }

        private void RegexBox_RegexChanged(object sender, RegexChangedEventArgs e)
        {
            if (e.err != "")
            {
                this.Errors.Text = e.err;
            } else
            {
                // Update the text highlighting
                this.Test_TextChanged();
            }
        }

        private void TestBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.Test_TextChanged();
        }
    }
}
