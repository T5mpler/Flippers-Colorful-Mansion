using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkIdentity : MonoBehaviour
	{
		[SerializeField]
		private NetworkSceneId m_SceneId;
		[SerializeField]
		private NetworkHash128 m_AssetId;
		[SerializeField]
		private bool m_ServerOnly;
		[SerializeField]
		private bool m_LocalPlayerAuthority;
	}
}
