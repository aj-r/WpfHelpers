using System;
using System.Windows.Input;

namespace WpfHelpers
{
    /// <summary>
    /// A basic implementation of the ICommand interface where the parameter is ignored.
    /// </summary>
    public class Command : ICommand
    {
        Action execute;
        Func<bool> canExecute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return (canExecute != null) ? canExecute() : true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (execute != null)
                execute();
        }

        #endregion
    }

    /// <summary>
    /// A basic implementation of the ICommand interface that accepts a typed parameter.
    /// </summary>
    public class Command<T> : ICommand
    {
        Action<T> execute;
        Func<bool> canExecute;

        public Command(Action<T> execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return (canExecute != null) ? canExecute() : true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (execute != null)
                execute((T)parameter);
        }

        #endregion
    }
}
