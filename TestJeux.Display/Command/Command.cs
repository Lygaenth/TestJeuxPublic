using System;
using System.Windows.Input;

namespace TestJeux.Business.Command
{
	public class Command<T> : ICommand where T : class
    {
        Action<T> _action;
        public event EventHandler CanExecuteChanged;

        public Command(Action<T> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke((T)parameter);
        }
    }

    public class Command : ICommand
    {
        System.Action _action;
        Predicate<object> _predicate;
        public event EventHandler CanExecuteChanged;

        public Command(System.Action action)
        {
            _action = action;
        }

        public Command(System.Action action, Predicate<object> canExecuteMthd )
        {
            _action = action;
            _predicate = canExecuteMthd;
        }

        public bool CanExecute(object parameter)
        {
            if (_predicate == null)
                return true;
            else
                return _predicate.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
