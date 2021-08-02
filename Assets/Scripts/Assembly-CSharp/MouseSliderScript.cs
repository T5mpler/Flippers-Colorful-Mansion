using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
public class MouseSliderScript : MonoBehaviour
{
	// Token: 0x0600010F RID: 271 RVA: 0x0000B724 File Offset: 0x00009924
	private void Start()
	{
		if (PlayerPrefs.GetFloat("MouseSensitivity") == 0f)
		{
			PlayerPrefs.SetFloat("MouseSensitvity", 1f);
		}
		this.slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000B75B File Offset: 0x0000995B
	public void SetMouseSensitvity(float value)
	{
		value = this.slider.value;
		PlayerPrefs.SetFloat("MouseSensitivity", value);
	}

	// Token: 0x0400023B RID: 571
	public Slider slider;
}
