using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// Contains data relating to the drop portion of a drag-and-drop operation.
    /// </summary>
    public class DropDefinition
    {
        public UIElement DropTarget { get; set; }
        public DragDropEffects AllowedEffects { get; set; }
        public string GroupName { get; set; }
        public bool AllowSelfDrop { get; set; }
    }
}
