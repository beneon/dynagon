using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//Linq引用

namespace Dynagon {

	public static class Factory {

		public static Polygon Create(GameObject gameObject, List<Vector2> vertices) {
			return new Polygon2D(
				gameObject,
				Triangulator2D.Triangulate(vertices.Select(v => (Vector3)v).ToList())
			).Build();
			//Polygon2D().Build();?
		}

		public static Polygon Create(string name, List<Vector2> vertices) {
			return Create(new GameObject(name), vertices);
			//转给第一个
		}

		public static Polygon Create(List<Vector2> vertices) {
			return Create(new GameObject(), vertices);
			//转给第一个，无object名
		}

		public static Polygon Create(GameObject gameObject, List<Vector3> vertices) {
			return new Polygon3D(
				gameObject,
				Triangulator3D.Triangulate(vertices)
			).Build();
			//Polygon3D().Build();
		}

		public static Polygon Create(string name, List<Vector3> vertices) {
			return Create(new GameObject(name), vertices);
			//转给第4个
		}

		public static Polygon Create(List<Vector3> vertices) {
			return Create(new GameObject(), vertices);
			//转给第4个，无object名
		}
		//六个Create的重载
		//最后都是发给Polygon2D/3D(gameObject引用，Vector2/3 泛型引用（vertices）>发给Triangulator2D/3D.triangluate(vertices(3d)/vertices2d>3d)).Build
		//Polygon 2D/3D?
		//Triangulator2D/3D?
		//lambda 表达式
		//polygon

	}

}
