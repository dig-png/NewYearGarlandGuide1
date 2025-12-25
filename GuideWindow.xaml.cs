using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewYearGarlandGuide
{
    public partial class GuideWindow : Window
    {
        private int currentStep = 1;
        private int totalSteps = 5;
        private int garlandType; // 0 - Нить, 1 - Сетка, 2 - Свечи

        public GuideWindow(int type)
        {
            InitializeComponent();
            garlandType = type;

            // Ждем загрузки окна для правильного расчета анимации прогресс-бара
            this.Loaded += (s, e) => UpdateStep(false);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep < totalSteps) { currentStep++; UpdateStep(true); }
            else { Exit_Click(null, null); }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1) { currentStep--; UpdateStep(true); }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void UpdateStep(bool animate)
        {
            // 1. Тексты номеров
            StepNumberText.Text = $"ШАГ {currentStep} ИЗ {totalSteps}";

            // 2. Анимация прогресс-бара
            double targetWidth = this.ActualWidth * ((double)currentStep / totalSteps);
            if (animate)
            {
                DoubleAnimation anim = new DoubleAnimation(targetWidth, TimeSpan.FromSeconds(0.5));
                anim.EasingFunction = new QuarticEase { EasingMode = EasingMode.EaseOut };
                ProgressFill.BeginAnimation(Rectangle.WidthProperty, anim);
            }
            else { ProgressFill.Width = targetWidth; }

            // 3. Управление кнопками
            BackButton.Visibility = (currentStep == 1) ? Visibility.Hidden : Visibility.Visible;
            NextButton.Content = (currentStep == totalSteps) ? "ЗАВЕРШИТЬ" : "ДАЛЕЕ ➡";

            // 4. Логика текстов и выбора папки картинок
            string prefix = "";
            if (garlandType == 0) { prefix = "Thread"; SetThreadText(); }
            else if (garlandType == 1) { prefix = "Net"; SetNetText(); }
            else { prefix = "Candle"; SetCandleText(); }

            // 5. Загрузка картинки (Например: Images/Thread1.png)
            try
            {
                string path = $"pack://application:,,,/Images/{prefix}{currentStep}.png";
                StepImage.Source = new BitmapImage(new Uri(path));
            }
            catch { StepImage.Source = null; }
        }

        private void SetThreadText()
        {
            switch (currentStep)
            {
                case 1: StepTitleText.Text = "Проверка нити"; StepDescriptionText.Text = "Размотайте нить и проверьте её на наличие повреждений."; break;
                case 2: StepTitleText.Text = "Начало укладки"; StepDescriptionText.Text = "Закрепите первый светодиод у верхушки елки."; break;
                case 3: StepTitleText.Text = "Спираль"; StepDescriptionText.Text = "Двигайтесь по спирали вниз, укладывая провод глубже в хвою."; break;
                case 4: StepTitleText.Text = "Равномерность"; StepDescriptionText.Text = "Отойдите назад и проверьте, нет ли 'темных' пятен."; break;
                case 5: StepTitleText.Text = "Готово!"; StepDescriptionText.Text = "Наслаждайтесь классическим сиянием нити!"; break;
            }
        }

        private void SetNetText()
        {
            switch (currentStep)
            {
                case 1: StepTitleText.Text = "Подготовка сетки"; StepDescriptionText.Text = "Найдите верхние углы сетки, чтобы она не перекосилась."; break;
                case 2: StepTitleText.Text = "Наброс"; StepDescriptionText.Text = "Накиньте сетку на одну сторону елки или оберните вокруг."; break;
                case 3: StepTitleText.Text = "Расправление"; StepDescriptionText.Text = "Растяните ячейки так, чтобы они покрывали ветки равномерно."; break;
                case 4: StepTitleText.Text = "Фиксация"; StepDescriptionText.Text = "Закрепите края сетки маленькими веточками."; break;
                case 5: StepTitleText.Text = "Готово!"; StepDescriptionText.Text = "Сетка создает идеальный световой ковер!"; break;
            }
        }

        private void SetCandleText()
        {
            switch (currentStep)
            {
                case 1: StepTitleText.Text = "Проверка прищепок"; StepDescriptionText.Text = "Убедитесь, что все зажимы свечей работают исправно."; break;
                case 2: StepTitleText.Text = "Расстановка"; StepDescriptionText.Text = "Крепите свечи вертикально на концы самых крепких веток."; break;
                case 3: StepTitleText.Text = "Баланс"; StepDescriptionText.Text = "Старайтесь ставить свечи симметрично по всей высоте."; break;
                case 4: StepTitleText.Text = "Направление"; StepDescriptionText.Text = "Поверните лампочки-пламя строго вверх."; break;
                case 5: StepTitleText.Text = "Готово!"; StepDescriptionText.Text = "Ваша елка выглядит как в старинной сказке!"; break;
            }
        }
    }
}