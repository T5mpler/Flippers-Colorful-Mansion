using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkDiscovery : MonoBehaviour
	{
		[SerializeField]
		private int m_BroadcastPort;
		[SerializeField]
		private int m_BroadcastKey;
		[SerializeField]
		private int m_BroadcastVersion;
		[SerializeField]
		private int m_BroadcastSubVersion;
		[SerializeField]
		private int m_BroadcastInterval;
		[SerializeField]
		private bool m_UseNetworkManager;
		[SerializeField]
		private string m_BroadcastData;
		[SerializeField]
		private bool m_ShowGUI;
		[SerializeField]
		private int m_OffsetX;
		[SerializeField]
		private int m_OffsetY;
	}
}
