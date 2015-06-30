using System.Windows;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// Contains data relating to the drop portion of a drag-and-drop operation.
    /// </summary>
    public class DropDefinition
    {
        /// <summary>
        /// Gets or sets the UI element that can be used as a drop target.
        /// </summary>
        public UIElement DropTarget { get; set; }

        /// <summary>
        /// Gets or sets the allowed effects for the dorp operation.
        /// </summary>
        public DragDropEffects AllowedEffects { get; set; }

        /// <summary>
        /// Gets or sets the group name for the drop operation.
        /// If this is set, then only drag data with the same group name can be dropped on the drop target.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets whether the drop target can be dropped on itself. This applies only if the drop target is also a drag source.
        /// </summary>
        public bool AllowSelfDrop { get; set; }
    }
}
