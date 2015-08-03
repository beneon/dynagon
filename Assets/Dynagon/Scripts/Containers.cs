using System.Collections;
using System.Collections.Generic;

namespace Dynagon {
	
	internal class Counter<Type> : IEnumerable {
		//Counter继承IEnumerable的接口
		//话说IEnumerabe是个啥？
		
		protected Dictionary<Type, int> dict;
		
		public Counter() {
			dict = new Dictionary<Type, int>();
		}
		
		public void Add(Type key) {
			if (dict.ContainsKey(key)) {
				dict[key]++;
			}
			else {
				dict.Add(key, 1);
			}
		}
		
		public IEnumerator GetEnumerator() {
			return dict.GetEnumerator();
			//GetEnumerator怎么用
		}
	}
	
}
