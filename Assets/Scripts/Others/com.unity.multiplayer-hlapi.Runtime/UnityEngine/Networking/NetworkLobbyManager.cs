using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkLobbyManager : NetworkManager
	{
		[SerializeField]
		private bool m_ShowLobbyGUI;
		[SerializeField]
		private int m_MaxPlayers;
		[SerializeField]
		private int m_MaxPlayersPerConnection;
		[SerializeField]
		private int m_MinPlayers;
		[SerializeField]
		private NetworkLobbyPlayer m_LobbyPlayerPrefab;
		[SerializeField]
		private GameObject m_GamePlayerPrefab;
		[SerializeField]
		private string m_LobbyScene;
		[SerializeField]
		private string m_PlayScene;
		public NetworkLobbyPlayer[] lobbySlots;
	}
}
