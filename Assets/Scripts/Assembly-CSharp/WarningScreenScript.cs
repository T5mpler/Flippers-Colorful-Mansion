using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200004D RID: 77
public class WarningScreenScript : MonoBehaviour
{
	// Token: 0x06000181 RID: 385 RVA: 0x0000F2EE File Offset: 0x0000D4EE
	public void Start()
	{
		this.colorChangingSpeed = 1f;
		this.warningText.color += new Color(1f, 0f, 0f, 1f);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000F32A File Offset: 0x0000D52A
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			SceneManager.LoadScene("MainMenu");
		}
		if (!this.isColorChanging)
		{
			base.StartCoroutine(this.ChangeColor());
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000F352 File Offset: 0x0000D552
	public IEnumerator ChangeColor()
	{
		this.isColorChanging = true;
		this.warningText.color = Color.red;
		while (this.warningText.color.g < 1f)
		{
			this.warningText.color += new Color(0f, Time.deltaTime / this.colorChangingSpeed, 0f);
			yield return null;
		}
		while (this.warningText.color.r > 0f)
		{
			this.warningText.color -= new Color(Time.deltaTime / this.colorChangingSpeed, 0f, 0f);
			yield return null;
		}
		while (this.warningText.color.b < 1f)
		{
			this.warningText.color += new Color(0f, 0f, Time.deltaTime / this.colorChangingSpeed);
			yield return null;
		}
		while (this.warningText.color.g > 0f)
		{
			this.warningText.color -= new Color(0f, Time.deltaTime / this.colorChangingSpeed, 0f);
			yield return null;
		}
		while (this.warningText.color.r < 0f)
		{
			this.warningText.color += new Color(Time.deltaTime / this.colorChangingSpeed, 0f, 0f);
			yield return null;
		}
		while (this.warningText.color.b > 1f)
		{
			this.warningText.color -= new Color(0f, 0f, Time.deltaTime / this.colorChangingSpeed);
			yield return null;
		}
		this.isColorChanging = false;
		yield break;
	}

	// Token: 0x04000316 RID: 790
	public Text warningText;

	// Token: 0x04000317 RID: 791
	public bool isColorChanging;

	// Token: 0x04000318 RID: 792
	public float colorChangingSpeed;
}
