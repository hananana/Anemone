using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TestSpace
{

	public class TestClass : MonoBehaviour
	{
		[SerializeField]
		float m_mapScale = 1.0f;

		int foo { get; set; }


		public void DoSomething()
		{
			var scaledSize = m_defaultSize * m_mapScale;
			var chipSize = 32;
			var chipPrefabPath = "Prefabs/Cell";
			var camera = gameObject.Parent().Descendants().Where(t => t.name == "Camera").Single().GetComponent<Camera>();
			var floorLayer = gameObject.Descendants().Where(t => t.name == "FloorLayer").Single().transform;

			camera.orthographicSize = m_defaultSize.y * m_mapScale / 2;

			cells.Where(c => c.x == 0).Where(c => c.y == 0)
                .Select(c => new {hoge = c.x, fuga = c.y }).ToList().ForEach(c =>
				{
					c.SetLocalScaleX(0);
				});

			PrefabPoolManager.Instance.Prepare(chipPrefabPath,
			                                   cells.Count);
			cells.ForEach(cell =>
				{
					var control = PrefabPoolManager.Instance.Get(chipPrefabPath, floorLayer).GetComponent<CellControl>();
					control.transform.SetLocalX(chipSize * cell.x);
					control.transform.SetLocalY(chipSize * cell.y);
					control.UpdateCell(cell.cellType);
				});

			DrawLine(chipSize, cells.Max(c => c.y) + 1, cells.Max(c => c.x) + 1);
		}

		void DrawLine(int chipSize, int rowCount, int colCount)
		{
			var linePreafabPath = "Prefabs/Line";

			PrefabPoolManager.Instance.Prepare(linePreafabPath,
			                                   (rowCount + 1) * (colCount + 1));
			var lineColor = new Color(1, 1, 1, 0.3f);
			var lineLayer = gameObject.Descendants().Where(t => t.name == "LineLayer").Single().transform;
			Enumerable.Range(0, colCount + 1).ToList().ForEach(col =>
				{
					var renderer = PrefabPoolManager.Instance.Get(linePreafabPath, lineLayer).GetComponent<SpriteRenderer>();
					var bottomY = -chipSize / 2;
					var topY = rowCount * chipSize - chipSize / 2;
					var diffY = topY - bottomY;
					var pos = new Vector3(col * chipSize - chipSize / 2,
					                      rowCount * chipSize / 2 - chipSize / 2,
					                      0);
					renderer.transform.localPosition = pos;
					renderer.color = lineColor;
					renderer.transform.SetLocalScaleX(50);
					renderer.transform.SetLocalScaleY(diffY * 50);
				});
			Enumerable.Range(0, rowCount + 1).ToList().ForEach(row =>
				{
					var renderer = PrefabPoolManager.Instance.Get(linePreafabPath, lineLayer).GetComponent<SpriteRenderer>();
					var leftX = -chipSize / 2;
					var rightX = colCount * chipSize - chipSize / 2;
					var diffX = rightX - leftX;
					var pos = new Vector3(colCount * chipSize / 2 - chipSize / 2, row * chipSize - chipSize / 2, 0);
					renderer.transform.localPosition = pos;
					renderer.color = lineColor;
					renderer.transform.SetLocalScaleX(diffX * 50);
					renderer.transform.SetLocalScaleY(50);
				});
		}
	}
}
