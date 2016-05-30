using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using Microsoft.Win32;
using System.IO;
namespace RibbonBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        Point startPoint;
        bool DuongThang = false;
        bool HinhChuNhat = false;
        bool HinhVuong = false;
        bool HinhEllipse = false;
        bool HinhTron = false;
        bool MuiTen = false;
        bool Ngoisao = false;
        bool TraiTim = false;
        
        bool ChonHinh = false;
        bool Hinhfree = false;
        bool isMouseDowned = false;
        Line lastLine;
        Rectangle lastRectangle;
        Ellipse lastEllipse;
        Polygon lastArrow;
        Polygon lastStar;
        System.Windows.Shapes.Path lastHeart;
        Brush  color2;
        static Color color;
        int size;
        int dragMode;
        int hinh = 1;
        bool startPaint = false;

        DoubleCollection lineType = null;
        SolidColorBrush myBrush = new SolidColorBrush(color);

        FrameworkElement slElement = null;
       // RotateTransform rotator = null;// đối tượng xoay hình

         // Thuộc tính chưa vị trị con trỏ hiện tại
        Point currentPoint = new Point(0, 0);
        
        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;

        AdornerLayer aLayer;// xử lý kéo thả
       UIElement selectedElement = null;
       
        bool _isDown;
        bool _isDragging;
        bool MouseInside = false;
        bool selected = false;
        bool Resize = false;
        //text
        int fontsize;
        int font;

        int tag;
        int tag2;
        public MainWindow()
        {
            InitializeComponent();
            Canvas clone = paintCanvas;
        }

        public void MauTo()
        {
            if (FillBlack.IsChecked == true) //mau den
            {
                color2 = Brushes.Black;
            }
            if (FillRed.IsChecked == true) //mau do
            {
                color2 = Brushes.Red;
            }

            if (FillYellow.IsChecked == true) //mau vang
            {
                color2 = Brushes.Yellow;
            }

            if (FillBlue.IsChecked == true) //mau xanh
            {
                color2 = Brushes.Blue;
            }
            if (FillGreen.IsChecked == true) //mau xanh lá
            {
                color2 = Brushes.Green;
            }
            if (FillOrange.IsChecked == true) //mau cam
            {
                color2 = Brushes.Orange;
            }
            if (FillPurple.IsChecked == true) //mau tím
            {
                color2 = Brushes.Purple;
            }
            if (FillPink.IsChecked == true) //mau hồng
            {
                color2 = Brushes.Pink;
            }
        }

        //Tạo textbox
         private void Addtext_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = new TextBox();
            tb.TextWrapping = TextWrapping.Wrap;
            tb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tb.AcceptsReturn = true;
            tb.Width = 300;
            tb.Height = 100;
            tb.Margin = new Thickness(5, 10, 0, 0);
            tb.BorderBrush = Brushes.White;
            tb.FontSize = fontsize;//size font
            if (font == 1)
            {
                tb.FontFamily = new FontFamily("Arial");
            }
            if (font == 2)
            {
                tb.FontFamily = new FontFamily("Arial Black");
            }
            if (font == 3)
            {
                tb.FontFamily = new FontFamily("Times New Roman");
            }
            if (font == 4)
            {
                tb.FontFamily = new FontFamily("VnTime");
            }
            paintCanvas.Children.Add(tb);
            Canvas.SetLeft(tb, startPoint.X);
            Canvas.SetTop(tb, startPoint.Y);
            return;
           
        }

        // Vẽ tự do
        private void freeDrawing(MouseEventArgs e)
        {
            Line line = new Line();

            var newBrush = myBrush.Clone();
            line.Stroke = newBrush;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = e.GetPosition(paintCanvas).X;
            line.Y2 = e.GetPosition(paintCanvas).Y;
            line.StrokeThickness = size;
            paintCanvas.Children.Add(line);
            currentPoint = e.GetPosition(paintCanvas);
        }



        private void paintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            MauTo();
            currentPoint = e.GetPosition(paintCanvas);
            if (slElement != null)
            {
                if (MouseInside)
                {
                    startPoint = e.GetPosition(paintCanvas);
                    _originalLeft = Canvas.GetLeft(slElement);
                    _originalTop = Canvas.GetTop(slElement);
                    return;
                }
            }
            if (selected)
            {
                selected = false;
                if (slElement != null)
                {
                    aLayer.Remove(aLayer.GetAdorners(slElement)[0]);
                    slElement = null;
                }
                _isDown = false;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                startPaint = true;

               if(tag == 1)
               {
                   lastLine = new Line();
                   lastLine.Stroke = myBrush.Clone();
                   lastLine.Fill = color2;
                   lastLine.StrokeThickness = size;
                   lastLine.StrokeDashArray = lineType;
                    lastLine.X1 = currentPoint.X;
                    lastLine.Y1 = currentPoint.Y;

                    paintCanvas.Children.Add(lastLine);

                }


                if (tag==2|| tag == 3)
                {
                    lastRectangle = new Rectangle();
                    lastRectangle.Stroke = myBrush.Clone();
                    lastRectangle.Fill = color2;
                    lastRectangle.StrokeThickness = size;
                    lastRectangle.StrokeDashArray = lineType;

                    Canvas.SetLeft(lastRectangle, currentPoint.X);
                    Canvas.SetTop(lastRectangle, currentPoint.Y);
                    paintCanvas.Children.Add(lastRectangle);

                }



                if (tag == 4 || tag == 5)
                {
                    lastEllipse = new Ellipse();
                    lastEllipse.Stroke = myBrush.Clone();
                    lastEllipse.Fill = color2;
                    lastEllipse.StrokeThickness = size;
                    lastEllipse.StrokeDashArray = lineType;

                    Canvas.SetLeft(lastEllipse, currentPoint.X);
                    Canvas.SetTop(lastEllipse, currentPoint.Y);

                    paintCanvas.Children.Add(lastEllipse);

                }


            }

            if (tag == 6)
            {
                lastArrow = new Polygon();
                lastArrow.Stroke = myBrush.Clone();
                lastArrow.Fill = color2;
                lastArrow.StrokeThickness = size;
                lastArrow.StrokeDashArray = lineType;

                Canvas.SetLeft(lastArrow, currentPoint.X);
                Canvas.SetTop(lastArrow, currentPoint.Y);
                paintCanvas.Children.Add(lastArrow);

            }
            if (tag == 7)
            {
                lastStar = new Polygon();
                lastStar.Stroke = myBrush.Clone();
                lastStar.Fill = color2;
                lastStar.StrokeThickness = size;
                lastStar.StrokeDashArray = lineType;

                Canvas.SetLeft(lastStar, currentPoint.X);
                Canvas.SetTop(lastStar, currentPoint.Y);
                paintCanvas.Children.Add(lastStar);

            }

            if (tag==8)
            {
                lastHeart = new System.Windows.Shapes.Path();
                lastHeart.Stroke = myBrush.Clone();
                lastHeart.Fill = color2;
                lastHeart.StrokeThickness = size;
                lastHeart.StrokeDashArray = lineType;

                Canvas.SetLeft(lastHeart, currentPoint.X);
                Canvas.SetTop(lastHeart, currentPoint.Y);
                paintCanvas.Children.Add(lastHeart);

            }
            e.Handled = true;
           
        }

        private void paintCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var endPoint = e.GetPosition(paintCanvas);//điểm khi thả chuột

            

            startPaint = false;

            if (lastRectangle != null || lastEllipse != null || lastLine != null || lastArrow != null || lastStar != null || lastHeart != null)
            {
                dragMode = tag;
                _isDown = true;
                startPoint = e.GetPosition(paintCanvas);

                if (lastRectangle != null)
                    slElement = lastRectangle;
                else if (lastEllipse != null)
                    slElement = lastEllipse;
                else if (lastArrow != null)
                    slElement = lastArrow;
                else if (lastStar != null)
                    slElement = lastStar;
                else if (lastHeart != null)
                    slElement = lastHeart;
                else
                    slElement = lastLine;

                if (lastLine == null)
                {
                    _originalLeft = Canvas.GetLeft(slElement);
                    _originalTop = Canvas.GetTop(slElement);
                    aLayer = AdornerLayer.GetAdornerLayer(slElement);
                    aLayer.Add(new ResizeAdorner(slElement));
                    //Layer.Add(new RotatingAdorner(slElement, paintCanvas));
                }
                else
                {
                    _originalLeft = lastLine.X1;
                    _originalTop = lastLine.Y1;
                    aLayer = AdornerLayer.GetAdornerLayer(slElement);
                    var a = new LineAdorner(slElement);
                    a.ClipToBounds = true;
                    aLayer.Add(a);
                }
                selected = true;

            }

            lastRectangle = null;
            lastEllipse = null;
            lastLine = null;
            lastArrow = null;
            lastStar = null;
            lastHeart = null;
            
        }

        private void paintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;


            if (slElement != null)
            {

                var endPoint = e.GetPosition(paintCanvas);

                switch (dragMode)
                {
                    case 1:
                        if (slElement.IsMouseOver)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;
                        }
                        break;
                    case 2:
                        Rectangle slRect = (Rectangle)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slRect)
                            && endPoint.X <= Canvas.GetLeft(slRect) + slRect.Width
                            && endPoint.Y >= Canvas.GetTop(slRect)
                            && endPoint.Y <= Canvas.GetTop(slRect) + slRect.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;

                        }
                        break;
                    case 3:
                        slRect = (Rectangle)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slRect)
                            && endPoint.X <= Canvas.GetLeft(slRect) + slRect.Width
                            && endPoint.Y >= Canvas.GetTop(slRect)
                            && endPoint.Y <= Canvas.GetTop(slRect) + slRect.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;

                        }
                        break;

                    case 4:
                        Ellipse slElip = (Ellipse)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slElip)
                            && endPoint.X <= Canvas.GetLeft(slElip) + slElip.Width
                            && endPoint.Y >= Canvas.GetTop(slElip)
                            && endPoint.Y <= Canvas.GetTop(slElip) + slElip.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;
                        }
                        break;
                    case 5:
                        slElip = (Ellipse)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slElip)
                            && endPoint.X <= Canvas.GetLeft(slElip) + slElip.Width
                            && endPoint.Y >= Canvas.GetTop(slElip)
                            && endPoint.Y <= Canvas.GetTop(slElip) + slElip.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;
                        }
                        break;
                    case 6:
                        Polygon slArrow = (Polygon)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slArrow)
                            && endPoint.X <= Canvas.GetLeft(slArrow) + slArrow.Width
                            && endPoint.Y >= Canvas.GetTop(slArrow)
                            && endPoint.Y <= Canvas.GetTop(slArrow) + slArrow.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;

                        }
                        break;

                    case 7:
                        Polygon slStar = (Polygon)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slStar)
                            && endPoint.X <= Canvas.GetLeft(slStar) + slStar.Width
                            && endPoint.Y >= Canvas.GetTop(slStar)
                            && endPoint.Y <= Canvas.GetTop(slStar) + slStar.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;

                        }
                        break;
                    case 8:
                        System.Windows.Shapes.Path slHeart = (System.Windows.Shapes.Path)slElement;
                        if (endPoint.X >= Canvas.GetLeft(slHeart)
                            && endPoint.X <= Canvas.GetLeft(slHeart) + slHeart.Width
                            && endPoint.Y >= Canvas.GetTop(slHeart)
                            && endPoint.Y <= Canvas.GetTop(slHeart) + slHeart.Height)
                        {
                            MouseInside = true;
                            this.Cursor = Cursors.SizeAll;
                        }
                        else
                        {
                            MouseInside = false;
                            this.Cursor = Cursors.Cross;

                        }
                        break;
                }
            }
                if (e.LeftButton != MouseButtonState.Pressed)
                    return;
                if (_isDown)
                {
                    var endPoint = e.GetPosition(paintCanvas);
                    if ((_isDragging == false) &&
                        ((Math.Abs(endPoint.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                        (Math.Abs(endPoint.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                        _isDragging = true;

                    if (_isDragging)
                    {
                        

                        endPoint = e.GetPosition(paintCanvas);
                        if (dragMode != 1)
                        {
                            if (slElement != null)
                            {
                                Canvas.SetTop(slElement, endPoint.Y - (startPoint.Y - _originalTop));
                                Canvas.SetLeft(slElement, endPoint.X - (startPoint.X - _originalLeft));
                            }

                        }
                        else
                        {

                            Line tempLine = slElement as Line;
                            var deltaX = Math.Abs(endPoint.X - startPoint.X);
                            var deltaY = Math.Abs(endPoint.Y - startPoint.Y);
                            if (Math.Max(endPoint.X, startPoint.X) == startPoint.X)
                            {
                                tempLine.X1 -= deltaX;
                                tempLine.X2 -= deltaX;
                            }
                            else
                            {
                                tempLine.X1 += deltaX;
                                tempLine.X2 += deltaX;
                            }
                            if (Math.Max(endPoint.Y, startPoint.Y) == startPoint.Y)
                            {
                                tempLine.Y1 -= deltaX;
                                tempLine.Y2 -= deltaX;
                            }
                            else
                            {
                                tempLine.Y1 += deltaX;
                                tempLine.Y2 += deltaX;
                            }
                            startPoint = endPoint;
                        }

                    }
                    return;
                }
                if (startPaint == true)
                {

                    var endPoint = e.GetPosition(paintCanvas);

                    if (tag == 1)
                    {
                        if (lastLine != null)
                        {
                            lastLine.X2 = endPoint.X;
                            lastLine.Y2 = endPoint.Y;
                        }

                    }


                    if (tag == 2)
                    {
                        if (lastRectangle == null)
                            return;



                        Canvas.SetLeft(lastRectangle, Math.Min(currentPoint.X, endPoint.X));
                        Canvas.SetTop(lastRectangle, Math.Min(currentPoint.Y, endPoint.Y));

                        lastRectangle.Height = Math.Abs(endPoint.Y - currentPoint.Y);
                        lastRectangle.Width = Math.Abs(endPoint.X - currentPoint.X);

                    }

                    if (tag == 3)
                    {
                        if (lastRectangle == null)
                            return;

                        var h = Math.Abs(endPoint.Y - currentPoint.Y);
                        var w = Math.Abs(endPoint.X - currentPoint.X);
                        var min = w < h ? w : h;

                        lastRectangle.Height = min;
                        lastRectangle.Width = min;

                        if (min == w)
                        {
                            Canvas.SetLeft(lastRectangle, Math.Min(currentPoint.X, endPoint.X));

                            if (currentPoint.Y > endPoint.Y)
                            {
                                Canvas.SetTop(lastRectangle, currentPoint.Y - min);
                            }
                            else
                            {
                                Canvas.SetTop(lastRectangle, currentPoint.Y);
                            }

                        }
                        if (min == h)
                        {
                            Canvas.SetTop(lastRectangle, Math.Min(currentPoint.Y, endPoint.Y));

                            if (currentPoint.X > endPoint.X)
                            {
                                Canvas.SetLeft(lastRectangle, currentPoint.X - min);
                            }
                            else
                            {
                                Canvas.SetLeft(lastRectangle, currentPoint.X);
                            }


                        }


                    }
                    if (tag == 4)
                    {
                        if (lastEllipse == null)
                            return;


                        Canvas.SetLeft(lastEllipse, Math.Min(currentPoint.X, endPoint.X));
                        Canvas.SetTop(lastEllipse, Math.Min(currentPoint.Y, endPoint.Y));

                        lastEllipse.Height = Math.Abs(endPoint.Y - currentPoint.Y);
                        lastEllipse.Width = Math.Abs(endPoint.X - currentPoint.X);


                    }
                    if (tag == 5)
                    {
                        if (lastEllipse == null)
                            return;



                        var h = Math.Abs(endPoint.Y - currentPoint.Y);
                        var w = Math.Abs(endPoint.X - currentPoint.X);
                        var min = w < h ? w : h;

                        lastEllipse.Height = min;
                        lastEllipse.Width = min;

                        if (min == w)
                        {
                            Canvas.SetLeft(lastEllipse, Math.Min(currentPoint.X, endPoint.X));

                            if (currentPoint.Y > endPoint.Y)
                            {
                                Canvas.SetTop(lastEllipse, currentPoint.Y - min);
                            }
                            else
                            {
                                Canvas.SetTop(lastEllipse, currentPoint.Y);
                            }

                        }
                        if (min == h)
                        {
                            Canvas.SetTop(lastEllipse, Math.Min(currentPoint.Y, endPoint.Y));

                            if (currentPoint.X > endPoint.X)
                            {
                                Canvas.SetLeft(lastEllipse, currentPoint.X - min);
                            }
                            else
                            {
                                Canvas.SetLeft(lastEllipse, currentPoint.X);
                            }
                        }

                    }


                    if (tag == 6)
                    {
                        if (lastArrow == null)
                            return;



                        Canvas.SetLeft(lastArrow, Math.Min(currentPoint.X, endPoint.X));
                        Canvas.SetTop(lastArrow, Math.Min(currentPoint.Y, endPoint.Y));

                        lastArrow.Height = Math.Abs(endPoint.Y - currentPoint.Y);
                        lastArrow.Width = Math.Abs(endPoint.X - currentPoint.X);
                        lastArrow.Stretch = Stretch.Fill;
                        PointCollection pc = new PointCollection();
                        pc.Add(new Point(1, 2)); pc.Add(new Point(5, 2)); pc.Add(new Point(5, 1)); pc.Add(new Point(9, 3)); pc.Add(new Point(5, 5)); pc.Add(new Point(5, 4)); pc.Add(new Point(1, 4));
                        lastArrow.Points = pc;


                    }
                    if (tag == 7)
                    {
                        if (lastStar == null)
                            return;
                        Canvas.SetLeft(lastStar, Math.Min(currentPoint.X, endPoint.X));
                        Canvas.SetTop(lastStar, Math.Min(currentPoint.Y, endPoint.Y));

                        lastStar.Height = Math.Abs(endPoint.Y - currentPoint.Y);
                        lastStar.Width = Math.Abs(endPoint.X - currentPoint.X);
                        lastStar.Stretch = Stretch.Fill;
                        PointCollection pc = new PointCollection();
                        pc.Add(new Point(0, 0)); pc.Add(new Point(-0.11226, 0.34549)); pc.Add(new Point(-0.47552, 0.34549)); pc.Add(new Point(-0.18163, 0.55901));
                        pc.Add(new Point(-0.29389, 0.90451)); pc.Add(new Point(0, 0.69097)); pc.Add(new Point(0.29389, 0.90451)); pc.Add(new Point(0.18163, 0.55901));
                        pc.Add(new Point(0.47552, 0.34549)); pc.Add(new Point(0.11226, 0.34549));
                        lastStar.Points = pc;

                    }

                    if (tag == 8)
                    {
                        if (lastHeart == null)
                            return;

                        Canvas.SetLeft(lastHeart, Math.Min(currentPoint.X, endPoint.X));
                        Canvas.SetTop(lastHeart, Math.Min(currentPoint.Y, endPoint.Y));

                        lastHeart.Height = Math.Abs(endPoint.Y - currentPoint.Y);
                        lastHeart.Width = Math.Abs(endPoint.X - currentPoint.X);

                        lastHeart.Data = Geometry.Parse(@"M 241,200 
                            A 20,20 0 0 0 200,240
                            C 210,250 240,270 240,270
                            C 240,270 260,260 280,240
                            A 20,20 0 0 0 239,200");
                        lastHeart.Stretch = Stretch.Fill;


                    }

                }
            }
            

        private void Choose_Shape(object sender, RoutedEventArgs e)
        {  
            RadioButton bt = (RadioButton)sender;
          
              tag = int.Parse(bt.Tag.ToString());
           
            
        }
 
        private void Save_Click(object sender, RoutedEventArgs e)
            {
                SaveFileDialog Filedlg = new SaveFileDialog();
                Filedlg.Filter = "PNG Images|*.png| JPEG Images |*.jpg";

                if ((bool)Filedlg.ShowDialog(this))
                {
                    try
                    {
                        using (FileStream savefile = new FileStream(Filedlg.FileName, FileMode.Create, FileAccess.Write))
                        {
                            Rect bounds = VisualTreeHelper.GetDescendantBounds(paintCanvas);
                            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, System.Windows.Media.PixelFormats.Default);
                            DrawingVisual dv = new DrawingVisual();
                            using (DrawingContext dc = dv.RenderOpen())
                            {
                                VisualBrush vb = new VisualBrush(paintCanvas);
                                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                            }
                            rtb.Render(dv);
                            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
                            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));
                            enc.Save(savefile);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, Title);
                    }
                }
            }

        private void Open_Click(object sender, RoutedEventArgs e)
            {
                Microsoft.Win32.OpenFileDialog dl1 = new Microsoft.Win32.OpenFileDialog();
                dl1.FileName = "";
                dl1.Filter = "All Image File|*.*";
                if ((bool)dl1.ShowDialog(this))
                {
                    var bitmap = new BitmapImage(new Uri(dl1.FileName));
                    var image = new Image { Source = bitmap };
                    this.paintCanvas.Children.Clear();
                    Canvas.SetLeft(image, 0);
                    Canvas.SetTop(image, 0);
                    paintCanvas.Children.Add(image);
                }
            }

        private void cbLineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                ComboBox myCb = sender as ComboBox;
                if (myCb == null)
                    return;
                int index = myCb.SelectedIndex;
                if (index == 0)
                    lineType = null;
                else if (index == 1)
                    lineType = new DoubleCollection() { 1, 1 };
                else if (index == 2)
                    lineType = new DoubleCollection() { 4, 4 };
            }

        private void Size_Checked(object sender, RoutedEventArgs e)
            {
                var radioButton = sender as RadioButton;
                if (radioButton == null)
                    return;
                size = int.Parse(radioButton.Tag.ToString());
            }

        private void Color_Checked(object sender, RoutedEventArgs e)
            {
                var radioButton = sender as RadioButton;
                if (radioButton == null)
                    return;
                var hexColor = radioButton.Background.ToString();

                color = (Color)ColorConverter.ConvertFromString(hexColor);
                myBrush.Color = color;
            }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            int n = paintCanvas.Children.Count;

            switch (Last_Shapes())
            {
                case 1:
                    Line line = paintCanvas.Children[n - 1] as Line;
                    Undo_Redo.Shapes.Add(line);
                    Undo_Redo.Type_Shapes.Add(1);
                    break;
                case 2:
                    Rectangle Rec = paintCanvas.Children[n - 1] as Rectangle;
                    Undo_Redo.Shapes.Add(Rec);
                    Undo_Redo.Type_Shapes.Add(2);
                    break;
                case 3:
                    Ellipse elip = paintCanvas.Children[n - 1] as Ellipse;
                    Undo_Redo.Shapes.Add(elip);
                    Undo_Redo.Type_Shapes.Add(3);
                    break;   
              
            }


            Undo_Redo.n++;
            paintCanvas.Children.RemoveAt(n - 1);
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            int n = Undo_Redo.n - 1;
            if (n < 0)
                return;
            switch (Undo_Redo.Type_Shapes[n])
            {
                case 1:
                    paintCanvas.Children.Add(Undo_Redo.Shapes[n] as Rectangle);
                    break;
                case 2:
                    paintCanvas.Children.Add(Undo_Redo.Shapes[n] as Ellipse);
                    break;
                case 3:
                    paintCanvas.Children.Add(Undo_Redo.Shapes[n] as Line);
                    break;
            }

            Undo_Redo.Shapes.RemoveAt(Undo_Redo.n - 1);
            Undo_Redo.Type_Shapes.RemoveAt(Undo_Redo.n - 1);

            Undo_Redo.n--;
        }

        private int Last_Shapes()
        {
            int n = paintCanvas.Children.Count;
            try
            {
                Line lp = paintCanvas.Children[n - 1] as Line;
                if (lp != null)
                    return 3;
            }
            catch { }
            try
            {
                Ellipse lp = paintCanvas.Children[n - 1] as Ellipse;
                if (lp != null) return 2;
            }
            catch { }

            try
            {
                Rectangle lp = paintCanvas.Children[n - 1] as Rectangle;
                if (lp != null) return 1;
            }
            catch { }



            return 0;
        }

       

        private void SelectFontSize(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cb = (ComboBoxItem)_fontsize.SelectedItem;
            string temp = cb.Content.ToString();
            fontsize = Convert.ToInt32(temp);
        }

        private void SelectFont(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cb = (ComboBoxItem)_Font.SelectedItem;
            string temp = cb.Content.ToString();
            switch (temp)
            {

                case "Arial":
                    {
                        font = 1;
                        break;
                    }
                case "Arial Black":
                    {
                        font = 2;
                        break;
                    }
                case "Times New Roman":
                    {
                        font = 3;
                        break;
                    }
                case "VnTime":
                    {
                        font = 4;
                        break;
                    }
            }
        }
    }
}
