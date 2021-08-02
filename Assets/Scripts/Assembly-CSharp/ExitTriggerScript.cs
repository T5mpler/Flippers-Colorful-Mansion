using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000022 RID: 34
public class ExitTriggerScript : MonoBehaviour
{
	// Token: 0x0600008A RID: 138 RVA: 0x00004C60 File Offset: 0x00002E60
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.notebooks >= 12 & other.tag == "Player")
		{
			if (this.gc.failedNotebooks >= 12)
			{
				if (gc.currentChallenge != ChallengeManagerScript.ChallengeType.None)
				{
					gc.UnlockMouse();
					SceneManager.LoadScene("ChallengeComplete");
				}
				else
				{
					SceneManager.LoadScene("Secret");
				}
			}
			else
			{
				if (gc.currentChallenge != ChallengeManagerScript.ChallengeType.None)
				{
					gc.UnlockMouse();
					SceneManager.LoadScene("ChallengeComplete");
				}
				else
				{
					SceneManager.LoadScene("Results");
				}
			}
		}
	}

	// Token: 0x040000C8 RID: 200
	public GameControllerScript gc;
}