using System;
using System.Timers;
using System.Windows;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Contains data relating to the drag portion of a drag-and-drop operation.
    /// </summary>
    public class DragDefinition
    {
        /// <summary>
        /// Gets or sets the UI element from which the drag operation was initiated.
        /// </summary>
        public UIElement DragSource { get; set; }

        /// <summary>
        /// Gets or sets the data being dragged.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the DragDropEffects that are allowed in the current drag operation.
        /// </summary>
        public DragDropEffects AllowedEffects { get; set; }

        /// <summary>
        /// Gets or sets the group name for the current drag operation.
        /// If this is set, then the current drag data can only be dropped on a drop target with the same group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets a timer used to delay the start of the drag operation.
        /// This is used to prevent a drag operation from starting when the user just wanted to click on the object.
        /// </summary>
        public Timer StartDragTimer { get; set; }

        /// <summary>
        /// Gets or sets whether the StartDragTimer delay has elapsed.
        /// </summary>
        public bool TimerElapsed { get; set; }
    }
}
