using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SharpUtils.Wpf.Controls
{
    /// <summary>
    /// Base class for a text box that only allows a specific type of input.
    /// </summary>
    /// <remarks>
    /// Copied from http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
    /// </remarks>
    public abstract class RestrictedValueTextBox : TextBox
    {
        public RestrictedValueTextBox()
        {
            DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(CheckPasteFormat));
        }

        protected abstract bool CheckFormat(string text);

        private void CheckPasteFormat(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.Text, true);
            if (isText)
            {
                var text = e.SourceDataObject.GetData(DataFormats.Text) as string;
                if (CheckFormat(text))
                {
                    return;
                }
            }
            e.CancelCommand();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!CheckFormat(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                base.OnPreviewTextInput(e);
            }
        }
    }
}
