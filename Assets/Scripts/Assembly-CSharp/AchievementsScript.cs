using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class AchievementsScript : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x0000217C File Offset: 0x0000037C
	private void Start()
	{
		if (PlayerPrefs.GetFloat("FreeRunUnlocked") == 1f)
		{
			this.images[0].sprite = this.achievements[0];
			this.images[0].color = Color.white;
		}
		else
		{
			this.images[0].sprite = this.achievements[0];
			this.images[0].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("StoryModeCompleted") == 1f)
		{
			this.images[1].sprite = this.achievements[1];
			this.images[1].color = Color.white;
		}
		else
		{
			this.images[1].sprite = this.achievements[1];
			this.images[1].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("Detention") == 1f)
		{
			this.images[2].sprite = this.achievements[2];
			this.images[2].color = Color.white;
		}
		else
		{
			this.images[2].sprite = this.achievements[2];
			this.images[2].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("KnifeSteamed") == 1f)
		{
			this.images[3].sprite = this.achievements[3];
			this.images[3].color = Color.white;
		}
		else
		{
			this.images[3].sprite = this.achievements[3];
			this.images[3].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("Erased") == 1f)
		{
			this.images[4].sprite = this.achievements[4];
			this.images[4].color = Color.white;
		}
		else
		{
			this.images[4].sprite = this.achievements[4];
			this.images[4].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("So Close!") == 1f)
		{
			this.images[5].sprite = this.achievements[5];
			this.images[5].color = Color.white;
		}
		else
		{
			this.images[5].sprite = this.achievements[5];
			this.images[5].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("Memes") == 1f)
		{
			this.images[6].sprite = this.achievements[6];
			this.images[6].color = Color.white;
		}
		else
		{
			this.images[6].sprite = this.achievements[6];
			this.images[6].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("Tunes") == 1f)
		{
			this.images[7].sprite = this.achievements[7];
			this.images[7].color = Color.white;
		}
		else
		{
			this.images[7].sprite = this.achievements[7];
			this.images[7].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("AllMemes") == 1f)
		{
			this.images[8].sprite = this.achievements[8];
			this.images[8].color = Color.white;
		}
		else
		{
			this.images[8].sprite = this.achievements[8];
			this.images[8].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("AllTunes") == 1f)
		{
			this.images[9].sprite = this.achievements[9];
			this.images[9].color = Color.white;
		}
		else
		{
			this.images[9].sprite = this.achievements[9];
			this.images[9].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("99") == 1f)
		{
			this.images[10].sprite = this.achievements[10];
			this.images[10].color = Color.white;
		}
		else
		{
			this.images[10].sprite = this.achievements[10];
			this.images[10].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("FarmCompleted") == 1f)
		{
			this.images[11].sprite = this.achievements[11];
			this.images[11].color = Color.white;
		}
		else
		{
			this.images[11].sprite = this.achievements[11];
			this.images[11].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("FlipperActive") == 1f)
		{
			this.images[12].sprite = this.achievements[12];
			this.images[12].color = Color.white;
		}
		else
		{
			this.images[12].sprite = this.achievements[12];
			this.images[12].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("LessThan200") == 1f)
		{
			this.images[13].sprite = this.achievements[13];
			this.images[13].color = Color.white;
		}
		else
		{
			this.images[13].sprite = this.achievements[13];
			this.images[13].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("LessThan175") == 1f)
		{
			this.images[14].sprite = this.achievements[14];
			this.images[14].color = Color.white;
		}
		else
		{
			this.images[14].sprite = this.achievements[14];
			this.images[14].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("LessThan150") == 1f)
		{
			this.images[15].sprite = this.achievements[15];
			this.images[15].color = Color.white;
		}
		else
		{
			this.images[15].sprite = this.achievements[15];
			this.images[15].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("Struck") == 1f)
		{
			this.images[16].sprite = this.achievements[16];
			this.images[16].color = Color.white;
		}
		else
		{
			this.images[16].sprite = this.achievements[16];
			this.images[16].color = Color.black;
		}
		if (PlayerPrefs.GetFloat("AllUnlocked") == 1f)
		{
			this.images[17].sprite = this.achievements[17];
			this.images[17].color = Color.white;
			return;
		}
		this.images[17].sprite = this.achievements[17];
		this.images[17].color = Color.black;
	}

	// Token: 0x0400000B RID: 11
	public Sprite[] achievements = new Sprite[18];

	// Token: 0x0400000C RID: 12
	public Image[] images = new Image[18];
}
