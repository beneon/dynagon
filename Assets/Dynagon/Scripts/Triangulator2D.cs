using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dynagon {

	public static class Triangulator2D {
		
		private static Triangle GetHugeTriangle(List<Vector3> vertices) {
			var max = new Vector3(float.MinValue, float.MinValue);
			var min = new Vector3(float.MaxValue, float.MaxValue);
			foreach (var v in vertices) {
				max = Vector3.Max(max, v);
				min = Vector3.Min(min, v);
			}
			var c = (max + min) * 0.5f;
			var radius = Vector3.Distance(max,c) + 1f;
			return new Triangle(
				new Vector3(c.x - Mathf.Sqrt(3.0f) * radius, c.y - radius),
				new Vector3(c.x + Mathf.Sqrt(3.0f) * radius, c.y - radius),
				new Vector3(c.x, c.y + 2f * radius)
				);
		}
		//这个就是画一个超大的等边三角形，把所有的点都包括进去（圆圈半径比起实际需要还多出来1）
		
		private static Circle GetCircumcircle(Triangle tri) {
			var p = tri.p;
			var m = 2f * ((p[1].x - p[0].x) * (p[2].y - p[0].y) - (p[1].y - p[0].y) * (p[2].x - p[0].x));
			var center = new Vector3(
				((p[2].y - p[0].y) * (p[1].x * p[1].x - p[0].x * p[0].x + p[1].y * p[1].y - p[0].y * p[0].y) + (p[0].y - p[1].y) * (p[2].x * p[2].x - p[0].x * p[0].x + p[2].y * p[2].y - p[0].y * p[0].y)) / m,
				((p[0].x - p[2].x) * (p[1].x * p[1].x - p[0].x * p[0].x + p[1].y * p[1].y - p[0].y * p[0].y) + (p[1].x - p[0].x) * (p[2].x * p[2].x - p[0].x * p[0].x + p[2].y * p[2].y - p[0].y * p[0].y)) / m
				);
			return new Circle(center, Vector3.Distance(p[0], center));
		}
		
		private static List<Triangle> GetDelaunayTriangles(List<Vector3> vertices) {
			var tris = new HashSet<Triangle>();
			
			Triangle huge = GetHugeTriangle(vertices);
			tris.Add(huge);
			
			foreach (var v in vertices) {
				var counter = new Counter<Triangle>();
				var trash = new List<Triangle>();
				
				foreach (var t in tris) {
					var circle = GetCircumcircle(t);
					if (Vector3.Distance(circle.center, v) < circle.radius) {
						counter.Add(new Triangle(v, t.p[0], t.p[1]));
						counter.Add(new Triangle(v, t.p[0], t.p[2]));
						counter.Add(new Triangle(v, t.p[1], t.p[2]));
						trash.Add(t);
					}
				}

				tris.RemoveWhere(t => trash.Contains(t));
				
				foreach (KeyValuePair<Triangle, int> entry in counter) {
					if (entry.Value == 1) {
						tris.Add(entry.Key);
					}
				}
			}

			tris.RemoveWhere(t => huge.ShareVertex(t));
			return tris.ToList();
		}
		//这里已经用到Delaunay三角形划分算法了。这个就有点点麻烦，有时间再研究研究
		//不过Delaunay用在这里真的是很棒，因为用这个才能够保证对有限点集建立三角形，且彼此不相交叉
		
		public static List<Vector3> Triangulate(List<Vector3> vertices) {
			return Function.ConvertTrianglesToList(GetDelaunayTriangles(vertices));
		}
		
	}
}
