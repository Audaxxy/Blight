using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script chooses which actions to use, when to use them and chooses a path of actions to reach the final goal.
// Basiclly plans what actions can be completed in order to fulfill a goal state.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
	public class GOAPPlanner
	{
		/// <summary>
		/// Plans what sequence of actions can fulfill the goal.
		/// Returns null if a plan could not be found, or a list of the actions that must be performed, in order, to fulfill the goal.
		/// </summary>
		/// <param name="agent">GOAPAgent</param>
		/// <param name="availableActions">Actions that can be performed</param>
		/// <param name="worldState">Starting world state</param>
		/// <param name="goal">Goal that GOAPAgent is trying to achieve</param>
		/// <param name="AIAgent">Implementing class that provides our world data and listens to feedback on planning</param>
		public Queue<GOAPAction> Plan(GameObject agent,
									  HashSet<GOAPAction> availableActions,
									  Dictionary<string, bool> worldState,
									  KeyValuePair<string, bool> goal,
									  IAIAgent AIAgent)
		{
			// Reset all of the actions that can be performed.
			foreach (GOAPAction action in availableActions)
			{
				action.DoReset();
			}

			// Loops through the available actions and adds the ones that can be performed to a list.
			HashSet<GOAPAction> usableActions = GOAPPlannerHelper.GetFreeActionSet();
			foreach (GOAPAction action in availableActions)
			{
				// If the the action can be performed.
				if (action.CheckProceduralPrecondition(agent, AIAgent.DataHolder))
					usableActions.Add(action);
			}

			// Create a "tree" of actions that will lead to the wanted goal.
			List<GOAPNode> leaves = new List<GOAPNode>();
			GOAPNode start = GOAPPlannerHelper.GetFreeNode(null, 0, 0, worldState, null);
			bool success = BuildGraph(start, leaves, usableActions, goal);
			if (!success)
			{
				return null;
			}

			// Find the cheapest plan of action out of each generated plan.
			GOAPNode cheapest = null;
			foreach (GOAPNode leaf in leaves)
			{
				if (cheapest == null)
					cheapest = leaf;
				else
				{
					if (leaf.BetterThen(cheapest))
						cheapest = leaf;
				}
			}

			// Get its node and work back through the parents.
			List<GOAPAction> result = new List<GOAPAction>();
			GOAPNode n = cheapest;
			while (n != null)
			{
				if (n._action != null)
				{
					// Insert the action in the front.
					result.Insert(0, n._action);
				}
				n = n._parent;
			}

			// Clean up after we're done.
			GOAPPlannerHelper.Release();

			// Create a final plan to follow.
			Queue<GOAPAction> finalQueue = new Queue<GOAPAction>();
			foreach (GOAPAction action in result)
			{
				finalQueue.Enqueue(action);
			}

			return finalQueue;

		}

		/// <summary>
		/// Builds the plan of actions. Returns true if at least one solution was found.
		/// The possible paths are stored in the leaves list. Each leaf has a 'runningCost' value where the lowest cost will be the best action sequence.
		/// </summary>
		/// <param name="parent">Parent node</param>
		/// <param name="leaves">List of nodes with possible paths</param>
		/// <param name="usableActions">Actions available to the GOAPAgent</param>
		/// <param name="goal">Goal that GOAPAgent is trying to achieve</param>
		private bool BuildGraph(GOAPNode parent, List<GOAPNode> leaves, HashSet<GOAPAction> usableActions, KeyValuePair<string, bool> goal)
		{
			bool foundOne = false;

			// Go through each action available at this node and see if we can use it here.
			foreach (GOAPAction action in usableActions)
			{
				// If the parent state has the conditions for this action's preconditions, we can use it here.
				if (InState(action.Preconditions, parent._state))
				{
					// Apply the action's effects to the parent state.
					Dictionary<string, bool> currentState = PopulateState(parent._state, action.Effects);
					GOAPNode node = GOAPPlannerHelper.GetFreeNode(parent, parent._runningCost + action.GetCost(), parent._weight + action.GetWeight(), currentState, action);

					// If there's no match between child's precondition and parent's effects or child's precondition is empty.
					if (parent._action != null && (action.Preconditions.Count == 0 || !CondRelation(action.Preconditions, parent._action.Effects)))
						continue;

					if (GoalInState(goal, currentState))
					{
						// There is a solution.
						leaves.Add(node);
						foundOne = true;
					}
					else
					{
						// Not at a solution yet, so we're testing all of the remaining actions and branching out the tree.
						HashSet<GOAPAction> subset = ActionSubset(usableActions, action);
						bool found = BuildGraph(node, leaves, subset, goal);
						if (found)
							foundOne = true;
					}
				}
			}

			return foundOne;
		}

		/// <summary>
		/// Checks if there's match between child's precondition and parent's effects.
		/// </summary>
		/// <param name="preconditions">Child's precondition</param>
		/// <param name="effects">Parent's effects</param>
		private bool CondRelation(Dictionary<string, bool> preconditions, Dictionary<string, bool> effects)
		{
			foreach (KeyValuePair<string, bool> t in preconditions)
			{
				bool match = effects.ContainsKey(t.Key) && effects[t.Key] == t.Value;
				if (match)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a subset of the actions excluding the actionToRemove. Creates a new set.
		/// </summary>
		/// <param name="actions">Usuable actions</param>
		/// <param name="actionToRemove">Action to exclude from this branch</param>
		private HashSet<GOAPAction> ActionSubset(HashSet<GOAPAction> actions, GOAPAction actionToRemove)
		{
			HashSet<GOAPAction> subset = GOAPPlannerHelper.GetFreeActionSet();

			foreach (GOAPAction action in actions)
			{
				if (!action.Equals(actionToRemove))
					subset.Add(action);
			}

			return subset;
		}

		/// <summary>
		/// Checks if goal is met.
		/// </summary>
		/// <param name="goal">Goal that GOAPAgent is trying to achieve</param>
		/// <param name="state">Current dictionary of states including effects of completed actions</param>
		private bool GoalInState(KeyValuePair<string, bool> goal, Dictionary<string, bool> state)
		{
			bool match = state.ContainsKey(goal.Key) && state[goal.Key] == goal.Value;

			return match;
		}

		/// <summary>
		/// Checks if the parent state has the conditions for this action's preconditions.
		/// </summary>
		/// <param name="preconditions">Preconditions of current action</param>
		/// <param name="parentState">Parent state, which includes effects from previous actions</param>
		private bool InState(Dictionary<string, bool> preconditions, Dictionary<string, bool> parentState)
		{
			bool allMatch = true;

			foreach (KeyValuePair<string, bool> t in preconditions)
			{
				bool match = parentState.ContainsKey(t.Key) && parentState[t.Key] == t.Value;
				if (!match)
				{
					allMatch = false;
					break;
				}
			}

			return allMatch;
		}

		/// <summary>
		/// Apply the action's effects to the parent state.
		/// </summary>
		/// <param name="actionEffects">Current state</param>
		/// <param name="parentState">State change</param>
		private Dictionary<string, bool> PopulateState(Dictionary<string, bool> actionEffects, Dictionary<string, bool> parentState)
		{
			Dictionary<string, bool> state = GOAPPlannerHelper.GetFreeState();

			state.Clear();

			foreach (KeyValuePair<string, bool> s in actionEffects)
			{
				state.Add(s.Key, s.Value);
			}

			foreach (KeyValuePair<string, bool> change in parentState)
			{
				// If key exists in the current state, update the value.
				if (state.ContainsKey(change.Key))
				{
					state[change.Key] = change.Value;
				}
				else
				{
					state.Add(change.Key, change.Value);
				}
			}

			return state;
		}
	}
}