using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000046 RID: 70
public class SettingsScript : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerPrefs.GetString("ToggleVignette") != string.Empty) vignetteToggle.targetGraphic.GetComponent<Image>().sprite = bool.Parse(PlayerPrefs.GetString("ToggleVignette")) == true ? check : nothing;
		if (PlayerPrefs.GetString("ToggleChromaticAberration") != string.Empty) chromaticAberrationToggle.targetGraphic.GetComponent<Image>().sprite = bool.Parse(PlayerPrefs.GetString("ToggleChromaticAberration")) == true ? check : nothing;
		if (PlayerPrefs.GetString("ToggleBloom") != string.Empty) bloomToggle.targetGraphic.GetComponent<Image>().sprite = bool.Parse(PlayerPrefs.GetString("ToggleBloom")) == true ? check : nothing;
		if (PlayerPrefs.GetString("ToggleMotionBlur") != string.Empty) motionBlurToggle.targetGraphic.GetComponent<Image>().sprite = bool.Parse(PlayerPrefs.GetString("ToggleMotionBlur")) == true ? check : nothing;

		fpsImage.sprite = PlayerPrefs.GetFloat("fpsDisplay") == 1f ? check : nothing;
		dateImage.sprite = PlayerPrefs.GetFloat("Date") == 1f ? check : nothing;
		if (PlayerPrefs.GetFloat("Time") == 1f)
		{
			this.tImage.sprite = this.check;
		}
		else
		{
			this.tImage.sprite = this.nothing;
			PlayerPrefs.SetFloat("Date", 0f);
			this.dateClicks = 0;
			this.dateImage.sprite = this.nothing;
		}
		this.resolutions = Screen.resolutions;
		this.resolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < this.resolutions.Length; i++)
		{
			string item = this.resolutions[i].width + " x " + this.resolutions[i].height;
			list.Add(item);
			if (this.resolutions[i].width == Screen.width && this.resolutions[i].height == Screen.height)
			{
				value = i;
			}
		}
		this.resolutionDropdown.AddOptions(list);
		this.resolutionDropdown.value = value;
		this.resolutionDropdown.RefreshShownValue();
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000E6FC File Offset: 0x0000C8FC
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = this.resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000E72E File Offset: 0x0000C92E
	public void SetVolume(float volume)
	{
		this.audioMixer.SetFloat("volume", volume);
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000E74A File Offset: 0x0000C94A
	public void ToggleFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000E754 File Offset: 0x0000C954
	public void EnableFPSDisplay()
	{
		this.fpsClicks++;
		switch (fpsClicks)
		{
			case 0:
				PlayerPrefs.SetInt("fpsDisplay", 0);
				this.fpsImage.sprite = this.nothing;
				break;
			case 1:
				PlayerPrefs.SetInt("fpsDisplay", 1);
				this.fpsImage.sprite = this.check;
				break;
			case 2:
				this.fpsClicks = -1;
				this.EnableFPSDisplay();
				break;
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
	// Token: 0x06000163 RID: 355 RVA: 0x0000E854 File Offset: 0x0000CA54
	public void ShowTime()
	{
		this.tClicks++;
		switch (tClicks)
		{
			case 0:
				PlayerPrefs.SetFloat("Time", 0f);
				this.tImage.sprite = this.nothing;
				PlayerPrefs.SetFloat("Date", 0f);
				this.dateClicks = 0;
				this.dateImage.sprite = this.nothing;
				break;
			case 1:
				PlayerPrefs.SetFloat("Time", 1f);
				this.tImage.sprite = this.check;
				break;
			case 2:
				this.tClicks = -1;
				this.ShowTime();
				break;
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000E900 File Offset: 0x0000CB00
	public void ShowDate()
	{
		if (PlayerPrefs.GetFloat("Time") == 1f)
		{
			this.dateClicks++;
			switch (dateClicks)
			{
				case 0:
					PlayerPrefs.SetFloat("Date", 0f);
					this.dateImage.sprite = this.nothing;
					break;
				case 1:
					PlayerPrefs.SetFloat("Date", 1f);
					this.dateImage.sprite = this.check;
					break;
				case 2:
					this.dateClicks = -1;
					this.ShowDate();
					break;
			}
		}

	}
	public void ToggleVignette(bool value)
	{
		PlayerPrefs.SetString("ToggleVignette", value.ToString());
		Image image = vignetteToggle.targetGraphic.gameObject.GetComponent<Image>();
		image.sprite = value == true ? check : nothing;
	}
	public void ToggleChromaticAberration(bool value)
	{
		PlayerPrefs.SetString("ToggleChromaticAberration", value.ToString());
		Image image = chromaticAberrationToggle.targetGraphic.gameObject.GetComponent<Image>();
		image.sprite = value == true ? check : nothing;
	}
	public void ToggleBloom(bool value)
	{
		PlayerPrefs.SetString("ToggleBloom", value.ToString());
		Image image = bloomToggle.targetGraphic.gameObject.GetComponent<Image>();
		image.sprite = value == true ? check : nothing;
	}
	public void ToggleMotionBlur(bool value)
	{
		PlayerPrefs.SetString("ToggleMotionBlur", value.ToString());
		Image image = motionBlurToggle.targetGraphic.gameObject.GetComponent<Image>();
		image.sprite = value == true ? check : nothing;
	}

	// Token: 0x040002DA RID: 730
	public AudioMixer audioMixer;

	// Token: 0x040002DB RID: 731
	public Resolution[] resolutions;

	// Token: 0x040002DC RID: 732
	public Dropdown resolutionDropdown;

	// Token: 0x040002DD RID: 733
	public Image fpsImage;

	// Token: 0x040002DE RID: 734
	public Sprite check;

	// Token: 0x040002DF RID: 735
	public Sprite nothing;

	// Token: 0x040002E0 RID: 736
	public Button button;

	// Token: 0x040002E1 RID: 737
	public int fpsClicks;

	// Token: 0x040002E4 RID: 740
	public int tClicks;

	// Token: 0x040002E5 RID: 741
	public Image tImage;

	// Token: 0x040002E6 RID: 742
	public int dateClicks;

	// Token: 0x040002E7 RID: 743
	public Image dateImage;
	public Toggle vignetteToggle, chromaticAberrationToggle, bloomToggle, motionBlurToggle;
}
