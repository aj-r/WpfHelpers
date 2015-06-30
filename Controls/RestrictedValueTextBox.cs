using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sharp.Utils.Wpf.Controls
{
    /// <summary>
    /// Base class for a text box that only allows a specific type of input.
    /// </summary>
    /// <remarks>
    /// Copied from http://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
    /// </remarks>
    public abstract class RestrictedValueTextBox : TextBox
    {
        /// <summary>
        /// Creates a new <see cref="RestrictedValueTextBox"/> instance.
        /// </summary>
        public RestrictedValueTextBox()
        {
            DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(CheckPasteFormat));
        }

        /// <summary>
        /// When overridden in a derived class, checks the format of the text and determines whether it is valid.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <returns><value>true</value> if the input text is valid; otherwise <value>false</value>.</returns>
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

        /// <summary>
        /// Invoked when an unhandled System.Windows.Input.TextCompositionManager.PreviewTextInput attached
        /// event reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The System.Windows.Input.TextCompositionEventArgs that contains the event data.</param>
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
