using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphLabs.CommonUI.Helpers
{
    /// <summary> Альтернатива обычному MouseMove для ускорения быстродействия UI</summary>
    /// <remarks> http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/ </remarks>
    public class ThrottledMouseMoveEvent
    {
        private bool _awaitingRender;

        private readonly UIElement _element;

        /// <summary> Конструктор </summary>
        /// <param name="element"> Элемент, к которому привязываем событие </param>
        public ThrottledMouseMoveEvent(UIElement element)
        {
            _element = element;
            _awaitingRender = false;
            element.MouseMove += ElementMouseMove;
        }

        /// <summary> Событие, фактически происходящее при перемещении курсора </summary>
        public event MouseEventHandler MouseMove;

        private void ElementMouseMove(object sender, MouseEventArgs e)
        {
            if (_awaitingRender) { return; }
            OnThrottledMouseMove(e);
            _awaitingRender = true;
            CompositionTarget.Rendering += CompositionTargetRendering;
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            _awaitingRender = false;
            CompositionTarget.Rendering -= CompositionTargetRendering;
        }

        /// <summary> Raises the MouseMove event </summary>
        protected void OnThrottledMouseMove(MouseEventArgs args)
        {
            if (MouseMove != null)
            {
                MouseMove(_element, args);
            }
        }
    }
}
