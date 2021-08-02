using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking.Match;

namespace UnityEngine.Networking
{
	public class NetworkManager : MonoBehaviour
	{
		[SerializeField]
		private int m_NetworkPort;
		[SerializeField]
		private bool m_ServerBindToIP;
		[SerializeField]
		private string m_ServerBindAddress;
		[SerializeField]
		private string m_NetworkAddress;
		[SerializeField]
		private bool m_DontDestroyOnLoad;
		[SerializeField]
		private bool m_RunInBackground;
		[SerializeField]
		private bool m_ScriptCRCCheck;
		[SerializeField]
		private float m_MaxDelay;
		[SerializeField]
		private LogFilter.FilterLevel m_LogLevel;
		[SerializeField]
		private GameObject m_PlayerPrefab;
		[SerializeField]
		private bool m_AutoCreatePlayer;
		[SerializeField]
		private PlayerSpawnMethod m_PlayerSpawnMethod;
		[SerializeField]
		private string m_OfflineScene;
		[SerializeField]
		private string m_OnlineScene;
		[SerializeField]
		private List<GameObject> m_SpawnPrefabs;
		[SerializeField]
		private bool m_CustomConfig;
		[SerializeField]
		private int m_MaxConnections;
		[SerializeField]
		private ConnectionConfig m_ConnectionConfig;
		[SerializeField]
		private GlobalConfig m_GlobalConfig;
		[SerializeField]
		private List<QosType> m_Channels;
		[SerializeField]
		private bool m_UseWebSockets;
		[SerializeField]
		private bool m_UseSimulator;
		[SerializeField]
		private int m_SimulatedLatency;
		[SerializeField]
		private float m_PacketLossPercentage;
		[SerializeField]
		private int m_MaxBufferedPackets;
		[SerializeField]
		private bool m_AllowFragmentation;
		[SerializeField]
		private string m_MatchHost;
		[SerializeField]
		private int m_MatchPort;
		[SerializeField]
		public string matchName;
		[SerializeField]
		public uint matchSize;
		public bool isNetworkActive;
		public NetworkMatch matchMaker;
	}
}
