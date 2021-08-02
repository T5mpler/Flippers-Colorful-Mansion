using UnityEngine;

namespace UnityEngine.Rendering
{
	public class Volume : MonoBehaviour
	{
		public bool isGlobal;
		public float priority;
		public float blendDistance;
		public float weight;
		public VolumeProfile sharedProfile;
	}
}
