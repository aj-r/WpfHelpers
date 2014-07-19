using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WpfHelpers
{
    public static class VisualTreeSearch
    {
        /// <summary>
        /// Does a search of the <see cref="System.Windows.DependencyObject"/>'s visual tree for all elements of the specefied type.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <param name="obj">The object to search in.</param>
        /// <param name="depthFirst">True to do a depth-first search, false to do a breadth-first search. Default is breadth-first.</param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj, bool depthFirst = false) where T : DependencyObject
        {
            if (obj == null)
                yield break;

            var searchObjects = new LinkedList<DependencyObject>();
            searchObjects.AddFirst(obj);

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
    }
}
