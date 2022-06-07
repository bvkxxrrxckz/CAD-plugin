using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
	/// <summary>
	/// Класс параметра.
	/// </summary>
    public class Parameter
    {
	    /// <summary>
	    /// Значение параметра.
	    /// </summary>
	    private double _value;

	    /// <summary>
	    /// Возвращает минимальное значение.
	    /// </summary>
	    public double MinValue { get; internal set; }

	    /// <summary>
	    /// Возвращает максимальное значение.
	    /// </summary>
	    public double MaxValue { get; internal set; }

	    /// <summary>
	    /// Возвращает и устанавливает значение параметра.
	    /// </summary>
	    public double Value
	    {
		    get => _value;
		    set
		    {
			    _value = value;
			    if (!Validate())
			    {
				    throw new ArgumentException(
					    $"значение не входит диапазон {MinValue} — {MaxValue}");
			    }
		    }
	    }

	    /// <summary>
	    /// Конструктор.
	    /// </summary>
	    /// <param name="minValue">Минимальное значение.</param>
	    /// <param name="maxValue">Максимальное значение.</param>
	    /// <param name="value">Значение параметра.</param>
	    public Parameter(double minValue, double maxValue, double value)
	    {
		    if (minValue > maxValue)
		    {
			    throw new ArgumentException(
				    "Значение минимума больше значения максимума.");
		    }

		    MinValue = minValue;
		    MaxValue = maxValue;
		    Value = value;
	    }

	    /// <summary>
	    /// Проверка значения на принадлежность промежутку <see cref="MinValue"/> и <see cref="MaxValue"/>.
	    /// </summary>
	    /// <returns>True, если валидация пройдена.</returns>
	    public bool Validate()
	    {
		    return Value >= MinValue && Value <= MaxValue;
	    }
	}
}
