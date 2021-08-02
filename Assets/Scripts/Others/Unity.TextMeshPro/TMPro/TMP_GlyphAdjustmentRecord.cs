using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public struct TMP_GlyphAdjustmentRecord
	{
		[SerializeField]
		private uint m_GlyphIndex;
		[SerializeField]
		private TMP_GlyphValueRecord m_GlyphValueRecord;
	}
}
