using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfHelpers
{
    /// <summary>
    /// Contains data relating to the drag portion of a drag-and-drop operation.
    /// </summary>
    public class DragDefinition
    {
        public UIElement DragSource { get; set; }
        public object Data { get; set; }
        public DragDropEffects AllowedEffects { get; set; }
        public string GroupName { get; set; }
    }
}
