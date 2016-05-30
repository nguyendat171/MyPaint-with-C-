using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Reflection;

namespace RibbonBar
{
    public class RotatingAdorner : Adorner
    {
        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        RotateThumb topLeft, topRight, bottomLeft, bottomRight;

        // To store and manage the adorner's visual children.
        VisualCollection visualChildren;

        private Canvas m_Canvas;
        private FrameworkElement m_Element;

        // Initialize the ResizingAdorner.
        public RotatingAdorner(FrameworkElement element, Canvas canvas)
            : base(element)
        {
            m_Element = element;
            m_Canvas = canvas;

            visualChildren = new VisualCollection(this);

            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            BuildAdornerCorner(ref topLeft, Cursors.SizeAll, element, "topleft");
            BuildAdornerCorner(ref topRight, Cursors.SizeAll, element, "topright");
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeAll, element, "bottomleft");
            BuildAdornerCorner(ref bottomRight, Cursors.SizeAll, element, "bottomright");
        }


        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;


            double gap = 17;

            // handlegap
            //topLeft.Arrange(new Rect(-gap, -gap, adornerWidth, adornerHeight));
            //topRight.Arrange(new Rect(width + gap - adornerWidth, -gap, adornerWidth, adornerHeight));
            //bottomLeft.Arrange(new Rect(-gap, height + gap - adornerHeight, adornerWidth, adornerHeight));
            //bottomRight.Arrange(new Rect(width + gap - adornerWidth, height + gap - adornerWidth, adornerWidth, adornerHeight)); 

            //// original
            topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            //midLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight / 2 - adornerHeight / 2, adornerWidth, adornerHeight));
            // handlegap
            //topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            //topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            //bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            //bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight)); 

            // spaced 
            //topLeft.Arrange(new Rect(-adornerWidth, -adornerHeight, adornerWidth, adornerHeight));
            //topRight.Arrange(new Rect(desiredWidth, -adornerHeight, adornerWidth, adornerHeight));
            //bottomLeft.Arrange(new Rect(-adornerWidth, desiredHeight, adornerWidth, adornerHeight));
            //bottomRight.Arrange(new Rect(desiredWidth, desiredHeight, adornerWidth, adornerHeight));
            // Return the final size.
            return finalSize;
        }


        // from http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/d1040026-bbfc-42e7-b788-7eec3bc76a84/
        // not used as resources are not saved in debug version, only release version
        // can't figure out why this is happening
        internal static Stream LoadResourceStream(Assembly assembly, string resId)
        {

            string basename = System.IO.Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) + ".g";
            ResourceManager resourceManager = new ResourceManager(basename, assembly);

            // resource names are lower case and contain
            // only forward slashes

            return (resourceManager.GetObject(resId.ToLower().Replace('\\', '/')) as Stream);

        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref RotateThumb cornerThumb, Cursor customizedCursor, UIElement m_Element, string name)
        {
            if (cornerThumb != null) return;

            cornerThumb = new RotateThumb();
            cornerThumb.DataContext = m_Element;

            // set cursor and name
            // cursor must be embedded resource. I know this is naughty, but its the only way I can get it to work
            string curName = "Ribbon.Resources.Cursors.rotate.cur";
            Stream stream = GetType().Assembly.GetManifestResourceStream(curName);
            cornerThumb.Cursor = new Cursor(stream);
            cornerThumb.Name = name;

            visualChildren.Add(cornerThumb);
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        void EnforceSize(FrameworkElement m_Element)
        {
            if (m_Element.Width.Equals(Double.NaN))
                m_Element.Width = m_Element.DesiredSize.Width;
            if (m_Element.Height.Equals(Double.NaN))
                m_Element.Height = m_Element.DesiredSize.Height;

            FrameworkElement parent = m_Element.Parent as FrameworkElement;
            if (parent != null)
            {
                m_Element.MaxHeight = parent.ActualHeight;
                m_Element.MaxWidth = parent.ActualWidth;
            }
        }
        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}


