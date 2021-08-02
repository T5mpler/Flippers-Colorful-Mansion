using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class MoveModifier : MonoBehaviour
{
	public MoveModifier(float multipler, Vector3 adder, float timer = float.PositiveInfinity)
	{
		this.multipler = multipler;
		this.adder = adder;
		this.timer = timer;
	}
	private void Update()
	{
		if (tick) TimerCountdown();
	}
	void TimerCountdown()
	{
		if (!float.IsPositiveInfinity(timer))
		{
			if (timer > 0f)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				ActivityModifierScript[] activityModifier = FindObjectsOfType<ActivityModifierScript>();
				foreach (ActivityModifierScript activity in activityModifier)
				{
					activity.movementModList.Remove(this);
					OnMoveModRemoved?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}
	public float timer;
	public float multipler;
	public Vector3 adder;
	[HideInInspector] public bool tick = true;
	public event EventHandler OnMoveModRemoved;
}
