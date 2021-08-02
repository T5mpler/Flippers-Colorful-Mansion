using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkManagerHUD : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField]
		public bool showGUI;
		[SerializeField]
		public int offsetX;
		[SerializeField]
		public int offsetY;
	}
}
