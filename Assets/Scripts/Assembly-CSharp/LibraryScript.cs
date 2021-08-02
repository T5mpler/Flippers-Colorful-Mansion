using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class LibraryScript : MonoBehaviour
{
	// Token: 0x060000FA RID: 250 RVA: 0x0000971E File Offset: 0x0000791E
	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			base.StartCoroutine(this.SilentLibrary("silent"));
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00009744 File Offset: 0x00007944
	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" & !this.player.isDeaf)
		{
			base.StartCoroutine(this.SilentLibrary("normal"));
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00009779 File Offset: 0x00007979
	private IEnumerator SilentLibrary(string type)
	{
		if (type == "silent")
		{
			while (AudioListener.volume > 0f)
			{
				AudioListener.volume -= Time.deltaTime;
				yield return null;
			}
			AudioListener.volume = 0f;
		}
		else if (type == "normal")
		{
			while (AudioListener.volume < 1f)
			{
				AudioListener.volume += Time.deltaTime;
				yield return null;
			}
			AudioListener.volume = 1f;
		}
		yield break;
	}

	// Token: 0x040001FC RID: 508
	public PlayerScript player;
}
