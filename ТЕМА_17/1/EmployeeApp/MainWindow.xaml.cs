using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using EmployeeApp.ViewModels;

namespace EmployeeApp
{
    public partial class MainWindow : Window
    {
        public static readonly RoutedUICommand FocusSearchCommand =
            new("FocusSearch", "FocusSearch", typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            Closed += (_, _) => ViewModel.Dispose();
            CommandBindings.Add(new CommandBinding(FocusSearchCommand, (_, _) => SearchBox.Focus()));

            Loaded += (_, _) =>
            {
                EmployeesList.ItemContainerGenerator.StatusChanged += OnGeneratorStatusChanged;
                ViewModel.FilteredEmployees.CollectionChanged += (_, _) => ScheduleListAnimation();
                ViewModel.PropertyChanged += OnVMPropertyChanged;
                ScheduleListAnimation();
            };
        }

        private EmployeeViewModel ViewModel => (EmployeeViewModel)DataContext;

        private void OnVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmployeeViewModel.SelectedEmployee))
                AnimateProfileReveal();
        }

        private void AnimateProfileReveal()
        {
            if (ProfileCard == null || ViewModel.SelectedEmployee == null) return;

            var scale = new ScaleTransform(0.94, 0.94);
            var translate = new TranslateTransform(0, 12);
            var group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(translate);
            ProfileCard.RenderTransform = group;
            ProfileCard.RenderTransformOrigin = new Point(0.5, 0);
            ProfileCard.Opacity = 0;

            var fade = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(320)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var sX = new DoubleAnimation(0.94, 1.0, new Duration(TimeSpan.FromMilliseconds(360)))
            {
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.4 }
            };
            var sY = new DoubleAnimation(0.94, 1.0, new Duration(TimeSpan.FromMilliseconds(360)))
            {
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.4 }
            };
            var slide = new DoubleAnimation(12, 0, new Duration(TimeSpan.FromMilliseconds(360)))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            ProfileCard.BeginAnimation(OpacityProperty, fade);
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, sX);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, sY);
            translate.BeginAnimation(TranslateTransform.YProperty, slide);
        }

        private void OnGeneratorStatusChanged(object? sender, EventArgs e)
        {
            if (EmployeesList.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return;
            ScheduleListAnimation();
        }

        private void ScheduleListAnimation()
        {
            Dispatcher.BeginInvoke(new Action(AnimateAllItems),
                System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private void AnimateAllItems()
        {
            if (EmployeesList == null) return;
            int index = 0;
            foreach (var item in EmployeesList.Items)
            {
                if (EmployeesList.ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem container)
                {
                    AnimateItem(container, index);
                    index++;
                }
            }
        }

        private static void AnimateItem(ListBoxItem container, int index)
        {
            var translate = new TranslateTransform(-30, 0);
            container.RenderTransform = translate;
            container.Opacity = 0;

            var delay = TimeSpan.FromMilliseconds(index * 60);

            var fade = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(380)))
            {
                BeginTime = delay,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            var slide = new DoubleAnimation(-30, 0, new Duration(TimeSpan.FromMilliseconds(420)))
            {
                BeginTime = delay,
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
            };
            container.BeginAnimation(OpacityProperty, fade);
            translate.BeginAnimation(TranslateTransform.XProperty, slide);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Автоматизация учёта сотрудников\nWPF, .NET 9, MVVM\n\n" +
                "Практика 17: анимации, эффекты, интерактивность.\n" +
                "Практика 16: чат (Named Pipes), уведомления (MMF), FileSystemWatcher.\n\n" +
                "Горячие клавиши:\n  Ctrl+N — добавить\n  Ctrl+E — изменить\n" +
                "  Ctrl+D / Del — удалить\n  Ctrl+S — сохранить\n  F5 — обновить\n  Ctrl+F — поиск",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ReportList_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Список сотрудников:");
            sb.AppendLine();
            foreach (var emp in ViewModel.Employees)
                sb.AppendLine($"• {emp.FullName} — {emp.Position} ({emp.Department}), {emp.Salary} руб.");
            sb.AppendLine();
            sb.AppendLine($"Всего: {ViewModel.Employees.Count}");
            sb.AppendLine($"Суммарный фонд оплаты: {ViewModel.Employees.Sum(e => e.Salary)} руб.");
            MessageBox.Show(sb.ToString(), "Отчёт", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public static class StatusAnimation
    {
        public static readonly DependencyProperty IsAvailableProperty =
            DependencyProperty.RegisterAttached("IsAvailable", typeof(bool), typeof(StatusAnimation),
                new PropertyMetadata(true, OnIsAvailableChanged));

        public static bool GetIsAvailable(DependencyObject obj) =>
            (bool)obj.GetValue(IsAvailableProperty);

        public static void SetIsAvailable(DependencyObject obj, bool value) =>
            obj.SetValue(IsAvailableProperty, value);

        private static void OnIsAvailableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement fe) return;
            EnsureScale(fe);
            PlayPop(fe);
            fe.Loaded -= OnFeLoaded;
            fe.Loaded += OnFeLoaded;
            if (fe.IsLoaded) StartPulseIfAvailable(fe);
        }

        private static void OnFeLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe) StartPulseIfAvailable(fe);
        }

        private static ScaleTransform EnsureScale(FrameworkElement fe)
        {
            if (fe.RenderTransform is ScaleTransform existing && !existing.IsFrozen)
                return existing;
            var st = new ScaleTransform(1, 1);
            fe.RenderTransform = st;
            fe.RenderTransformOrigin = new Point(0.5, 0.5);
            return st;
        }

        private static void PlayPop(FrameworkElement fe)
        {
            var st = EnsureScale(fe);
            var pop = new DoubleAnimation(0.5, 1.0, new Duration(TimeSpan.FromMilliseconds(380)))
            {
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.7 }
            };
            st.BeginAnimation(ScaleTransform.ScaleXProperty, pop);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, pop);
        }

        private static void StartPulseIfAvailable(FrameworkElement fe)
        {
            var st = EnsureScale(fe);
            bool available = GetIsAvailable(fe);
            st.BeginAnimation(ScaleTransform.ScaleXProperty, null);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            if (!available) return;
            var pulse = new DoubleAnimation
            {
                From = 1.0,
                To = 1.16,
                Duration = new Duration(TimeSpan.FromMilliseconds(700)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            st.BeginAnimation(ScaleTransform.ScaleXProperty, pulse);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, pulse);
        }
    }
}
