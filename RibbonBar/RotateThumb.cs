using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RibbonBar
{
    public class RotateThumb : Thumb
    {
        private Point m_Centre;
        private Vector m_StartVector;
        private double m_InitialAngle;
        private Canvas m_Canvas;
        private FrameworkElement m_Element;
        private RotateTransform m_RotateTransform;

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler(RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(RotateThumb_DragStarted);

            Style = (Style)FindResource("RotateThumbStyle");
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            m_Element = DataContext as FrameworkElement;

            if (m_Element != null)
            {
                m_Canvas = VisualTreeHelper.GetParent(m_Element) as Canvas;

                if (m_Canvas != null)
                {
                    m_Centre = m_Element.TranslatePoint(
                        new Point(m_Element.Width / 2,
                                  m_Element.Height / 2),
                                  m_Canvas);

                    Point startPoint = Mouse.GetPosition(m_Canvas);
                    m_StartVector = Point.Subtract(startPoint, m_Centre);

                    m_RotateTransform = m_Element.RenderTransform as RotateTransform;
                    if (m_RotateTransform == null)
                    {
                        m_RotateTransform = new RotateTransform(0);
                        m_Element.RenderTransform = m_RotateTransform;
                        m_InitialAngle = 0;
                    }
                    else
                    {
                        m_InitialAngle = m_RotateTransform.Angle;
                    }

                    // set
                    m_RotateTransform.CenterX = m_Element.Width / 2;
                    m_RotateTransform.CenterY = m_Element.Height / 2;

                    // see if shift is down
                    //bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                    bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                    if (shift)
                    {
                        // need to get general transform as rotate transform is not enough.


                        switch (this.Name)
                        {
                            case "topleft":
                                m_RotateTransform.CenterX = m_Element.Width;
                                m_RotateTransform.CenterY = m_Element.Height;
                                break;
                            case "topright":
                                m_RotateTransform.CenterX = 0;
                                m_RotateTransform.CenterY = m_Element.Height / 2;
                                break;
                            case "bottomleft":
                                m_RotateTransform.CenterX = m_Element.Width;
                                m_RotateTransform.CenterY = 0;
                                break;
                            case "bottomright":
                                m_RotateTransform.CenterX = 0;
                                m_RotateTransform.CenterY = 0;
                                break;

                        }
                    }
                    else // set default transform about centre
                    {
                        m_RotateTransform.CenterX = m_Element.Width / 2;
                        m_RotateTransform.CenterY = m_Element.Height / 2;
                    }

                }
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (m_Element != null && m_Canvas != null)
            {
                Point currentPoint = Mouse.GetPosition(m_Canvas);
                Vector deltaVector = Point.Subtract(currentPoint, m_Centre);

                double angle = Vector.AngleBetween(m_StartVector, deltaVector);

                m_RotateTransform = m_Element.RenderTransform as RotateTransform;
                m_RotateTransform.Angle = m_InitialAngle + angle;
                m_Element.InvalidateMeasure();
            }
        }
    }
}