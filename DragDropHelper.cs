using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfHelpers
{
    /// <summary>
    /// Contains helper methods for dragging and dropping controls.
    /// </summary>
    public static class DragDropHelper
    {
        /// <summary>
        /// The DragDefinition for the object that is currently being dragged.
        /// </summary>
        private static DragDefinition currentDragDef;

        private static Dictionary<UIElement, DragDefinition> dragDefinitions = new Dictionary<UIElement, DragDefinition>();
        private static Dictionary<UIElement, DropDefinition> dropDefinitions = new Dictionary<UIElement, DropDefinition>();
        
        /// <summary>
        /// Registers (or modifies the registration for) an object as a drag source, allowing the user to click and drag it around.
        /// </summary>
        /// <param name="element">The element to make draggable</param>
        /// <param name="data">The data that will be dropped if the element is successfully dropped on the drop target.</param>
        /// <param name="dragType">A name for the drag group that the drag source belongs to. The drag source can only be dropped onto a drop target with the same group name.</param>
        public static void RegisterDragSource(UIElement dragSource, object data, string groupName = null, DragDropEffects allowedEffects = DragDropEffects.All)
        {
            DragDefinition dragDef;
            if (!dragDefinitions.TryGetValue(dragSource, out dragDef))
            {
                dragSource.MouseDown += dragSource_MouseDown;
                dragSource.MouseUp += dragSource_MouseUp;
                dragDef = new DragDefinition
                {
                    DragSource = dragSource,
                    Data = data,
                    GroupName = groupName,
                    AllowedEffects = allowedEffects
                };
                dragDefinitions.Add(dragSource, dragDef);
            }
            else
            {
                dragDef.Data = data;
                dragDef.GroupName = groupName;
                dragDef.AllowedEffects = allowedEffects;
            }
        }

        /// <summary>
        /// Unregisters an object as a drag source.
        /// </summary>
        /// <param name="element">The element to make non-draggable</param>
        public static void UnregisterDragSource(UIElement dragSource)
        {
            if (!dragDefinitions.Remove(dragSource))
                return;
            dragSource.MouseDown -= dragSource_MouseDown;
            dragSource.MouseUp -= dragSource_MouseUp;
        }

        /// <summary>
        /// Registers (or modifies the registration for) the object as a drop target, allowing the user to drop draggable items on it.
        /// </summary>
        /// <param name="element">The element to make draggable</param>
        /// <param name="data">The data that will be dropped if the element is successfully dropped on the drop target.</param>
        /// <param name="dragType">A name for the drag group that the </param>
        public static void RegisterDropTarget(UIElement dropTarget, string groupName = null, DragDropEffects allowedEffects = DragDropEffects.All)
        {
            dropTarget.AllowDrop = true;
            DropDefinition dropDef;
            if (!dropDefinitions.TryGetValue(dropTarget, out dropDef))
            {
                dropTarget.DragOver += dropTarget_DragOver;
                dropDef = new DropDefinition
                {
                    DropTarget = dropTarget,
                    GroupName = groupName,
                    AllowedEffects = allowedEffects
                };
                dropDefinitions.Add(dropTarget, dropDef);
            }
            else
            {
                dropDef.GroupName = groupName;
                dropDef.AllowedEffects = allowedEffects;
            }
        }

        /// <summary>
        /// Unregisters an object as a drop target.
        /// </summary>
        /// <param name="element">The element to make non-droppable</param>
        public static void UnregisterDropTarget(UIElement dropTarget)
        {
            if (!dropDefinitions.Remove(dropTarget))
                return;
            dropTarget.DragOver -= dropTarget_DragOver;
        }

        private static void SetCurrentDragSource(UIElement dragSource)
        {
            if (currentDragDef != null)
                currentDragDef.DragSource.MouseMove -= dragSource_MouseMove;

            DragDefinition dragDef;
            if (dragSource == null || !dragDefinitions.TryGetValue(dragSource, out dragDef))
            {
                currentDragDef = null;
                return;
            }
            currentDragDef = dragDef;
            dragSource.MouseMove += dragSource_MouseMove;
        }

        private static void dragSource_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;
            SetCurrentDragSource((UIElement)sender);
            e.Handled = true;
        }

        private static void dragSource_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;
            SetCurrentDragSource(null);
            e.Handled = true;
        }

        private static void dragSource_MouseMove(object sender, MouseEventArgs e)
        {
            var allowedEffects = currentDragDef.AllowedEffects;
            /*bool ctrlKey = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if ((allowedEffects & DragDropEffects.Move) == DragDropEffects.None || ctrlKey)
            {
                // Control key is pressed or move is not allowed. Do copy/link.
                allowedEffects &= ~DragDropEffects.Move;
            }
            else
            {
                // Control key not pressed. Do move.
                allowedEffects &= (DragDropEffects.Move | DragDropEffects.Scroll);
            }*/
            DragDrop.DoDragDrop(currentDragDef.DragSource, currentDragDef.Data, allowedEffects);
            e.Handled = true;
        }

        private static void dropTarget_DragOver(object sender, DragEventArgs e)
        {
            DropDefinition dropDef;
            if (!dropDefinitions.TryGetValue((UIElement)sender, out dropDef))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            if (currentDragDef == null) 
            {
                // Drag from external application.
                if (dropDef.GroupName == null)
                {
                    // Leave effects as-is
                    e.Handled = true;
                    return;
                }
                // If group name is specified, don't allow drag from external application (should we really do this?)
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            // Don't use string.Equals() here because GroupName could be null
            if (dropDef.GroupName != currentDragDef.GroupName)
            {
                // If the drag source is in a different group then don't allow drop.
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            bool ctrlKey = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if ((e.Effects & DragDropEffects.Move) == DragDropEffects.None || ctrlKey)
            {
                // Control key is pressed or move is not allowed. Do copy/link.
                e.Effects &= ~DragDropEffects.Move;
            }
            else
            {
                // Control key not pressed. Do move.
                e.Effects &= (DragDropEffects.Move | DragDropEffects.Scroll);
            }
            e.Handled = true;
        }
    }
}
