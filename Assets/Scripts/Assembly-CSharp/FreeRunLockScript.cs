using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002A RID: 42
public class FreeRunLockScript : MonoBehaviour
{
	// Token: 0x060000A7 RID: 167 RVA: 0x000059C4 File Offset: 0x00003BC4
	private void Start()
	{
		if (PlayerPrefs.GetFloat("FreeRunUnlocked") == 1f)
		{
			this.imageRenderer.sprite = this.normal;
			this.button.enabled = true;
			return;
		}
		this.imageRenderer.sprite = this.locked;
		this.button.enabled = false;
		this.text.text = string.Empty;
	}

	// Token: 0x040000FF RID: 255
	public Sprite locked;

	// Token: 0x04000100 RID: 256
	public Sprite normal;

	// Token: 0x04000101 RID: 257
	public Image imageRenderer;

	// Token: 0x04000102 RID: 258
	public Text text;

	// Token: 0x04000103 RID: 259
	public Button button;
}
