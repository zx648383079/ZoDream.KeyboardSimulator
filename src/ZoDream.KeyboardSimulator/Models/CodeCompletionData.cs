using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZoDream.KeyboardSimulator.Models
{
    public class CodeCompletionData : ICompletionData
    {
        public CodeCompletionData()
        {

        }
        public CodeCompletionData(string text)
        {
            Text = text;
        }

        public ImageSource? Image => null;

        public string Text { get; set; } = string.Empty;

        public object Content { get; set; } = string.Empty;

        public string FilterFlag { get; set; } = string.Empty;

        public object Description { get; set; }

        public double Priority { get; set; }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment.Offset - 1, completionSegment.Length + 1, Text);
        }
    }
}
