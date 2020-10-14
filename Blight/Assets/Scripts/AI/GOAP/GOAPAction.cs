using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script defines the variables that a GOAP actions should inherit.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
	public abstract class GOAPAction
	{
		/// <summary>
		/// The list of preconditions this action requires to function.
		/// </summary>
		public Dictionary<string, bool> Preconditions { get; private set; }

		/// <summary>
		/// The list of effects this action leads to when successfuly performed.
		/// </summary>
		public Dictionary<string, bool> Effects { get; private set; }

		/// <summary>
		/// The target to perform this action on. Can be null.
		/// </summary>
		public GameObject _target;

		/// <summary>
		/// Determines whether the AI Agent is in range to perform this action.
		/// </summary>
		private bool _inRange = false;

		/// <summary>
		/// The cost of this action. Changing it will affect what actions are chosen during planning.
		/// </summary>
		protected float _cost = 1f;

		/// <summary>
		/// The risk of performing the action.
		/// </summary>
		protected float _risk = 0f;

		/// <summary>
		/// The Benefits of performing the action.
		/// </summary>
		protected float _return = 1f;

		/// <summary>
		/// Constructor.
		/// </summary>
		public GOAPAction()
		{
			Preconditions = new Dictionary<string, bool>();
			Effects = new Dictionary<string, bool>();
		}

		/// <summary>
		/// Returns cost of the action.
		/// </summary>
		public virtual float GetCost()
		{
			return _cost;
		}

		/// <summary>
		/// Calculate a weight that suits the action.
		/// </summary>
		public virtual float GetWeight()
		{
			return (1 - _risk) * _return;
		}

		/// <summary>
		/// Resets any variables that need to be reset before planning happens again.
		/// </summary>
		public void DoReset()
		{
			_inRange = false;
			_target = null;
			DoSubReset();
		}

		/// <summary>
		/// Resets any variables from derrived class that need to be reset before planning happens again.
		/// </summary>
		public abstract void DoSubReset();

		/// <summary>
		/// Check if action is done.
		/// </summary>
		public abstract bool IsDone();

		/// <summary>
		/// Procedurally check if this action can run. Not always needed.
		/// </summary>
		/// <param name="agent">GOAPAgent</param>
		/// <param name="dataHolder">dynamic data about the world</param>
		public abstract bool CheckProceduralPrecondition(GameObject agent, DataStorage dataHolder);

		/// <summary>
		/// Performs the action. Returns True if the action performed successfully.
		/// In this case of False the action queue should clear out and the goal cannot be reached.
		/// </summary>
		/// <param name="agent">GOAPAgent</param>
		/// <param name="dataHolder">dynamic data about the world</param>
		public abstract bool Perform(GameObject agent, DataStorage dataHolder);

		/// <summary>
		/// Check if this action need to be within range of a target.
		/// If not then the moveToState is not needed for this action.
		/// </summary>
		public abstract bool RequiresInRange();

		/// <summary>
		/// Get inRange value.
		/// </summary>
		public bool IsInRange()
		{
			return _inRange;
		}

		/// <summary>
		/// Set inRange value.
		/// </summary>
		/// <param name="val">new inRange value</param>
		public void SetInRange(bool val)
		{
			_inRange = val;
		}

		/// <summary>
		/// Adds the given precondition.
		/// </summary>
		/// <param name="key">Precondition</param>
		/// <param name="value">True or False</param>
		public void AddPrecondition(string key, bool value)
		{
			Preconditions.Add(key, value);
		}

		/// <summary>
		/// Removes the given precondition.
		/// </summary>
		/// <param name="key">Precondition</param>
		public void RemovePrecondition(string key)
		{
			if (Preconditions.ContainsKey(key))
				Preconditions.Remove(key);
		}

		/// <summary>
		/// Adds the given effect.
		/// </summary>
		/// <param name="key">Effect</param>
		/// <param name="value">True or False</param>
		public void AddEffect(string key, bool value)
		{
			Effects.Add(key, value);
		}

		/// <summary>
		/// Removes the given effect.
		/// </summary>
		/// <param name="key">Effect</param>
		public void RemoveEffect(string key)
		{
			if (Effects.ContainsKey(key))
				Effects.Remove(key);
		}
	}
}