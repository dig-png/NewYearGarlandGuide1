using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NewYearGarlandGuide
{
    public partial class GuideWindow : Window
    {
        private int step = 0;

        private readonly List<string> steps = new()
        {
            "Шаг 1. Проверьте гирлянду на исправность. Убедитесь, что все лампочки горят, провод не повреждён, а вилка в хорошем состоянии.",
            "Шаг 2. Распланируйте место размещения гирлянды. Измерьте длину украшаемой поверхности и выберите оптимальную траекторию.",
            "Шаг 3. Аккуратно закрепите гирлянду. Используйте специальные пластиковые клипсы, чтобы не повредить провод.",
            "Шаг 4. Убедитесь, что провод не натянут. Оставьте небольшой запас для предотвращения повреждений.",
            "Шаг 5. Подключите гирлянду к розетке через сетевой фильтр. Проверьте все режимы работы перед фиксацией."
        };

        public GuideWindow()
        {
            InitializeComponent();
            UpdateStep();
        }

        private void UpdateStep()
        {
            StepText.Text = steps[step];
            UpdateImage();
            UpdateControls();
            AnimateProgress();
        }

        private void UpdateControls()
        {
            CurrentStepNumber.Text = (step + 1).ToString();

            BackButton.IsEnabled = step > 0;
            BackButton.Opacity = step > 0 ? 1.0 : 0.5;

            if (step == steps.Count - 1)
            {
                NextButton.Content = "ЗАВЕРШИТЬ 🏠";
                NextButton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 105, 0));
                NextButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 200, 0));
            }
            else
            {
                NextButton.Content = "ДАЛЕЕ ▶";
                NextButton.Background = new SolidColorBrush(Color.FromArgb(255, 0, 102, 68));
                NextButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 136));
            }

           
        }

        private void AnimateProgress()
        {
            double targetWidth = (step + 1) * (ProgressFill.Parent as Border).ActualWidth / 5;

            var animation = new DoubleAnimation
            {
                To = targetWidth,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            ProgressFill.BeginAnimation(Border.WidthProperty, animation);
        }

        private void UpdateImage()
        {
            try
            {
                string[] imageNames = { "stepx1.png", "stepx2.png", "stepx3.png",
                                        "stepx4.png", "stepx5.png" };

                bool imageLoaded = false;

                string imagePath = Path.Combine("Images", imageNames[step]);
                if (File.Exists(imagePath))
                {
                    LoadImageFromFile(imagePath);
                    imageLoaded = true;
                }
                else
                {
                    try
                    {
                        string resourcePath = $"pack://application:,,,/Images/{imageNames[step]}";
                        StepImage.Source = new BitmapImage(new Uri(resourcePath, UriKind.Absolute));
                        imageLoaded = true;
                    }
                    catch { }
                }

                if (!imageLoaded)
                {
                    CreateGradientImage();
                }

                AnimateImage();
            }
            catch
            {
                CreateGradientImage();
            }
        }

        private void LoadImageFromFile(string path)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath(path), UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            StepImage.Source = bitmap;
        }

        private void CreateGradientImage()
        {
            var drawingVisual = new DrawingVisual();
            using (var dc = drawingVisual.RenderOpen())
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 30, 30, 60));
                dc.DrawRectangle(brush, null, new Rect(0, 0, 600, 250));

                var text = new FormattedText(
                    $"ШАГ {step + 1}",
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial Black"),
                    64,
                    Brushes.White,
                    96);

                dc.DrawText(text, new Point(200, 90));
            }

            var bmp = new RenderTargetBitmap(600, 250, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            StepImage.Source = bmp;
        }

        private void AnimateImage()
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0.3,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            StepImage.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (step < steps.Count - 1)
            {
                step++;
                UpdateStep();
            }
            else
            {
                new MainWindow().Show();
                this.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (step > 0)
            {
                step--;
                UpdateStep();
            }
        }
    }
}