using System;
using UnityEngine;

namespace UnityEngine.Rendering
{
	[Serializable]
	public class VolumeComponent : ScriptableObject
	{
		public bool active;
		[SerializeField]
		private bool m_AdvancedMode;
	}
}
