using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeApp.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object?, Task> execute;
        private readonly Predicate<object?>? canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(_ => { execute(); return Task.CompletedTask; },
                   canExecute == null ? null : new Predicate<object?>(_ => canExecute()))
        {
        }

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
            : this(p => { execute(p); return Task.CompletedTask; }, canExecute)
        {
        }

        public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
            : this(_ => execute(),
                   canExecute == null ? null : new Predicate<object?>(_ => canExecute()))
        {
        }

        public RelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

        public async void Execute(object? parameter)
        {
            try { await execute(parameter); }
            catch (Exception ex) { App.Log("Command failed: " + ex); }
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
