using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkMigrationManager : MonoBehaviour
	{
		[SerializeField]
		private bool m_HostMigration;
		[SerializeField]
		private bool m_ShowGUI;
		[SerializeField]
		private int m_OffsetX;
		[SerializeField]
		private int m_OffsetY;
	}
}
