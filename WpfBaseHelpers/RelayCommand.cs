using System;
using System.Windows.Input;

namespace AR.WPF.Helpers
{
    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        Action _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod;

        /// <summary>
        /// Create Command
        /// </summary>
        /// <param name="executeMethod">Action to execute on command</param>
        public RelayCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Create Command
        /// </summary>
        /// <param name="executeMethod">Action to execute on command</param>
        /// <param name="canExecuteMethod">Determine if command can be executed</param>
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Pull CanExecuteMethod 
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod();
            }
        }
        #endregion
    }

    /// <summary>
    /// Implementation of ICommand with parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RelayCommand<T> : ICommand
    {
        Action<T> _TargetExecuteMethod;
        Func<T, bool> _TargetCanExecuteMethod;

        /// <summary>
        /// Create Command
        /// </summary>
        /// <param name="executeMethod">Action to execute on command</param>
        public RelayCommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Create Command
        /// </summary>
        /// <param name="executeMethod">Action to execute on command</param>
        /// <param name="canExecuteMethod">Determine if command can be executed</param>
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Pull CanExecuteMethod 
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return _TargetCanExecuteMethod(tparm);
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod((T)parameter);
            }
        }
        #endregion
    }
}

