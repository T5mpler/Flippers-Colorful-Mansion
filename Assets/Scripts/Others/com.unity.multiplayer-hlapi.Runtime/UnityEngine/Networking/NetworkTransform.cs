using UnityEngine;

namespace UnityEngine.Networking
{
	public class NetworkTransform : NetworkBehaviour
	{
		public enum TransformSyncMode
		{
			SyncNone = 0,
			SyncTransform = 1,
			SyncRigidbody2D = 2,
			SyncRigidbody3D = 3,
			SyncCharacterController = 4,
		}

		public enum AxisSyncMode
		{
			None = 0,
			AxisX = 1,
			AxisY = 2,
			AxisZ = 3,
			AxisXY = 4,
			AxisXZ = 5,
			AxisYZ = 6,
			AxisXYZ = 7,
		}

		public enum CompressionSyncMode
		{
			None = 0,
			Low = 1,
			High = 2,
		}

		[SerializeField]
		private TransformSyncMode m_TransformSyncMode;
		[SerializeField]
		private float m_SendInterval;
		[SerializeField]
		private AxisSyncMode m_SyncRotationAxis;
		[SerializeField]
		private CompressionSyncMode m_RotationSyncCompression;
		[SerializeField]
		private bool m_SyncSpin;
		[SerializeField]
		private float m_MovementTheshold;
		[SerializeField]
		private float m_VelocityThreshold;
		[SerializeField]
		private float m_SnapThreshold;
		[SerializeField]
		private float m_InterpolateRotation;
		[SerializeField]
		private float m_InterpolateMovement;
	}
}
