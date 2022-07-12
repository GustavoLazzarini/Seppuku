//Copyright Galactspace Studios 2022

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Scriptable.Abstract
{
	public abstract class GroupSo<T> : ScriptableObject
	{
		//Variables
		[Space]
		[SerializeField] private T[] _groupArray;
		public T[] GroupArray => _groupArray;
		public T this[int index] => _groupArray[index];
		public IEnumerator GetEnumerator() => _groupArray.GetEnumerator();

		public int Length => _groupArray.Length;

		public void Populate(T[] elements) => _groupArray = elements;
		public void Populate(List<T> elements) => _groupArray = elements.ToArray();
		public void Populate(IEnumerable<T> elements) => _groupArray = elements.ToArray();

		public T GetRandom()
		{
			if (GroupArray.Length == 0) return default;
			return GroupArray[Random.Range(0, GroupArray.Length)];
		}
		
		public T GetRandomExclude(T exclude) 
		{
			if (GroupArray.Length == 0) return default;
			if (GroupArray.Length == 1) return GroupArray[0];

			T element = GroupArray[Random.Range(0, GroupArray.Length)];
			while (ReferenceEquals(element, exclude)) element = GroupArray[Random.Range(0, GroupArray.Length)];

			return GroupArray[Random.Range(0, GroupArray.Length)];
		}
	}
}