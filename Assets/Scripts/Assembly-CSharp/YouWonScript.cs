using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000051 RID: 81
public class YouWonScript : MonoBehaviour
{
	// Token: 0x06000199 RID: 409 RVA: 0x0000FC92 File Offset: 0x0000DE92
	private void Start()
	{
		this.delay = 10f;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
	private void Update()
	{
		if (PlayerPrefs.GetFloat("FreeRunUnlocked") == 0f)
		{
			this.aPopUp.QueueAchievement(this.aPopUp.achievements[0]);
			PlayerPrefs.SetFloat("FreeRunUnlocked", 1f);
		}
		if (PlayerPrefs.GetFloat("StoryModeCompleted") == 0f)
		{
			this.aPopUp.QueueAchievement(this.aPopUp.achievements[1]);
			PlayerPrefs.SetFloat("StoryModeCompleted", 1f);
		}
		this.delay -= Time.deltaTime;
		if (this.delay <= 0f)
		{
			SceneManager.LoadScene(7);
		}
	}

	// Token: 0x04000349 RID: 841
	private float delay;

	// Token: 0x0400034A RID: 842
	public AchievementPopUpScript aPopUp;
}
