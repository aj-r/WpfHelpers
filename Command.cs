using System;
using System.Windows.Input;

namespace Sharp.Utils.Wpf
{
    /// <summary>
    /// A basic implementation of the ICommand interface where the command parameter is ignored.
    /// </summary>
    public class Command : ICommand
    {
        #region Static Methods

        /// <summary>
        /// Creates a new <see cref="Command"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        /// <param name="canExecute">A predicate that determines whether the command is currently able to execute.</param>
        /// <returns>A <see cref="Command"/>.</returns>
        public Command Create(Action execute, Func<bool> canExecute = null)
        {
            return new Command(execute, canExecute);
        }

        /// <summary>
        /// Creates a new <see cref="Command{T}"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        /// <param name="canExecute">A predicate that determines whether the command is currently able to execute.</param>
        /// <returns>A <see cref="Command{T}"/>.</returns>
        public Command<T> Create<T>(Action<T> execute, Func<T, bool> canExecute = null)
        {
            return new Command<T>(execute, canExecute);
        }

        #endregion

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Creates a new <see cref="Command"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        public Command(Action execute)
            : this(execute, null)
        { }

        /// <summary>
        /// Creates a new <see cref="Command"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        /// <param name="canExecute">A predicate that determines whether the command is currently able to execute.</param>
        public Command(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. This parameter is ignored by the <see cref="Command"/> class.</param>
        /// <returns><value>true</value> if this command can be executed; otherwise, <value>false</value>.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command. This parameter is ignored by the <see cref="Command"/> class.</param>
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

        /// <summary>
        /// Creates a new <see cref="Command{T}"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        public Command(Action<T> execute)
            : this(execute, null)
        { }

        /// <summary>
        /// Creates a new <see cref="Command{T}"/> instance.
        /// </summary>
        /// <param name="execute">The action to perform when the command is executed.</param>
        /// <param name="canExecute">A predicate that determines whether the command is currently able to execute.</param>
        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns><value>true</value> if this command can be executed; otherwise, <value>false</value>.</returns>
        public bool CanExecute(object parameter)
        {
            ValidateParameterType(parameter);

            return (canExecute != null) ? canExecute((T)parameter) : true;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
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
