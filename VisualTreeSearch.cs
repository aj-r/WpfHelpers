using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SharpUtils.Wpf
{
    public static class VisualTreeSearch
    {
        /// <summary>
        /// Finds all elements of the specified type in the <see cref="System.Windows.DependencyObject"/>'s visual tree using a breadth-first search.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="root">The object to search in.</param>
        /// <returns>A list of elements that match the criteria.</returns>
        public static IEnumerable<T> Find<T>(this DependencyObject root) where T : DependencyObject
        {
            return root.Find<T>(false, true);
        }

        /// <summary>
        /// Finds all elements of the specified type in the <see cref="System.Windows.DependencyObject"/>'s visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="root">The object to search in.</param>
        /// <param name="depthFirst">True to do a depth-first search; false to do a breadth-first search.</param>
        /// <param name="includeRoot">True to include the root element in the search; false to exclude it.</param>
        /// <returns>A list of elements that match the criteria.</returns>
        public static IEnumerable<T> Find<T>(this DependencyObject root, bool depthFirst, bool includeRoot) where T : DependencyObject
        {
            if (includeRoot)
            {
                var depRoot = root as T;
                if (depRoot != null)
                    yield return depRoot;
            }

            var searchObjects = new LinkedList<DependencyObject>();
            searchObjects.AddFirst(root);

            while (searchObjects.First != null)
            {
                var parent = searchObjects.First.Value;
                var count = VisualTreeHelper.GetChildrenCount(parent);
                var insertAfterNode = depthFirst ? searchObjects.First : searchObjects.Last;

                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    var depChild = child as T;
                    if (depChild != null)
                        yield return depChild;
                    insertAfterNode = searchObjects.AddAfter(insertAfterNode, child);
                }
                searchObjects.RemoveFirst();
            }
        }

        /// <summary>
        /// Finds the first element of the specified type that matches the specified uid in the <see cref="System.Windows.DependencyObject"/>'s visual tree using a breadth-first search.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="root">The object to search in.</param>
        /// <param name="uid">The UID of the object to find.</param>
        /// <returns>The first element found that matches the criteria, or null if none was found.</returns>
        public static T FindByUid<T>(this DependencyObject root, string uid) where T : UIElement
        {
            return root.Find<T>().Where(o => o.Uid == uid).FirstOrDefault();
        }

        /// <summary>
        /// Finds the first element of the specified type that matches the specified name in the <see cref="System.Windows.DependencyObject"/>'s visual tree using a breadth-first search.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="root">The object to search in.</param>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>The first element found that matches the criteria, or null if none was found.</returns>
        public static T FindByName<T>(this DependencyObject root, string name) where T : FrameworkElement
        {
            return root.Find<T>().Where(o => o.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Finds the first ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="element">The child object.</param>
        /// <returns>The first ancestor found that matches the criteria, or null if none was found.</returns>
        public static T Ancestor<T>(this DependencyObject element) where T : DependencyObject
        {
            while (element != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(element) ?? LogicalTreeHelper.GetParent(element);
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                    return correctlyTyped;
                element = parent;
            }
            return null;
        }

        /// <summary>
        /// Detemines whether the specified element has the specified ancestor.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="element">The child object.</param>
        /// <param name="ancestor">The ancestor to look for.</param>
        /// <returns>True if the specified element has the specified ancestor; false if not.</returns>
        public static bool HasAncestor(this DependencyObject element, DependencyObject ancestor)
        {
            while (element != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(element) ?? LogicalTreeHelper.GetParent(element);
                if (parent == ancestor)
                    return true;
                element = parent;
            }
            return false;
        }
    }
}
