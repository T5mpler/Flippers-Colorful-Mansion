using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000042 RID: 66
public class ResetButtonScript : MonoBehaviour
{
	// Token: 0x0600014D RID: 333 RVA: 0x0000E358 File Offset: 0x0000C558
	public void Awake()
	{
		this.resetImage.enabled = false;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000E366 File Offset: 0x0000C566
	private void Start()
	{
		this.resetButton.onClick.AddListener(new UnityAction(this.BeginResetting));
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000E384 File Offset: 0x0000C584
	private void BeginResetting()
	{
		base.StartCoroutine(this.ResetGame());
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000E393 File Offset: 0x0000C593
	public IEnumerator ResetGame()
	{
		for (int i = 0; i < 2; i++)
		{
			this.audioDevices[i].Stop();
		}
		this.audioDevice.PlayOneShot(this.aud_reset);
		this.resetImage.enabled = true;
		this.resetAnimator.SetTrigger("fadeIn");
		this.timer = 2f;
		while (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			yield return null;
		}
		this.resetAnimator.SetTrigger("fadeOut");
		this.timer = 0.085f;
		while (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadSceneAsync("Warning");
		PlayerPrefs.DeleteAll();
		yield break;
	}

	// Token: 0x040002CE RID: 718
	public Button resetButton;

	// Token: 0x040002CF RID: 719
	public Animator resetAnimator;

	// Token: 0x040002D0 RID: 720
	public AudioSource audioDevice;

	// Token: 0x040002D1 RID: 721
	public float timer;

	// Token: 0x040002D2 RID: 722
	public RawImage resetImage;

	// Token: 0x040002D3 RID: 723
	public AudioClip aud_reset;

	// Token: 0x040002D4 RID: 724
	public AudioSource[] audioDevices = new AudioSource[2];
}
