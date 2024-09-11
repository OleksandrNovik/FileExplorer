using CommunityToolkit.WinUI.UI.Controls;
using Helpers.General;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Models.Storage.Additional.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FileExplorer.UI.UserControls.Charts
{
    public sealed partial class PieChart : UserControl
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register(nameof(Segments), typeof(ICollection<PieChartSegment>),
                typeof(PieChart), new PropertyMetadata(null, OnSegmentsChanged));

        private static void OnSegmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PieChart chart && e.NewValue is ICollection<PieChartSegment> { Count: > 0 } segments)
            {
                chart.Draw(segments);
            }
        }

        public ICollection<PieChartSegment> Segments
        {
            get => (ICollection<PieChartSegment>)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public int Size { get; set; }
        public PieChart()
        {
            this.InitializeComponent();
            Loading += OnLoading;
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            Draw(Segments);
        }

        private void Draw(ICollection<PieChartSegment> segments)
        {
            if (Chart.Width < 0 || Chart.Height < 0) return;

            Chart.Children.Clear();

            var text = new List<DropShadowPanel>();

            double total = segments.Sum(s => s.Value);
            double angleOffset = 0;
            double centerX = Chart.Width / 2;
            double centerY = Chart.Height / 2;
            double radius = Math.Min(centerX, centerY);

            foreach (var segment in segments)
            {
                double sweepAngle = segment.Value / total * 360;

                var pathFigure = new PathFigure
                {
                    StartPoint = new Point(centerX, centerY)
                };

                double arcX = centerX + radius * Math.Cos((angleOffset + sweepAngle) * Math.PI / 180);
                double arcY = centerY + radius * Math.Sin((angleOffset + sweepAngle) * Math.PI / 180);

                bool isLargeArc = sweepAngle > 180;

                pathFigure.Segments.Add(new LineSegment
                {
                    Point = new Point(centerX + radius * Math.Cos(angleOffset * Math.PI / 180),
                        centerY + radius * Math.Sin(angleOffset * Math.PI / 180))
                });

                pathFigure.Segments.Add(new ArcSegment
                {
                    Point = new Point(arcX, arcY),
                    Size = new Size(radius, radius),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc
                });
                pathFigure.Segments.Add(new LineSegment { Point = new Point(centerX, centerY) });

                var pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                var path = new Microsoft.UI.Xaml.Shapes.Path
                {
                    Fill = segment.Color,
                    Data = pathGeometry
                };

                Chart.Children.Add(path);

                angleOffset += sweepAngle;

                // Обчислюємо середній кут сегмента
                double middleAngle = angleOffset - sweepAngle / 2;

                // Обчислюємо позицію для відображення відсотків (ближче до центру)
                double textX = centerX + (radius * 0.6) * Math.Cos(middleAngle * Math.PI / 180);
                double textY = centerY + (radius * 0.6) * Math.Sin(middleAngle * Math.PI / 180);

                // Обчислюємо відсоток
                double percents = segment.Value * 100 / total;

                // Створюємо TextBlock для відображення відсотка
                var percentsText = new TextBlock
                {
                    Text = $"{percents:0.0}%",
                    Foreground = new SolidColorBrush(Colors.White), // Колір тексту (можна змінити)
                    FontSize = 16, // Розмір тексту
                };

                // Додаємо ефект тіні для тексту
                var textShadow = new DropShadowPanel
                {
                    ShadowOpacity = 0.8,
                    BlurRadius = 3,
                    OffsetX = 1,
                    OffsetY = 1,
                    Content = percentsText
                };

                // Додаємо текст до Canvas на відповідну позицію
                Canvas.SetLeft(textShadow, textX - percentsText.ActualWidth / 2); // Центруємо текст
                Canvas.SetTop(textShadow, textY - percentsText.ActualHeight / 2);

                text.Add(textShadow);
            }

            Chart.Children.AddRange(text);
        }
    }
}
