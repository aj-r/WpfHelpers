using System;
using System.Windows.Input;

namespace SharpUtils.Wpf
{
    /// <summary>
    /// A basic implementation of the ICommand interface where the parameter is ignored.
    /// </summary>
    public class Command : ICommand
    {
        #region Static Methods

        public Command Create(Action execute, Func<bool> canExecute = null)
        {
            return new Command(execute, canExecute);
        }

        public Command<T> Create<T>(Action<T> execute, Func<T, bool> canExecute = null)
        {
            return new Command<T>(execute, canExecute);
        }

        #endregion

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public Command(Action execute)
            : this(execute, null)
        { }

        public Command(Action execute, Func<bool> canExecute)
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
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public Command(Action<T> execute)
            : this(execute, null)
        { }

        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            ValidateParameterType(parameter);

            return (canExecute != null) ? canExecute((T)parameter) : true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ValidateParameterType(parameter);

            if (execute != null)
                execute((T)parameter);
        }

        #endregion

        private static void ValidateParameterType(object parameter)
        {
            if (parameter is T)
                return;
            var type = typeof(T);
            if (parameter == null && (type.IsClass || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))))
                return;

            throw new ArgumentException("Command parameter must be of type " + type.FullName);
        }
    }
}
