using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using TopCalculate;

namespace TopCalculate
{
    public partial class MainWindow : Window
    {
        private string selectedRealOperation = ""; // Операция для вещественных чисел
        private string selectedComplexOperation = ""; // Операция для комплексных чисел

        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик кнопок операций для вещественных чисел
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Убираем выделение со всех кнопок
                foreach (var child in (button.Parent as WrapPanel).Children)
                {
                    if (child is Button btn)
                    {
                        btn.Style = null;  // Сброс стиля
                    }
                }

                // Применяем стиль к выбранной кнопке
                button.Style = (Style)FindResource("SelectedButtonStyle");

                selectedRealOperation = button.Content.ToString();

                // Показываем/скрываем второе поле в зависимости от операции
                bool requiresSecondInput = selectedRealOperation is "+" or "-" or "*" or "/" or "^";
                lblInput2.Visibility = requiresSecondInput ? Visibility.Visible : Visibility.Collapsed;
                txtInput2.Visibility = requiresSecondInput ? Visibility.Visible : Visibility.Collapsed;
            }
        }




        // Кнопка "Рассчитать" для вещественных чисел
        // Кнопка "Рассчитать" для вещественных чисел
        private void BtnCalculateReal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Парсинг чисел из текстовых полей
                double input1 = double.Parse(txtInput1.Text);
                double input2 = string.IsNullOrWhiteSpace(txtInput2.Text) ? 0 : double.Parse(txtInput2.Text);

                // Выполнение операции в зависимости от выбранного действия
                double result = selectedRealOperation switch
                {
                    "+" => input1 + input2,
                    "-" => input1 - input2,
                    "*" => input1 * input2,
                    "/" => input2 != 0 ? input1 / input2 : throw new DivideByZeroException(),
                    "^" => Math.Pow(input1, input2),
                    "√" => input1 >= 0 ? Math.Sqrt(input1) : throw new ArgumentException("Корень из отрицательного числа"),
                    "sin" => Math.Sin(DegreesToRadians(input1)),
                    "cos" => Math.Cos(DegreesToRadians(input1)),
                    "tan" => Math.Tan(DegreesToRadians(input1)),
                    "arcsin" => Math.Asin(input1),
                    "arccos" => Math.Acos(input1),
                    "arctan" => Math.Atan(input1),
                    "cot" => 1 / Math.Tan(DegreesToRadians(input1)),
                    "arccot" => Math.PI / 2 - Math.Atan(input1),
                    _ => throw new InvalidOperationException("Неизвестная операция")
                };

                // Формирование строки результата для истории
                string historyEntry = selectedRealOperation switch
                {
                    "+" or "-" or "*" or "/" or "^" => $"{input1} {selectedRealOperation} {input2} = {result}",
                    "√" => $"{selectedRealOperation}{input1} = {result}",
                    _ => $"{selectedRealOperation}({input1}) = {result}" // Для функций
                };

                // Обновление интерфейса
                lblResultReal.Text = $"Результат: {result}";
                lstHistoryReal.Items.Add(historyEntry);

                // Автоматическое обновление поля первого числа
                txtInput1.Text = result.ToString();
                txtInput2.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }



        // Кнопка "Очистить" для вещественных чисел
        private void BtnClearReal_Click(object sender, RoutedEventArgs e)
        {
            txtInput1.Clear();
            txtInput2.Clear();
            lblResultReal.Text = "Результат:";
            lstHistoryReal.Items.Clear();
        }

        // Обработчик кнопок операций для комплексных чисел
        private void ComplexOperation_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                selectedComplexOperation = button.Content.ToString();
            }
        }

        // Кнопка "Рассчитать" для комплексных чисел
        private void BtnCalculateComplex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double real1 = double.Parse(txtReal1.Text);
                double imaginary1 = double.Parse(txtImaginary1.Text);
                double real2 = double.Parse(txtReal2.Text);
                double imaginary2 = double.Parse(txtImaginary2.Text);

                Complex number1 = new(real1, imaginary1);
                Complex number2 = new(real2, imaginary2);

                Complex result = selectedComplexOperation switch
                {
                    "+" => ComplexNumberOperations.Add(number1, number2),
                    "-" => ComplexNumberOperations.Subtract(number1, number2),
                    "*" => ComplexNumberOperations.Multiply(number1, number2),
                    "/" => ComplexNumberOperations.Divide(number1, number2),
                    _ => throw new InvalidOperationException("Неизвестная операция")
                };

                lblResultComplex.Text = $"Результат: {result.Real} + {result.Imaginary}i";
                lstHistoryComplex.Items.Add($"{number1} {selectedComplexOperation} {number2} = {result.Real} + {result.Imaginary}i");
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка "Очистить" для комплексных чисел
        private void BtnClearComplex_Click(object sender, RoutedEventArgs e)
        {
            txtReal1.Clear();
            txtImaginary1.Clear();
            txtReal2.Clear();
            txtImaginary2.Clear();
            lblResultComplex.Text = "Результат:";
            lstHistoryComplex.Items.Clear();
        }
    }
}