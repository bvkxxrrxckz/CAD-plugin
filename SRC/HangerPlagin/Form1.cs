using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core;
using KompasWrapper;

namespace HangerPlagin
{
    public partial class Form1 : Form
    {
	    /// <summary>
	    /// Параметры вешалки.
	    /// </summary>
	    private readonly HangerParameters _parameters = new HangerParameters();

        /// <summary>
        /// Строитель вешалки.
        /// </summary>
        private readonly HangerBuilder _builder = new HangerBuilder();

        /// <summary>
        /// Словарь параметров. Ключ — поле для ввода значения <see cref="TextBox"/>,
        /// Значение — тип параметра.
        /// </summary>
        private readonly Dictionary<TextBox, ParameterType> _parameterTypes;

        /// <summary>
        /// Словарь <see cref="Label"/>
        /// </summary>
        private readonly Dictionary<ParameterType, Label> _labels;

        /// <summary>
        /// Словарь ошибок.
        /// </summary>
        private readonly Dictionary<ParameterType, string> _errors =
	        new Dictionary<ParameterType, string>();

        public Form1()
        {
            InitializeComponent();

            _parameterTypes = new Dictionary<TextBox, ParameterType>
            {
	            {textBox1, ParameterType.HangerWidth},
	            {textBox2, ParameterType.HangerHeight},
	            {textBox3, ParameterType.SectionWidth},
	            {textBox4, ParameterType.WidthShelf},
	            {textBox5, ParameterType.DepthShelf},
            };

            _labels = new Dictionary<ParameterType, Label>
            {
	            {ParameterType.HangerWidth, label1},
	            {ParameterType.HangerHeight, label2},
	            {ParameterType.SectionWidth, label3},
	            {ParameterType.WidthShelf, label4},
	            {ParameterType.DepthShelf, label5},
            };

            SetDefaultParameter();
        }

        /// <summary>
        /// Установление параметров в стандартное значение.
        /// </summary>
        private void SetDefaultParameter()
        {

	        textBox1.Text = _parameters.GetValue(
		        ParameterType.HangerWidth).ToString();
	        textBox2.Text = _parameters.GetValue(
		        ParameterType.HangerHeight).ToString();
	        textBox3.Text = _parameters.GetValue(
		        ParameterType.SectionWidth).ToString();
	        textBox4.Text = _parameters.GetValue(
		        ParameterType.WidthShelf).ToString();
	        textBox5.Text = _parameters.GetValue(
		        ParameterType.DepthShelf).ToString();
        }

        /// <summary>
        /// Получить название неверного поля
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        private string GetNameLabel(ParameterType parameterType)
        {
	        return _labels[parameterType].Text;
        }

        /// <summary>
        /// Установить значение параметру детали
        /// </summary>
        /// <param name="textBox">
        /// <see cref="TextBox"/> из которого будет браться значение</param>
        /// <param name="toolTip">
        /// <see cref="System.Windows.Forms.ToolTip"/> для показа ошибки</param>
        /// <param name="parameterType">Параметр для записи</param>
		private void SetValueParameter(TextBox textBox, ToolTip toolTip,
	        ParameterType parameterType)
        {
	        double value;
	        try
	        {
		        value = GetValueFromString(textBox.Text);
	        }
	        catch (ArgumentException exception)
	        {
		        SetErrors(textBox, toolTip, 
			        parameterType, exception.Message);
				return;
	        }


	        var errors = _parameters.SetValue(parameterType, value);
	        if (errors.Any())
	        {
		        foreach (var error in errors)
		        {
			        var stringParameterType = error.Split(':')[0];
					var message = error.Split(':')[1];
					var parameter = (ParameterType)Enum.Parse(typeof(ParameterType), stringParameterType);
					SetErrors(_parameterTypes
						.First(p => p.Value == parameter).Key, 
						toolTip1, parameter, message);
		        }

				return;
	        }

	        if (_errors.ContainsKey(parameterType))
	        {
		        _errors.Remove(parameterType);
		        if (parameterType == ParameterType.HangerWidth)
		        {
			        _errors.Remove(ParameterType.SectionWidth);
			        _errors.Remove(ParameterType.WidthShelf);
					textBox3.BackColor = Color.White;
					textBox4.BackColor = Color.White;
				}
	        }

			textBox.BackColor = Color.White;
	        toolTip.Hide(textBox);
        }

		/// <summary>
		/// Устанавливает ошибку.
		/// </summary>
		/// <param name="textBox">
		/// <see cref="TextBox"/> из которого будет браться значение</param>
		/// <param name="toolTip">
		/// <see cref="System.Windows.Forms.ToolTip"/> для показа ошибки</param>
		/// <param name="parameterType">Параметр для записи</param>
		private void SetErrors(TextBox textBox, ToolTip toolTip,
	        ParameterType parameterType, string message)
        {
	        textBox.BackColor = Color.MistyRose;
	        toolTip.Show(message, textBox);
	        if (!_errors.ContainsKey(parameterType))
	        {
		        _errors.Add(parameterType, message);
	        }
	        else
	        {
		        _errors[parameterType] = message;
	        }
		}

		private void button1_Click(object sender, EventArgs e)
        {
	        if (_errors.Any())
	        {
		        var message = string.Empty;
		        foreach (var error in _errors)
		        {
			        message += GetNameLabel(error.Key) + error.Value + '\n';
		        }

		        MessageBox.Show(message, "Ошибка",
			        MessageBoxButtons.OK, MessageBoxIcon.Error);
		        return;
	        }

	        _builder.Build(_parameters);
        }

        private void TextChanged(object sender, EventArgs e)
        {
	        if (!(sender is TextBox textBox))
	        {
                return;
	        }

	        SetValueParameter(textBox, toolTip1,
		        _parameterTypes[textBox]);
        }

        /// <summary>
        /// Обработчик события для <see cref="TextBox"/> при нажатии на <see cref="TextBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnyTextBox_Enter(object sender, EventArgs e)
        {
	        if (!(sender is TextBox textBox))
	        {
		        return;
	        }

	        toolTip1.Hide(this);
	        var parameter = _parameterTypes[textBox];
	        if (_errors.ContainsKey(parameter))
	        {
		        toolTip1.Show(_errors[parameter], textBox);
	        }
        }

        /// <summary>
        /// Получить из строки число типа <see cref="double"/>
        /// </summary>
        /// <param name="valueString">Строка для парса</param>
        /// <returns>Число типа <see cref="double"/></returns>
        public static double GetValueFromString(string valueString)
        {
	        if (string.IsNullOrEmpty(valueString))
	        {
		        throw new ArgumentException("Строка не должна быть пуста.");
	        }

	        if (!double.TryParse(valueString, out var value))
	        {
		        throw new ArgumentException(
			        "Введено некорректное значение." +
			        " Нужно ввести либо целое число, либо число с плавающей точкой");
	        }

	        return value;
        }
	}
}
