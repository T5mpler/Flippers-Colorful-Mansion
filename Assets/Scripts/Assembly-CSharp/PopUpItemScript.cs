using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class PopUpItemScript : MonoBehaviour
{
	// Token: 0x06000139 RID: 313 RVA: 0x0000DAB7 File Offset: 0x0000BCB7
	private void Start()
	{
		base.StartCoroutine(this.ItemPopUp());
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000DAC6 File Offset: 0x0000BCC6
	public IEnumerator ItemPopUp()
	{
		while (base.transform.localScale != new Vector3(2.5f, 2.5f, 2.5f))
		{
			base.transform.localScale += new Vector3(0.05f, 0.05f, 0f);
			yield return null;
		}
		base.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
		yield break;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000DAD5 File Offset: 0x0000BCD5
	public IEnumerator ItemPopDown()
	{
		while (base.transform.localScale != new Vector3(0f, 0f, 0f))
		{
			base.transform.localScale -= new Vector3(0.05f, 0.05f, 0f);
			yield return null;
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		yield break;
	}
}
