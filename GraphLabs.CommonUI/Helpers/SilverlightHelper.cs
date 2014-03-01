using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GraphLabs.CommonUI.Helpers
{
    /// <summary> Вспомагательные методы </summary>
    public static class SilverlightHelper
    {
        /// <summary> Подписка на изменение DependencyProperty </summary>
        /// <param name="name">Имя свойства</param>
        /// <param name="propertyType">Тип свойства</param>
        /// <param name="ownerType">Тип объекта, которому принадлежит свойство</param>
        /// <param name="element">Объект, которому принадлежит свойство</param>
        /// <param name="callback">Callback на изменение этого свойства</param>
        /// <remarks>Это вместо отсутствующего в silverlight'е OverrideMetadata</remarks>
        public static void RegisterForNotification(
            string name, 
            Type propertyType,
            Type ownerType,
            FrameworkElement element, 
            PropertyChangedCallback callback)
        {
            var binding = new Binding(name) { Source = element };
            var property = DependencyProperty.RegisterAttached(
                "ListenAttached" + name,
                propertyType,
                ownerType,
                new PropertyMetadata(callback));

            element.SetBinding(property, binding);
        }


        #region Animation

        private static Timeline GetAnimation(object animateTo, Duration duration)
        {
            Timeline animation;

            if (animateTo is double)
                animation = new DoubleAnimation { To = (double)animateTo };
            else if (animateTo is Color)
                animation = new ColorAnimation { To = (Color)animateTo };
            else
                throw new NotSupportedException();

            animation.Duration = duration;
            return animation;
        }

        /// <summary> Возвращает Storyboard анимации для заданного свойства заданного объекта </summary>
        /// <param name="targetObject">Целевой объект</param>
        /// <param name="targetProperty">Свойство целевого объекта</param>
        /// <param name="animateTo">Целевое значение целевого свойства</param>
        /// <param name="duration">Продолжительность анимации</param>
        /// <param name="onCompletedAction">Действие при завершении анимации</param>
        public static Storyboard GetStoryboard(DependencyObject targetObject, string targetProperty,
            object animateTo, double duration, Action<object, EventArgs> onCompletedAction)
        {
            var dur = new Duration(TimeSpan.FromSeconds(duration));
            var animation = GetAnimation(animateTo, dur);
            var storyboard = new Storyboard { Duration = dur };
            Storyboard.SetTarget(storyboard, targetObject);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(targetProperty));
            storyboard.Children.Add(animation);
            storyboard.AutoReverse = false;
            if (onCompletedAction != null)
                storyboard.Completed += (sender, args) => onCompletedAction(sender, args);
            return storyboard;
        }

        #endregion // Animation


        /// <summary> Находит визуальный корень для заданного элемента </summary>
        /// <remarks> Если переданный элемент не имеет визуальных предков, вовращает null </remarks>
        /// <requires> element != null </requires>
        public static UIElement FindVisualRoot(UIElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            var newParent = VisualTreeHelper.GetParent(element) as UIElement;
            UIElement parent = null;

            while (newParent != null)
            {
                parent = newParent;
                newParent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return parent;
        }

        /// <summary> Размер строки </summary>
        public static Size GetTextSize(string text, 
            FontFamily fontFamily = null,
            int? fontSize = null,
            FontStyle? fontStyle = null)
        {
            var txtMeasure = new TextBlock();

            if (fontSize.HasValue)
                txtMeasure.FontSize = fontSize.Value;
            if (fontFamily != null)
                txtMeasure.FontFamily = fontFamily;
            if (fontStyle.HasValue)
                txtMeasure.FontStyle = fontStyle.Value;
            txtMeasure.Text = text;

            var size = new Size(txtMeasure.ActualWidth, txtMeasure.ActualHeight);
            return size;
        }
    }
}
