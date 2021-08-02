using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000047 RID: 71
public class StartButton : MonoBehaviour
{
	// Token: 0x06000166 RID: 358 RVA: 0x0000E993 File Offset: 0x0000CB93
	private void Start()
	{
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this.StartGame));
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
	private void StartGame()
	{
		if (base.name != "FarmModeButton" && name != "Race")
		{
			if (base.name == "StoryButton")
			{
				PlayerPrefs.SetString("CurrentMode", "story");
				PlayerPrefs.SetString("ChallengeMode", string.Empty);
			}
			else if (base.name == "EndlessButton")
			{
				PlayerPrefs.SetString("CurrentMode", "endless");
				PlayerPrefs.SetString("ChallengeMode", string.Empty);
			}
			else if (base.name == "FreeRunButton")
			{
				PlayerPrefs.SetString("CurrentMode", "freeRun");
				PlayerPrefs.SetString("ChallengeMode", string.Empty);
			}
			else if (CompareTag("ChallengeButton") && name != "Race")
			{
				PlayerPrefs.SetString("ChallengeMode", name);
				PlayerPrefs.SetString("CurrentMode", "story");
				lss.LoadScene(2);
				return;
			}
			if (name != "Race")
			{
				this.lss.LoadScene(2);
			}
		}
		else if (name == "FarmModeButton")
		{
			PlayerPrefs.SetString("CurrentMode", "farmMode");
			PlayerPrefs.SetString("ChallengeMode", string.Empty);
			this.lss.LoadScene(6);
		}
		else if (name == "Race")
		{
			lss.LoadScene(8);
			PlayerPrefs.SetString("ChallengeMode", string.Empty);
		}
	}


	// Token: 0x040002E8 RID: 744
	private Button button;

	// Token: 0x040002E9 RID: 745
	public LoadingScreenScript lss;
}
