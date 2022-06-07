using System;
using Core;
using NUnit.Framework;

namespace TestCore
{
	/// <summary>
	/// Класс для тестирования <see cref="Parameter"/>
	/// </summary>
	[TestFixture]
	public class ParameterTest
	{
		[TestCase(TestName = "Проверка корректного вызова конструктора")]
		public void TestConstructor_CorrectValue()
		{
			Assert.DoesNotThrow(
				() => new Parameter(1, 10, 5)
			, "Вылетела ошибка при корректных значениях конструктора.");
		}

		[TestCase(TestName = "Проверка некорректного вызова конструктора." +
							 "Должно выбросить ошибку, " +
							 "если минимальное значение больше максимального.")]
		public void TestConstructor_IncorrectValue()
		{
			Assert.Throws<ArgumentException>(
				() => new Parameter(10, 1, 5)
				, "Не вылетела ошибка при некорректных значениях конструктора.");
		}

		[TestCase(TestName = "Проверка корректного получения" +
							 " значения свойства MinValue.")]
		public void TestMinValue_CorrectGetValue()
		{
			var value = 10.0;

			var expected = value;

			var parameter = new Parameter(value, 20, 15);

			var actual = parameter.MinValue;

			Assert.AreEqual(expected, actual, "Вернулось некорректное значение.");
		}

		[TestCase(TestName = "Проверка корректного получения" +
							 " значения свойства MaxValue.")]
		public void TestMaxValue_CorrectGetValue()
		{
			var value = 10.0;

			var expected = value;

			var parameter = new Parameter(1, value, 5);

			var actual = parameter.MaxValue;

			Assert.AreEqual(expected, actual, "Вернулось некорректное значение.");
		}

		[TestCase(TestName = "Проверка корректного получения" +
							 " значения свойства Value.")]
		public void TestValue_CorrectGetValue()
		{
			var value = 10.0;

			var expected = value;

			var parameter = new Parameter(1, 20, value);

			var actual = parameter.Value;

			Assert.AreEqual(expected, actual, "Вернулось некорректное значение.");
		}

		[TestCase(TestName = "Проверка корректной записи" +
							 " значения свойства Value.")]
		public void TestValue_CorrectSetValue()
		{
			var value = 10.0;

			var parameter = new Parameter(1, 20, 5);

			Assert.DoesNotThrow(() => parameter.Value = value,
				"Не удалось присвоить корректное значение.");
		}

		[TestCase(1, TestName = "Проверка записи" +
							 " значения свойства Value" +
							 " меньшему минимальному значению." +
							 "Должно выкинуть исключение.")]
		[TestCase(100, TestName = "Проверка записи" +
								" значения свойства Value" +
								" больше максимального значения." +
								"Должно выкинуть исключение.")]
		public void TestValue_IncorrectSetValue(double value)
		{
			var parameter = new Parameter(5, 9, 7);

			Assert.Throws<ArgumentException>(() => parameter.Value = value,
				"Не удалось присвоить корректное значение.");
		}
	}
}