using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

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

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextPointer start = this.TestBox.Document.ContentStart;

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
    }
}
