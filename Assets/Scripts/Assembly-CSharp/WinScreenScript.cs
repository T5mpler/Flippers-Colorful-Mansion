using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200004E RID: 78
public class WinScreenScript : MonoBehaviour
{
	// Token: 0x06000185 RID: 389 RVA: 0x0000F364 File Offset: 0x0000D564
	private void Start()
	{
		if (PlayerPrefs.GetFloat("AlreadySeen") == 1f)
		{
			this.button.gameObject.SetActive(true);
		}
		else
		{
			this.button.gameObject.SetActive(false);
		}
		if (Mathf.RoundToInt(Random.Range(0f, 8f)) >= 4)
		{
			this.audioDevice.PlayOneShot(this.ucnWin);
		}
		else
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 3f));
			this.audioDevice.PlayOneShot(this.mus_Credits[num]);
		}
		base.StartCoroutine(this.ChangeColor());
		this.timer = 0.75f;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000F410 File Offset: 0x0000D610
	private void Update()
	{
		this.cursorController.UnlockCursor();
		this.button.onClick.AddListener(new UnityAction(this.FinishScreen));
		if (!this.audioDevice.isPlaying)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.audioDevice.PlayOneShot(this.mus_Credits[num]);
		}
		if (this.time > 0f)
		{
			this.time -= Time.deltaTime;
		}
		else if (this.stopped)
		{
			PlayerPrefs.SetFloat("AlreadySeen", 1f);
			Application.Quit();
		}
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		else
		{
			this.timer = 0.75f;
			base.StartCoroutine(this.ChangeColor());
		}
		if (this.mainScreen.GetComponent<RectTransform>().anchoredPosition.y >= 3750f & !this.stopped)
		{
			this.FinishScreen();
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000F520 File Offset: 0x0000D720
	public IEnumerator ChangeColor()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, (float)(this.colorArray.Length - 1)));
		for (int i = 0; i < 8; i++)
		{
			this.outline[i].effectColor = this.colorArray[num];
			this.outline[i].effectDistance = new Vector2(1f, -1f);
			this.outline[i].effectColor += new Color(0f, 0f, 0f, 1f);
		}
		yield break;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000F530 File Offset: 0x0000D730
	public void FinishScreen()
	{
		if (this.mainScreen.GetComponent<RectTransform>().anchoredPosition.y < 3750f)
		{
			this.mainScreen.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 3750f);
		}
		this.button.gameObject.SetActive(false);
		this.stopped = true;
		this.mainScreen.GetComponent<AutoScrollingScript>().enabled = false;
		this.time = 5f;
		for (int i = 0; i < 8; i++)
		{
			this.outline[i].GetComponent<Text>().enabled = false;
		}
	}

	// Token: 0x04000319 RID: 793
	public AudioSource audioDevice;

	// Token: 0x0400031A RID: 794
	public Color[] colorArray;

	// Token: 0x0400031B RID: 795
	public Outline[] outline = new Outline[8];

	// Token: 0x0400031C RID: 796
	public float timer;

	// Token: 0x0400031D RID: 797
	public AudioClip[] mus_Credits = new AudioClip[4];

	// Token: 0x0400031E RID: 798
	public AudioClip ucnWin;

	// Token: 0x0400031F RID: 799
	public bool stopped;

	// Token: 0x04000320 RID: 800
	public GameObject mainScreen;

	// Token: 0x04000321 RID: 801
	public float time;

	// Token: 0x04000322 RID: 802
	public Button button;

	// Token: 0x04000323 RID: 803
	public CursorControllerScript cursorController;
}
