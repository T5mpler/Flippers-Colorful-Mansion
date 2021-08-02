using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000035 RID: 53
public class MathGameScript : MonoBehaviour
{
	// Token: 0x06000101 RID: 257 RVA: 0x000098A8 File Offset: 0x00007AA8
	private void Start()
	{
		this.playerAnswer.characterValidation = InputField.CharacterValidation.None;
		this.playerAnswer.ActivateInputField();
		this.gc.ActivateLearningGame();
		if (this.gc.notebooks == 1)
		{
			this.QueueAudio(this.bal_intro);
			this.QueueAudio(this.bal_howto);
		}
		this.NewProblem();
		if (this.gc.spoopMode)
		{
			this.baldiFeedTransform.position = new Vector3(-1000f, -1000f, 0f);
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00009930 File Offset: 0x00007B30
	private void Update()
	{
		if (!this.baldiAudio.isPlaying)
		{
			if (this.audioInQueue > 0 & !this.gc.spoopMode)
			{
				this.PlayQueue();
			}
			this.baldiFeed.SetBool("talking", false);
		}
		else
		{
			this.baldiFeed.SetBool("talking", true);
		}
		if ((Input.GetKeyDown("return") || Input.GetKeyDown("enter")) & this.questionInProgress)
		{
			this.questionInProgress = false;
			this.CheckAnswer();
		}
		if (this.problem > 3)
		{
			this.endDelay -= 1f * Time.unscaledDeltaTime;
			if (this.endDelay <= 0f)
			{
				this.ExitGame();
			}
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x000099F4 File Offset: 0x00007BF4
	private void NewProblem()
	{
		this.playerAnswer.text = string.Empty;
		this.problem++;
		if (this.problem <= 3)
		{
			this.QueueAudio(this.bal_problems[this.problem - 1]);
			if ((this.gc.mode == "story" & (this.problem <= 2 || this.gc.notebooks <= 1)) || (this.gc.mode == "endless" & (this.problem <= 2 || this.gc.notebooks != 2)) || (this.gc.mode == "freeRun" & (this.problem <= 2 || this.gc.notebooks <= 1)))
			{
				this.num1 = (float)Mathf.RoundToInt(Random.Range(0f, 9f));
				this.num2 = (float)Mathf.RoundToInt(Random.Range(0f, 9f));
				this.sign = Mathf.RoundToInt(Random.Range(4f, 4f));
				if (this.sign == this.sameSign)
				{
					this.sign = Mathf.RoundToInt(Random.Range(0f, 20f));
				}
				this.sameSign = this.sign;
				if (this.num1 == this.sameNum1)
				{
					this.num1 = (float)Mathf.RoundToInt(Random.Range(0f, 9f));
				}
				this.sameNum1 = this.num1;
				if (this.num2 == this.sameNum2)
				{
					this.num2 = (float)Mathf.RoundToInt(Random.Range(0f, 9f));
				}
				this.sameNum2 = this.num2;
				if ((this.solutionQuestion && this.sign != 17) || this.sign != 18 || this.sign != 19 || this.sign != 20)
				{
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
				}
				if (this.sign == 17 || this.sign == 18 || this.sign == 19 || this.sign == 20)
				{
					this.x1 = (float)Mathf.RoundToInt(Random.Range(-25f, 25f));
					this.x2 = (float)Mathf.RoundToInt(Random.Range(-25f, 25f));
					this.y1 = (float)Mathf.RoundToInt(Random.Range(-25f, 25f));
					this.y2 = (float)Mathf.RoundToInt(Random.Range(-25f, 25f));
				}
				if (this.sign == 0)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 + this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n \n",
						this.num1,
						"+",
						this.num2,
						"="
					});
					this.QueueAudio(this.bal_plus);
				}
				else if (this.sign == 1)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 - this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n \n",
						this.num1,
						"-",
						this.num2,
						"="
					});
					this.QueueAudio(this.bal_minus);
				}
				else if (this.sign == 2)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt(Mathf.Sqrt(this.num1));
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What's the square root of\n",
						this.num1,
						"\nIf irrational, round"
					});
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 3)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt(Mathf.Sqrt(this.num1 + this.num2));
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What's the square root of\n",
						this.num1,
						"+",
						this.num2,
						"\nIf irrational, round"
					});
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 4)
				{
					this.solutionQuestion = true;
					this.solution = (float)((int)Mathf.Sqrt(Mathf.Abs(this.num1 - this.num2)));
					this.questionText.fontSize = 15;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What's the square root of\n",
						this.num1,
						"-",
						this.num2,
						"\nIf irrational, then round. If negititve make your answer positive"
					});
					this.QueueAudio(this.bal_minus);
				}
				else if (this.sign == 5)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt(Mathf.Sqrt(this.num1 * this.num2));
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n ",
						"What's the square root of\n",
						this.num1,
						"*",
						this.num2,
						"\nIf irrational, round \n"
					});
					this.QueueAudio(this.bal_minus);
				}
				else if (this.sign == 6)
				{
					this.solutionQuestion = false;
					this.triviaSolution = "A";
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"This is the 300th WIP, right?",
						"                                      A) True          B) False;"
					});
				}
				else if (this.sign == 7)
				{
					this.solutionQuestion = false;
					this.triviaSolution = "C";
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"Who made the first Baldi modding guide?",
						"                                      A) Purple Ghost          B) TheBaldiModder452",
						"                                 C) ThatEpicJake72       D) Mystman12              "
					});
				}
				else if (this.sign == 8)
				{
					this.solutionQuestion = false;
					this.triviaSolution = "B";
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What engine is Baldi's Basics made from?",
						"                                      A)  Unreal Engine 4          B) Unity",
						"                                 C) No Engine       D) GameMaker              "
					});
				}
				else if (this.sign == 9)
				{
					this.solutionQuestion = false;
					this.triviaSolution = "AC";
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What's the usual application used for coding Baldi?",
						"                                      A) DnSpy          B) MS Paint",
						"                                 C) Microsoft Visual Studio     D) Nothing  "
					});
				}
				else if (this.sign == 10)
				{
					this.solutionQuestion = false;
					this.triviaSolution = "A";
					this.questionText.fontSize = 18;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"What version of Baldi did the game start to get more 'attention'?",
						"                                      A) 1.2.2          B) 1.3.2",
						"                                           C) 1.4     D) 1.4.1  "
					});
				}
				else if (this.sign == 11)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 + this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²+",
						this.num2,
						"="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_plus);
				}
				else if (this.sign == 12)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 - this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²-",
						this.num2,
						"="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_minus);
				}
				else if (this.sign == 13)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 * this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²*",
						this.num2,
						"="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 14)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 + this.num2 * this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²+",
						this.num2,
						"²="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_plus);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num2)]);
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 15)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 - this.num2 * this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²-",
						this.num2,
						"²="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_minus);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num2)]);
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 16)
				{
					this.solutionQuestion = true;
					this.solution = this.num1 * this.num1 * this.num2 * this.num2;
					this.questionText.text = string.Concat(new object[]
					{
						"Solve this Q",
						this.problem,
						": \n \n",
						this.num1,
						"²*",
						this.num2,
						"²="
					});
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num1)]);
					this.QueueAudio(this.bal_times);
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num2)]);
					this.QueueAudio(this.bal_times);
				}
				else if (this.sign == 17)
				{
					this.solutionQuestion = true;
					this.solution = Mathf.Round((this.y2 - this.y1) / (this.x2 - this.x1));
					this.demoninator = this.x2 - this.x1;
					if (this.demoninator == 0f)
					{
						this.solution = 0f;
					}
					this.questionText.fontSize = 15;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"Find the slope with the given coordinates\n",
						"Round if necessary",
						"(",
						this.x1,
						",",
						this.y1,
						") ",
						"(",
						this.x2,
						",",
						this.y2,
						") "
					});
				}
				else if (this.sign == 18)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt((this.y2 - this.y1) / (this.x2 - this.x1));
					this.demoninator = this.x2 - this.x1;
					if (this.demoninator == 0f)
					{
						this.solution = 0f;
					}
					this.questionText.fontSize = 15;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"Find the slope with the given coordinates\n",
						"Round if necessary",
						"(",
						this.x1,
						",",
						this.y1,
						") ",
						"(",
						this.x2,
						",",
						this.y2,
						") "
					});
				}
				else if (this.sign == 19)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt((this.y2 - this.y1) / (this.x2 - this.x1));
					this.demoninator = this.x2 - this.x1;
					if (this.demoninator == 0f)
					{
						this.solution = 0f;
					}
					this.questionText.fontSize = 15;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"Find the slope with the given coordinates\n",
						"Round if necessary",
						"(",
						this.x1,
						",",
						this.y1,
						") ",
						"(",
						this.x2,
						",",
						this.y2,
						") "
					});
				}
				else if (this.sign == 20)
				{
					this.solutionQuestion = true;
					this.solution = (float)Mathf.RoundToInt((this.y2 - this.y1) / (this.x2 - this.x1));
					this.demoninator = this.x2 - this.x1;
					if (this.demoninator == 0f)
					{
						this.solution = 0f;
					}
					this.questionText.fontSize = 15;
					this.questionText.text = string.Concat(new object[]
					{
						"Problem ",
						this.problem,
						": \n",
						"Find the slope with the given coordinates\n",
						"Round if necessary",
						"(",
						this.x1,
						",",
						this.y1,
						") ",
						"(",
						this.x2,
						",",
						this.y2,
						") "
					});
				}
				if (this.sign != 17 || this.sign != 18 || this.sign != 19 || this.sign != 20)
				{
					this.QueueAudio(this.bal_numbers[Mathf.RoundToInt(this.num2)]);
					this.QueueAudio(this.bal_equals);
				}
			}
			else
			{
				this.impossibleMode = true;
				this.num1 = Random.Range(1f, 9999f);
				this.num2 = Random.Range(1f, 9999f);
				this.num3 = Random.Range(1f, 9999f);
				this.sign = Mathf.RoundToInt((float)Random.Range(0, 1));
				this.QueueAudio(this.bal_screech);
				if (this.sign == 0)
				{
					this.questionText.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n",
						"HAHAAAHAAHHA YOUR BSDODFDK DEADDSK HIRNKFNKFDFD"
					});
					this.QueueAudio(this.bal_screech);
				}
				else if (this.sign == 1)
				{
					this.questionText.color = Color.blue;
					this.questionText.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n",
						"   HAHAAAHAAHHA YOUR BSDODFDK DEADDSK HIRNKFNKFDFD"
					});
					this.QueueAudio(this.bal_screech);
				}
				this.num1 = Random.Range(1f, 9999f);
				this.num2 = Random.Range(1f, 9999f);
				this.num3 = Random.Range(1f, 9999f);
				this.sign = Mathf.RoundToInt((float)Random.Range(0, 1));
				if (this.sign == 0)
				{
					this.questionText2.color = Color.yellow;
					this.questionText2.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n",
						"   HAHAAAHAAHHA YOUR BSDODFDK DEADDSK HIRNKFNKFDFD"
					});
				}
				else if (this.sign == 1)
				{
					this.questionText2.color = Color.grey;
					this.questionText2.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n",
						"   HAHAAAHAAHHA YOUR BSDODFDK DEADDSK HIRNKFNKFDFD"
					});
				}
				this.num1 = Random.Range(1f, 9999f);
				this.num2 = Random.Range(1f, 9999f);
				this.num3 = Random.Range(1f, 9999f);
				this.sign = Mathf.RoundToInt((float)Random.Range(0, 1));
				if (this.sign == 0)
				{
					this.questionText3.color = Color.blue;
					this.questionText3.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n",
						" HAHAAAHAAHHA YOUR BSDODFDK DEADDSK HIRNKFNKFDFD"
					});
				}
				else if (this.sign == 1)
				{
					this.questionText3.color = Color.red;
					this.questionText3.text = string.Concat(new object[]
					{
						"SOLVE MATH Q",
						this.problem,
						": \n (",
						this.num1,
						"/",
						this.num2,
						")+",
						this.num3,
						"="
					});
				}
				this.QueueAudio(this.bal_equals);
			}
			this.playerAnswer.ActivateInputField();
			this.questionInProgress = true;
			return;
		}
		this.endDelay = 5f;
		if (!this.gc.spoopMode)
		{
			this.questionText.fontSize = 30;
			this.questionText.text = "Hmmmm.....\n That's actually not that bad";
			return;
		}
		if (this.gc.mode == "endless" & this.problemsWrong <= 0)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 1f));
			this.questionText.text = this.endlessHintText[num];
			base.StartCoroutine(this.TypeSentence(this.endlessHintText[num]));
			questionText.color = Color.green;
			this.questionText.fontSize = 30;
			return;
		}
		if (this.gc.mode == "story" & this.problemsWrong >= 3)
		{
			this.questionText.text = ">>:::::((";
			StartCoroutine(TypeSentence(secretHintText[UnityEngine.Random.Range(0, secretHintText.Length)]));
			this.questionText.fontSize = 30;
			this.questionText2.text = string.Empty;
			this.questionText3.text = string.Empty;
			this.baldiScript.Hear(this.playerPosition, 10f);
			this.gc.failedNotebooks++;
			return;
		}
		int num2 = Mathf.RoundToInt(Random.Range(0f, 4f));
		this.questionText.text = this.hintText[num2];
		base.StartCoroutine(this.TypeSentence(this.hintText[num2]));
		this.questionText.fontSize = 30;
		if (num2 == 0)
		{
			this.questionText.fontSize = 25;
		}
		this.questionText2.text = string.Empty;
		this.questionText3.text = string.Empty;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0000B21C File Offset: 0x0000941C
	public void CheckAnswer()
	{
		if (this.playerAnswer.text == "Give me memes")
		{
			base.StartCoroutine(this.CheatText("You wanted memes, I'll give you memes"));
			this.gc.CollectItem(16);
		}
		if (this.playerAnswer.text == "Tunes")
		{
			base.StartCoroutine(this.CheatText("Some tunes...."));
			this.gc.CollectItem(17);
		}
		if (this.playerAnswer.text == "Upside Down")
		{
			base.StartCoroutine(this.CheatText("Oh noes you got upside now"));
			this.baldiScript.upSideDown = true;
		}
		else if (this.playerAnswer.text == "Uh noes...")
		{
			base.StartCoroutine(this.CheatText(""));
			RenderSettings.ambientLight = Color.red;
		}
		else if (this.playerAnswer.text == "Flipper is happy")
		{
			base.StartCoroutine(this.CheatText("this isn't what you think"));
			this.baldiScript.timeToMove = 0.01f;
			this.baldiScript.flipperHappy = true;
		}
		else if (this.playerAnswer.text == "Radar")
		{
			base.StartCoroutine(this.CheatText("Time to stalk people with my little handy dandy item!"));
			this.gc.CollectItem(18);
		}
		if (this.playerAnswer.text == this.solution.ToString() & !this.impossibleMode & this.solutionQuestion)
		{
			this.results[this.problem - 1].texture = this.correct;
			this.baldiAudio.Stop();
			this.ClearAudioQueue();
			int num = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.QueueAudio(this.bal_praises[num]);
			this.NewProblem();
			return;
		}
		if (this.playerAnswer.text == this.triviaSolution & !this.impossibleMode & !this.solutionQuestion)
		{
			this.results[this.problem - 1].texture = this.correct;
			this.baldiAudio.Stop();
			this.ClearAudioQueue();
			int num2 = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.QueueAudio(this.bal_praises[num2]);
			this.NewProblem();
			return;
		}
		this.problemsWrong++;
		this.results[this.problem - 1].texture = this.incorrect;
		if (!this.gc.spoopMode)
		{
			this.baldiFeed.SetTrigger("angry");
			this.gc.ActivateSpoopMode();
		}
		if (this.gc.mode == "story" || this.gc.mode == "freeRun")
		{
			if (this.problem == 3)
			{
				this.baldiScript.GetAngry(1f);
			}
			else
			{
				this.baldiScript.GetTempAngry(0.25f);
			}
		}
		else if (gc.mode == "endless")
		{
			this.baldiScript.GetAngry(1f);
		}
		this.ClearAudioQueue();
		this.baldiAudio.Stop();
		this.NewProblem();
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0000B540 File Offset: 0x00009740
	private void QueueAudio(AudioClip sound)
	{
		this.audioQueue[this.audioInQueue] = sound;
		this.audioInQueue++;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x0000B55E File Offset: 0x0000975E
	private void PlayQueue()
	{
		this.baldiAudio.PlayOneShot(this.audioQueue[0]);
		this.UnqueueAudio();
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0000B57C File Offset: 0x0000977C
	private void UnqueueAudio()
	{
		for (int i = 1; i < this.audioInQueue; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue--;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000B5BA File Offset: 0x000097BA
	private void ClearAudioQueue()
	{
		this.audioInQueue = 0;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000B5C4 File Offset: 0x000097C4
	private void ExitGame()
	{
		if (this.problemsWrong <= 0 & this.gc.mode == "endless")
		{
			this.baldiScript.GetAngry(-1f);
		}
		this.gc.DeactivateLearningGame(base.gameObject);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000B616 File Offset: 0x00009816
	private IEnumerator TypeSentence(string sentence)
	{
		this.baldiAudio.clip = this.typingSound;
		this.questionText.text = string.Empty;
		foreach (char c in sentence.ToCharArray())
		{
			Text text = this.questionText;
			text.text += c.ToString();
			this.baldiAudio.volume = 0.15f;
			this.baldiAudio.PlayOneShot(this.typingSound);
			yield return null;
		}
		baldiAudio.volume = 1f;
		yield break;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000B62C File Offset: 0x0000982C
	private IEnumerator CheatText(string text)
	{
		for (;;)
		{
			this.questionText.text = text;
			this.questionText2.text = string.Empty;
			this.questionText3.text = string.Empty;
			yield return new WaitForEndOfFrame();
		}
	}

	// Token: 0x04000208 RID: 520
	public GameControllerScript gc;

	// Token: 0x04000209 RID: 521
	public BaldiScript baldiScript;

	// Token: 0x0400020A RID: 522
	public Vector3 playerPosition;

	// Token: 0x0400020B RID: 523
	public GameObject mathGame;

	// Token: 0x0400020C RID: 524
	public RawImage[] results = new RawImage[3];

	// Token: 0x0400020D RID: 525
	public Texture correct;

	// Token: 0x0400020E RID: 526
	public Texture incorrect;

	// Token: 0x0400020F RID: 527
	public InputField playerAnswer;

	// Token: 0x04000210 RID: 528
	public Text questionText;

	// Token: 0x04000211 RID: 529
	public Text questionText2;

	// Token: 0x04000212 RID: 530
	public Text questionText3;

	// Token: 0x04000213 RID: 531
	public Animator baldiFeed;

	// Token: 0x04000214 RID: 532
	public Transform baldiFeedTransform;

	// Token: 0x04000215 RID: 533
	public AudioClip bal_plus;

	// Token: 0x04000216 RID: 534
	public AudioClip bal_minus;

	// Token: 0x04000217 RID: 535
	public AudioClip bal_times;

	// Token: 0x04000218 RID: 536
	public AudioClip bal_divided;

	// Token: 0x04000219 RID: 537
	public AudioClip bal_equals;

	// Token: 0x0400021A RID: 538
	public AudioClip bal_howto;

	// Token: 0x0400021B RID: 539
	public AudioClip bal_intro;

	// Token: 0x0400021C RID: 540
	public AudioClip bal_screech;

	// Token: 0x0400021D RID: 541
	public AudioClip[] bal_numbers = new AudioClip[10];

	// Token: 0x0400021E RID: 542
	public AudioClip[] bal_praises = new AudioClip[5];

	// Token: 0x0400021F RID: 543
	public AudioClip[] bal_problems = new AudioClip[3];

	// Token: 0x04000220 RID: 544
	private float endDelay;

	// Token: 0x04000221 RID: 545
	private int problem;

	// Token: 0x04000222 RID: 546
	private int audioInQueue;

	// Token: 0x04000223 RID: 547
	private float num1;

	// Token: 0x04000224 RID: 548
	private float num2;

	// Token: 0x04000225 RID: 549
	private float num3;

	// Token: 0x04000226 RID: 550
	private int sign;

	// Token: 0x04000227 RID: 551
	private float solution;

	// Token: 0x04000228 RID: 552
	private string[] hintText = new string[]
	{
		"If you don't understand right now, I'M ANGRY WITH YOU PLAYER",
		"Oh this will be fun :) Well for me at least :P",
		"MY EARS HAVE MORE CAPABILITY THAN YOU THINK",
		"EVERY WRONG PROBLEM PUTS ANGRISM ON ME",
		"I'LL HAVE YOU FOR DINNER WHEN I FIND YOU"
	};

	// Token: 0x04000229 RID: 553
	private string[] endlessHintText = new string[]
	{
		"IDC",
		"I still want you for dinner when I get to you"
	};
	string[] secretHintText = new string[]
	{
		"This isn't over",
		">:((((",
		"bruh moment, YA REALLY THINK THIS IS A GOOD IDEA?"
	};

	// Token: 0x0400022A RID: 554
	private bool questionInProgress;

	// Token: 0x0400022B RID: 555
	private bool impossibleMode;

	// Token: 0x0400022C RID: 556
	private int problemsWrong;

	// Token: 0x0400022D RID: 557
	private AudioClip[] audioQueue = new AudioClip[20];

	// Token: 0x0400022E RID: 558
	public AudioSource baldiAudio;

	// Token: 0x0400022F RID: 559
	private int sameSign;

	// Token: 0x04000230 RID: 560
	public string triviaSolution;

	// Token: 0x04000231 RID: 561
	public bool solutionQuestion;

	// Token: 0x04000232 RID: 562
	private float sameNum1;

	// Token: 0x04000233 RID: 563
	public float sameNum2;

	// Token: 0x04000234 RID: 564
	private float x1;

	// Token: 0x04000235 RID: 565
	private float x2;

	// Token: 0x04000236 RID: 566
	private float y1;

	// Token: 0x04000237 RID: 567
	private float y2;

	// Token: 0x04000238 RID: 568
	private float demoninator;

	// Token: 0x04000239 RID: 569
	public AudioClip typingSound;
}
