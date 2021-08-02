using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkAnimator : NetworkBehaviour
	{
		[SerializeField]
		private Animator m_Animator;
		[SerializeField]
		private uint m_ParameterSendBits;
		public string param0;
		public string param1;
		public string param2;
		public string param3;
		public string param4;
		public string param5;
	}
}
