using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	/// <summary>
	/// Класс параметров вешалки.
	/// </summary>
	public class HangerParameters
	{
		/// <summary>
		/// Словарь параметров.
		/// </summary>
		private readonly Dictionary<ParameterType, Parameter> _parameters =
			new Dictionary<ParameterType, Parameter>
			{
				{
					ParameterType.HangerWidth, new Parameter(600.0,
						1000.0, 800.0)
				},
				{
					ParameterType.HangerHeight, new Parameter(
						800.0, 1500.0, 800.0)
				},
				{
					ParameterType.SectionWidth, new Parameter(
						200.0, 500.0, 200.0)
				},
				{
					ParameterType.WidthShelf, new Parameter(
						500.0, 600.0, 500.0)
				},
				{
					ParameterType.DepthShelf, new Parameter(
						100.0, 200.0, 100.0)
				},
			};

		/// <summary>
		/// Установить значение параметра.
		/// </summary>
		/// <param name="parameterType">Тип параметра..</param>
		/// <param name="value">Значение параметра.</param>
		public List<string> SetValue(ParameterType parameterType, double value)
		{
			var parameter = _parameters[parameterType];
			var minValue = parameter.MinValue;
			var maxValue = parameter.MaxValue;
			var errors = new List<string>();

			switch (parameterType)
			{
				case ParameterType.HangerWidth:
				{
					CheckError(ParameterType.SectionWidth, value / 3, errors);
					CheckError(ParameterType.WidthShelf, value, errors);
					break;
				}
				default:
				{
					break;
				}

			}

			parameter.MinValue = minValue;
			parameter.MaxValue = maxValue;
			try
			{
				parameter.Value = value;
			}
			catch (ArgumentException e)
			{
				errors.Add($"{parameterType}:{e.Message}");
			}

			return errors;
		}

		/// <summary>
		/// Получить значение параметра.
		/// </summary>
		/// <param name="parameterType">Тип параметра.</param>
		/// <returns>Значение параметра.</returns>
		public double GetValue(ParameterType parameterType)
		{
			return _parameters[parameterType].Value;
		}

		/// <summary>
		/// Проверить валидацию.
		/// </summary>
		/// <param name="parameterType">Тип параметра для проверки.</param>
		/// <param name="maxValue">Устанавливаемое максимальное значение.</param>
		/// <param name="errors">Коллекция ошибок.</param>
		private void CheckError(ParameterType parameterType, double maxValue, List<string> errors)
		{
			var parameter = _parameters[parameterType];
			parameter.MaxValue = maxValue;
			if (!parameter.Validate())
			{
				errors.Add($"{parameterType}:значение не входит диапазон " +
				           $"{parameter.MinValue} — {parameter.MaxValue}");
			}
		}
	}
}