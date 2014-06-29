using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WpfHelpers
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyExpression"></param>
        protected void RaisePropertyChanged(Expression<Func<object>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the name of a property in a lambda expression.
        /// </summary>
        /// <param name="propertyExpression">A lambda expression in the form: () => SomeProperty</param>
        /// <returns>A string that represents the property name.</returns>
        protected string GetPropertyName(Expression<Func<object>> propertyExpression)
        {
            MemberExpression expr;
            if (propertyExpression.Body is UnaryExpression)
                expr = (MemberExpression)((UnaryExpression)propertyExpression.Body).Operand;
            else if (propertyExpression.Body is MemberExpression)
                expr = (MemberExpression)propertyExpression.Body;
            else
                throw new ArgumentException("propertyExpression must be in the form '() => SomeProperty'", "propertyExpression");
            return expr.Member.Name;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
