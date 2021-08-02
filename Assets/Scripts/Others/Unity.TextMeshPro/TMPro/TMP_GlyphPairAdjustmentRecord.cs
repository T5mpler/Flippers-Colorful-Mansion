using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_GlyphPairAdjustmentRecord
	{
		[SerializeField]
		private TMP_GlyphAdjustmentRecord m_FirstAdjustmentRecord;
		[SerializeField]
		private TMP_GlyphAdjustmentRecord m_SecondAdjustmentRecord;
		[SerializeField]
		private FontFeatureLookupFlags m_FeatureLookupFlags;
	}
}
