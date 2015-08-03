using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dynagon {

	public class Polygon3D : Polygon {

		public Polygon3D(GameObject gameObject, List<Vector3> vertices) : base(gameObject, vertices) {}

		protected Vector3 GetCentroid() {
			var uniqs = vertices.Distinct();
			//Distinct()继承的是ienumerable的方法，返回的是IEnumerable类型的结果
			return uniqs.Aggregate(Vector3.zero, (sum, v) => (sum + v)) / uniqs.Count();
			//其实这个我之前也自己撸了一个method出来，不过比起这里两行搞定，我那个就真是没脸见人的了
		}

		protected override void OptimizeIndexes() {
			var centroid = GetCentroid();
			foreach (var i in Enumerable.Range(0, indexes.Count/3).Select(i => i * 3)) {
				var center = GetCenterOfTriangle(i);
				if (Vector3.Dot(GetNormalOfTriangle(i), center - centroid) < 0) {
					ReverseSurface(i);
				}
			}
		}
		
	}

}
