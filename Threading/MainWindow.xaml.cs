using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Threading
{
    public partial class MainWindow : Window
    {
        private bool TextThreadRunning { get; set; }
        private Thread TextThread { get; set; }

        private bool Running { get; set; }
        private Thread Thread { get; set; }

        private int ShapeRotateAngle = 1;
        private int TextRotateAngle = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Start()
        {
            Running = false;
            Thread = new Thread(StartRotating);
            Thread.Start();
        }

        private void StartRotating()
        {
            while (!Running)
            {
                Dispatcher.Invoke(() =>
                {

                    rectangle.RenderTransform = new RotateTransform(ShapeRotateAngle);
                    ShapeRotateAngle = ShapeRotateAngle + 10;
                    if (ShapeRotateAngle > 360)
                    {
                        ShapeRotateAngle = 0;
                    }
                });
                Thread.Sleep(50);
            }
        }

        public void StopRotatingShape()
        {
            Running = true;
            Thread?.Join();
        }

        public void DrawShape()
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromArgb(255, 120, 30, 0)
            };
            rectangle.Fill = mySolidColorBrush;
            rectangle.StrokeThickness = 2;
            rectangle.Stroke = Brushes.Black;
        }

        private void StartRandomText(object sender, RoutedEventArgs e)
        {
            DrawTextBlock();
            StartTextThread();
            StopRandomTextBtn.IsEnabled = true;
            StartRandomTextBtn.IsEnabled = false;
        }

        private void StopRandomText(object sender, RoutedEventArgs e)
        {
            StopTextThread();
            StopRandomTextBtn.IsEnabled = false;
            StartRandomTextBtn.IsEnabled = true;
        }

        public void StopTextThread()
        {
            TextThreadRunning = true;
            TextThread?.Join();
        }

        private void StartWritingAndRotatingText()
        {
            while (!TextThreadRunning)
            {
                Dispatcher.Invoke(() =>
                {
                    GenerateRandomText();
                    TextTextBlock.RenderTransform = new RotateTransform(TextRotateAngle);
                    TextRotateAngle = TextRotateAngle + 10;
                    if (TextRotateAngle > 80 && TextRotateAngle < 200)
                    {
                        TextRotateAngle = 260;
                    }
                    if (TextRotateAngle > 360)
                    {
                        TextRotateAngle = 0;
                    }
                });
                Thread.Sleep(1000);
            }
        }

        public void DrawTextBlock()
        {
            TextTextBlock.Height = 50;
            TextTextBlock.Width = 200;
            TextTextBlock.Foreground = new SolidColorBrush(Colors.Black);
        }

        public void GenerateRandomText()
        {
            string[] Texts = new string[] { "this is a random text", "this is a new text", "here comes one more text" };
            Random rnd = new Random();
            int index = rnd.Next(Texts.Length);
            TextTextBlock.Text = $"{Texts[index]}";
        }

        public void StartTextThread()
        {
            TextThreadRunning = false;
            TextThread = new Thread(StartWritingAndRotatingText);
            TextThread.Start();
        }

        private void StartRotatingShape(object sender, RoutedEventArgs e)
        {
            DrawShape();
            Start();
            StartRotatingShapeBtn.IsEnabled = false;
            StopRotatingShapeBtn.IsEnabled = true;
        }

        private void StopRotatingShape(object sender, RoutedEventArgs e)
        {
            StopRotatingShape();
            StartRotatingShapeBtn.IsEnabled = true;
            StopRotatingShapeBtn.IsEnabled = false;
        }
    }
}
