using System;
using System.Windows.Input;

namespace WpfHelpers
{
    /// <summary>
    /// A basic implementation of the ICommand interface where the parameter is ignored.
    /// </summary>
    public class Command : ICommand
    {
        Action action;

        public Command(Action a)
        {
            action = a;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }

        #endregion
    }

    /// <summary>
    /// A basic implementation of the ICommand interface that accepts a typed parameter.
    /// </summary>
    public class Command<T> : ICommand
    {
        Action<T> action;

        public Command(Action<T> a)
        {
            action = a;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action((T)parameter);
        }

        #endregion
    }
}
