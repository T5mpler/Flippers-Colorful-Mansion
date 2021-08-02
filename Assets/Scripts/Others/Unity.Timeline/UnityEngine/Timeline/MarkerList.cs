using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Timeline
{
	[Serializable]
	internal struct MarkerList
	{
		[SerializeField]
		private List<ScriptableObject> m_Objects;
	}
}
