using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NUnit.Framework;

namespace TestCore
{
	/// <summary>
	/// Класс для тестирования <see cref="HangerParameters"/>
	/// </summary>
	[TestFixture]
	public class HangerParametersTest
	{
		/// <summary>
		/// Возвращает новый экземпляр параметров
		/// </summary>
		private static HangerParameters Parameters => new HangerParameters();

		[TestCase(ParameterType.WidthShelf, 550.0,
			TestName = "Проверка корректного получения" +
							 " значения свойства WidthShelf.")]
		[TestCase(ParameterType.DepthShelf, 150.0,
			TestName = "Проверка корректного получения" +
					   " значения свойства DepthShelf.")]
		[TestCase(ParameterType.HangerHeight, 900.0,
			TestName = "Проверка корректного получения" +
					   " значения свойства HangerHeight.")]
		[TestCase(ParameterType.HangerWidth, 750.0,
			TestName = "Проверка корректного получения" +
					   " значения свойства HangerWidth.")]
		[TestCase(ParameterType.SectionWidth, 250.0,
			TestName = "Проверка корректного получения" +
					   " значения свойства SectionWidth.")]
		public void TestGetValue_CorrectGetValue(ParameterType parameterType, double value)
		{
			var fenceParameters = Parameters;

			var expected = value;

			fenceParameters.SetValue(parameterType, value);

			var actual = fenceParameters.GetValue(parameterType);

			Assert.AreEqual(expected, actual, "Вернулось некорректное значение.");
		}

		[TestCase(ParameterType.WidthShelf, 550.0,
			TestName = "Проверка корректной записи значения свойства WidthShelf." +
					   " Не должно выбрасывать исключения.")]
		[TestCase(ParameterType.DepthShelf, 150.0,
			TestName = "Проверка корректной записи значения свойства DepthShelf." +
							 " Не должно выбрасывать исключения.")]
		[TestCase(ParameterType.HangerHeight, 900.0,
			TestName = "Проверка корректной записи значения свойства HangerHeight." +
							 " Не должно выбрасывать исключения.")]
		[TestCase(ParameterType.HangerWidth, 750.0,
			TestName = "Проверка корректной записи значения свойства HangerWidth." +
							 " Не должно выбрасывать исключения.")]
		[TestCase(ParameterType.SectionWidth, 250.0,
			TestName = "Проверка корректной записи значения свойства SectionWidth." +
							 " Не должно выбрасывать исключения.")]
		public void TestSetValue_CorrectSetValue(ParameterType parameterType, double value)
		{
			var fenceParameters = Parameters;

			Assert.DoesNotThrow(() => fenceParameters.SetValue(parameterType, value),
				"Не удалось присвоить корректное значение.");
		}

		[TestCase(ParameterType.WidthShelf, 9.0,
			TestName = "Проверка некорректной передачи значения свойства WidthShelf," +
					   " меньшему минимальному." +
					   "  Должно выбросить исключение.")]
		[TestCase(ParameterType.WidthShelf, 10000.0,
			TestName = "Проверка некорректной передачи значения свойства WidthShelf," +
					   " большему максимальному." +
					   " Должно выбросить исключение.")]
		[TestCase(ParameterType.DepthShelf, 9.0,
			TestName = "Проверка некорректной передачи значения свойства DepthShelf," +
					   " меньшему минимальному." +
					   "  Должно выбросить исключение.")]
		[TestCase(ParameterType.DepthShelf, 10000.0,
			TestName = "Проверка некорректной передачи значения свойства DepthShelf," +
					   " большему максимальному." +
					   " Должно выбросить исключение.")]
		[TestCase(ParameterType.HangerHeight, 9.0,
			TestName = "Проверка некорректной передачи значения свойства HangerHeight," +
					   " меньшему минимальному." +
					   "  Должно выбросить исключение.")]
		[TestCase(ParameterType.HangerHeight, 10000.0,
			TestName = "Проверка некорректной передачи значения свойства HangerHeight," +
					   " большему максимальному." +
					   " Должно выбросить исключение.")]
		[TestCase(ParameterType.HangerWidth, 9.0,
			TestName = "Проверка некорректной передачи значения свойства HangerWidth," +
					   " меньшему минимальному." +
					   "  Должно выбросить исключение.")]
		[TestCase(ParameterType.HangerWidth, 10000.0,
			TestName = "Проверка некорректной передачи значения свойства HangerWidth," +
					   " большему максимальному." +
					   " Должно выбросить исключение.")]
		[TestCase(ParameterType.SectionWidth, 9.0,
			TestName = "Проверка некорректной передачи значения свойства SectionWidth," +
					   " меньшему минимальному." +
					   "  Должно выбросить исключение.")]
		[TestCase(ParameterType.SectionWidth, 10000.0,
			TestName = "Проверка некорректной передачи значения свойства SectionWidth," +
					   " большему максимальному." +
					   " Должно выбросить исключение.")]
		public void TestSetValue_IncorrectSetValue(ParameterType parameterType, double value)
		{
			var fenceParameters = Parameters;
			var errors = fenceParameters.SetValue(parameterType, value);
			Assert.IsTrue(errors.Any(),
				$"Присвоило значение не входящие в диапазон.");
		}
	}
}
