namespace KompasWrapper
{
	/// <summary>
	/// Класс точки
	/// </summary>
	public class Point
	{
		/// <summary>
		/// Х координата
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// Y координата
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="x">Х координата</param>
		/// <param name="y">Y координата</param>
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}