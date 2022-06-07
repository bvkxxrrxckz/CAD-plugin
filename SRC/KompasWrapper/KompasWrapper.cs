using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using Kompas6Constants3D;

namespace KompasWrapper
{
	/// <summary>
	/// Класс для работы с Компас 3D.
	/// </summary>
    public class KompasWrapper
    {
		/// <summary>
		/// Возвращает экземпляр Компас 3D
		/// </summary>
		public KompasObject KompasObject { get; private set; }

		/// <summary>
		/// Запускает Компас 3D
		/// </summary>
		public void RunKompas()
		{
			if (KompasObject == null)
			{
				var kompasType = Type.GetTypeFromProgID(
					"KOMPAS.Application.5");
				KompasObject = (KompasObject)Activator.CreateInstance(kompasType);
			}

			if (KompasObject != null)
			{
				var retry = true;
				short tried = 0;
				while (retry)
				{
					try
					{
						tried++;
						KompasObject.Visible = true;
						retry = false;
					}
					catch (COMException)
					{
						var kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
						KompasObject =
							(KompasObject)Activator.CreateInstance(kompasType);

						if (tried > 3)
						{
							retry = false;
						}
					}
				}

				KompasObject.ActivateControllerAPI();
			}
		}

		/// <summary>
		/// Выдавливание объекта
		/// </summary>
		/// <param name="part"></param>
		/// <param name="sketch"></param>
		/// <param name="height"></param>
		public void BossExtrusion(ksPart part, ksEntity sketch, double height)
		{
			ksEntity extrude = part.NewEntity((int)Obj3dType.o3d_bossExtrusion);
			ksBossExtrusionDefinition extrudeDefinition = extrude.GetDefinition();
			extrudeDefinition.directionType = (int)Direction_Type.dtNormal;
			extrudeDefinition.SetSketch(sketch);
			ksExtrusionParam extrudeParam = extrudeDefinition.ExtrusionParam();
			extrudeParam.depthNormal = height;
			extrude.Create();
		}

        /// <summary>
		/// Построение на эскизе прямоугольника через две точки.
		/// </summary>
		/// <param name="document2D">2-D эскиз.</param>
		/// <param name="point1">Первая точка.</param>
		/// <param name="point2">Вторая точка.</param>
		public void CreateTwoPointRectangle(ksDocument2D document2D, Point point1, Point point2)
		{
			document2D.ksLineSeg(point1.X, -point1.Y, point2.X, -point1.Y, 1);
			document2D.ksLineSeg(point2.X, -point1.Y, point2.X, -point2.Y, 1);
			document2D.ksLineSeg(point1.X, -point2.Y, point2.X, -point2.Y, 1);
			document2D.ksLineSeg(point1.X, -point1.Y, point1.X, -point2.Y, 1);
		}
	}
}
