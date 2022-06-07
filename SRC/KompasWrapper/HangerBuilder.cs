using System.Collections.Generic;
using Core;
using Kompas6API5;
using Kompas6Constants3D;

namespace KompasWrapper
{
	/// <summary>
	/// Построитель вешалки.
	/// </summary>
	public class HangerBuilder
	{
		/// <summary>
		/// Толщина стенки.
		/// </summary>
		private const double WallWidth = 20.0;

		/// <summary>
		/// Параметры детали.
		/// </summary>
		private HangerParameters _parameters;

		/// <summary>
		/// Экземпляр класса работы с Компас 3D.
		/// </summary>
		private readonly KompasWrapper _kompasWrapper;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public HangerBuilder()
		{
			_kompasWrapper = new KompasWrapper();
		}

		/// <summary>
		/// Построить вешалку.
		/// </summary>
		/// <param name="parameters"></param>
		public void Build(HangerParameters parameters)
		{
			_parameters = parameters;

			_kompasWrapper.RunKompas();
			ksDocument3D document3D = _kompasWrapper.KompasObject.Document3D();
			document3D.Create();
			ksPart part = document3D.GetPart((int)Part_Type.pTop_Part);

			CreateBody(part);
			CreateShelf(part);
		}

		/// <summary>
		/// Создать полку вешалки.
		/// </summary>
		private void CreateShelf(ksPart part)
		{
			ksEntity plane = part.GetDefaultEntity((int)Obj3dType.o3d_planeXOY);
			ksEntity sketch = part.NewEntity((int)Obj3dType.o3d_sketch);
			ksSketchDefinition sketchDefinition = sketch.GetDefinition();
			sketchDefinition.SetPlane(plane);
			sketch.Create();
			var hangerWidth = _parameters.GetValue(ParameterType.HangerWidth);
			var depthShelf = _parameters.GetValue(ParameterType.DepthShelf);
			var widthShelf = _parameters.GetValue(ParameterType.WidthShelf);
			var firstPoint = new Point((hangerWidth - widthShelf) / 2, WallWidth);
			var secondPoint = new Point((hangerWidth + widthShelf) / 2, WallWidth * 2);
			// Входим в режим редактирования эскиза
			ksDocument2D document2D = sketchDefinition.BeginEdit();
			_kompasWrapper.CreateTwoPointRectangle(document2D, firstPoint, secondPoint);
			sketchDefinition.EndEdit();
			_kompasWrapper.BossExtrusion(part, sketch, depthShelf);
		}

		/// <summary>
		/// Создать тело вешалки.
		/// </summary>
		private void CreateBody(ksPart part)
		{
			ksEntity plane = part.GetDefaultEntity((int)Obj3dType.o3d_planeXOY);
			ksEntity sketch = part.NewEntity((int)Obj3dType.o3d_sketch);
			ksSketchDefinition sketchDefinition = sketch.GetDefinition();
			sketchDefinition.SetPlane(plane);
			sketch.Create();
			var hangerWidth = _parameters.GetValue(ParameterType.HangerWidth);
			var hangerHeight = _parameters.GetValue(ParameterType.HangerHeight);
			var sectionWidth = _parameters.GetValue(ParameterType.SectionWidth);
			// Количество созданных секций
			var times = 3;
			var distanceBetweenBoards = (hangerWidth - 3 * sectionWidth) / 2;
			var firstPoint = new Point(0, 0);
			var secondPoint = new Point(sectionWidth, hangerHeight);
			if (distanceBetweenBoards == 0.0)
			{
				secondPoint = new Point(hangerWidth, hangerHeight);
				times = 1;
			}

			// Входим в режим редактирования эскиза
			ksDocument2D document2D = sketchDefinition.BeginEdit();
            var centerPoints = new List<Point>();
			for (int i = 0; i < times; i++)
			{
				_kompasWrapper.CreateTwoPointRectangle(document2D, firstPoint, secondPoint);
                centerPoints.Add(new Point((secondPoint.X + firstPoint.X) * 0.5,
                    (firstPoint.Y - secondPoint.Y) * 0.25));
				firstPoint = new Point(secondPoint.X + distanceBetweenBoards, 0);
				secondPoint = new Point(firstPoint.X + sectionWidth, hangerHeight);
			}

			sketchDefinition.EndEdit();
			_kompasWrapper.BossExtrusion(part, sketch, -WallWidth);
            CreateHooks(part, centerPoints);
        }

		/// <summary>
		/// Создает крючки.
		/// </summary>
        private void CreateHooks(ksPart part, List<Point> centerPoints)
        {
			ksEntity plane = part.GetDefaultEntity((int)Obj3dType.o3d_planeXOY);
            ksEntity sketch = part.NewEntity((int)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = sketch.GetDefinition();
            sketchDefinition.SetPlane(plane);
            sketch.Create();

            ksDocument2D document2D = sketchDefinition.BeginEdit();
			foreach (var point in centerPoints)
            {
                document2D.ksCircle(point.X, point.Y, WallWidth, 1);
            }

            sketchDefinition.EndEdit();
            _kompasWrapper.BossExtrusion(part, sketch, WallWidth * 2);
        }
    }
}