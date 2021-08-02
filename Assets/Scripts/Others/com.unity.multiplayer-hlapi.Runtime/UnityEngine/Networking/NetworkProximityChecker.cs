namespace UnityEngine.Networking
{
	public class NetworkProximityChecker : NetworkBehaviour
	{
		public enum CheckMethod
		{
			Physics3D = 0,
			Physics2D = 1,
		}

		public int visRange;
		public float visUpdateInterval;
		public CheckMethod checkMethod;
		public bool forceHidden;
	}
}
