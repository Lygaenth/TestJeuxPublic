using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestJeux.Display.ViewModels.Base
{
    public class BaseViewModel : BindableBase
    {
        /// <summary>
        /// Execute in UI thread
        /// </summary>
        /// <param name="action"></param>
        protected void ExecuteUithread(Action action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }
    }
}
