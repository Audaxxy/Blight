using System;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script controls each AI agent and defines FSM and GOAP planners for each of them. Always add to any NPC.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
	public class GOAPAgent : MonoBehaviour, IGOAPAgent
	{
		/// <summary>
		/// Represents Finite State Machine.
		/// </summary>
		private FSM _finiteStateMachine;



		/// <summary>
		/// Represents Idle State of Finite State Machine, which finds something to do.
		/// </summary>
		private FSM.FSMState _idleState;

		/// <summary>
		/// Represents MoveTo State of Finite State Machine, which moves to a target.
		/// </summary>
		private FSM.FSMState _moveToState;

		/// <summary>
		/// Represents MoveTo State of Finite State Machine, which performs an action.
		/// </summary>
		private FSM.FSMState _performActionState;



		/// <summary>
		/// Represents all actions available for the plan.
		/// </summary>
		private HashSet<GOAPAction> _availableActions;

		/// <summary>
		/// Represents a queue of actions being performed.
		/// </summary>
		private Queue<GOAPAction> _currentActions;



		/// <summary>
		/// Represents an action planner, which generates a queue of actions to achieve the goal.
		/// </summary>
		private GOAPPlanner _planner;



		/// <summary>
		/// This is the implementing class that provides our world data and listens to feedback on planning.
		/// </summary>
		private IAIAgent _AIAgent;



		/// <summary>
		/// Use this for initialization.
		/// </summary>
		void Start()
		{
			_finiteStateMachine = new FSM();
			_availableActions = new HashSet<GOAPAction>();
			_currentActions = new Queue<GOAPAction>();
			_planner = new GOAPPlanner();

			FindAndLinkAIAgent();

			CreateIdleState();
			CreateMoveToState();
			CreatePerformActionState();

			_finiteStateMachine.PushState(_idleState);

			LoadActions();
		}

		/// <summary>
		/// Update is called once per frame.
		/// </summary>
		void Update()
		{
			_finiteStateMachine.Update(this.gameObject);
		}

		#region Interface

		public void AddAction(GOAPAction action)
		{
			_availableActions.Add(action);
		}

		public GOAPAction GetAction(Type action)
		{
			foreach (GOAPAction currentAction in _availableActions)
			{
				if (currentAction.GetType().Equals(action))
				{
					return currentAction;
				}
			}

			return null;
		}

		public void RemoveAction(GOAPAction action)
		{
			_availableActions.Remove(action);
		}

		public void AbortFSM()
		{
			_finiteStateMachine.ClearState();
			_finiteStateMachine.PushState(_idleState);
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Determines whether the GOAPAgent has an action plan for the AI Agent.
		/// </summary>
		private bool HasActionPlan()
		{
			return _currentActions.Count > 0;
		}

		#endregion

		#region Create States

		/// <summary>
		/// Creates idleState for the FSM. This state is used to find something to do for the AI Agent.
		/// </summary>
		private void CreateIdleState()
		{
			_idleState = (fsm, obj) =>
			{

				Dictionary<string, bool> worldState = _AIAgent.CreateWorldState();
				Dictionary<string, bool> goals = _AIAgent.CreateGoalState();

				Queue<GOAPAction> plan = null;
				KeyValuePair<string, bool> lastGoal = new KeyValuePair<string, bool>();
				foreach (KeyValuePair<string, bool> goal in goals)
				{
					// Use first achievable plan
					lastGoal = goal;
					plan = _planner.Plan(gameObject, _availableActions, worldState, goal, _AIAgent);
					if (plan != null)
						break;
				}

				if (plan != null)
				{
					_currentActions = plan;
					_AIAgent.PlanFound(lastGoal, plan);

					fsm.PopState();
					fsm.PushState(_performActionState);
				}
				else
				{
					_AIAgent.PlanFailed(goals);

					fsm.ClearState();
					fsm.PushState(_idleState);
				}
			};
		}

		/// <summary>
		/// Creates moveToState for the FSM. This state is used to move the Ai Agent to its destination.
		/// </summary>
		private void CreateMoveToState()
		{
			_moveToState = (fsm, gameObject) =>
			{

				GOAPAction action = _currentActions.Peek();

				if (action.RequiresInRange() && action._target == null)
				{
					fsm.ClearState();
					fsm.PushState(_idleState);
					return;
				}

				if (_AIAgent.MoveAgent(action))
				{
					fsm.PopState();
				}

			};
		}

		/// <summary>
		/// Creates performActionState for the FSM. This state is used to make the AI Agent perform an action.
		/// </summary>
		private void CreatePerformActionState()
		{

			_performActionState = (fsm, obj) =>
			{

				if (!HasActionPlan())
				{
					_AIAgent.ActionsFinished();

					fsm.ClearState();
					fsm.PushState(_idleState);
					return;
				}

				GOAPAction action = _currentActions.Peek();
				if (action.IsDone())
				{
					// If action is done, remove it to perform the next one.
					_currentActions.Dequeue();
				}

				if (HasActionPlan())
				{
					action = _currentActions.Peek();
					bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

					if (inRange)
					{
						bool success = action.Perform(obj, _AIAgent.DataHolder);
						if (!success)
						{
							_AIAgent.PlanAborted(action);

							fsm.ClearState();
							fsm.PushState(_idleState);
						}
					}
					else
					{
						fsm.PushState(_moveToState);
					}
				}
				else
				{
					_AIAgent.ActionsFinished();

					fsm.ClearState();
					fsm.PushState(_idleState);
				}
			};
		}

		#endregion

		#region Initializations

		/// <summary>
		/// Finds the AIAgent component and links it together with itself.
		/// </summary>
		private void FindAndLinkAIAgent()
		{
			if (this.gameObject.TryGetComponent(out _AIAgent))
            {
				_AIAgent.GOAPAgent = this;
			}
		}

		/// <summary>
		/// Loads available actions.
		/// </summary>
		private void LoadActions()
		{
			_availableActions = _AIAgent.CreateActionSet();
		}

		#endregion
	}
}