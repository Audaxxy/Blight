using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is an interface, which is used to create AI agents.
// Any AI agent that wants to use GOAP must implement this interface. It provides information to the GOAP planner so it can plan what actions to use.
// It also provides an interface for the planner to give feedback to the Agent and report success/failure.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
	public interface IAIAgent
	{
		/// <summary>
		/// Data Storage getter and setter. Used to store dynamic data about the world.
		/// Don't forget to assign value.
		/// </summary>
		DataStorage DataHolder { get; set; }

		/// <summary>
		/// Behaviour getter and setter.
		/// Should be assigned with value from CreateBehaviour() to allow different behaviours in subclasses of AI Agent.
		/// </summary>
		IBehaviour Behaviour { get; set; }

		/// <summary>
		/// GOAP Agent instance getter and setter. Used to abort plan, add, get and remove actions to 
		/// Assigned by GOAP Agent itself. Don't use in Start() function, since it might be not assigned yet.
		/// </summary>
		IGOAPAgent GOAPAgent { get; set; }

		/// <summary>
		/// The starting state of the Agent and the world. Supply what states are needed for actions to run. Recreated when we enter the idle state.
		/// </summary>
		Dictionary<string, bool> CreateWorldState();

		/// <summary>
		/// Gives the planner a new goal so it can figure out the actions needed to fulfill it. Recreated when we enter the idle state.
		/// Usually done in specific subclass of AI Agent, so it should be abstract in parent class.
		/// </summary>
		Dictionary<string, bool> CreateGoalState();

		/// <summary>
		/// Function for the move state of the FSM, tells the agent how to move to its target. Called during update in FSM.
		/// Returns true if AI Agent is at the target and next action can performed. False if it is not there yet.
		/// </summary>
		/// <param name="nextAction">Action to do after the agent arrives at the target</param>
		bool MoveAgent(GOAPAction nextAction);

		/// <summary>
		/// Creates new DataStorage, adds data into it and returns it.
		/// Usually done in specific subclass of AI Agent, so it should be abstract in parent class.
		/// </summary>
		DataStorage CreateDataStorage();

		/// <summary>
		/// Creates new Behaviour and returns it.
		/// Usually done in specific subclass of AI Agent, so it should be abstract in parent class.
		/// </summary>
		IBehaviour CreateBehaviour();

		/// <summary>
		/// Creates new set of avaliable actions and returns it.
		/// Usually done in specific subclass of AI Agent, so it should be abstract in parent class.
		/// </summary>
		HashSet<GOAPAction> CreateActionSet();

		/// <summary>
		/// Handles what happens when no sequence of actions could be found for the supplied goal in Idle State.
		/// Used for debuging.
		/// </summary>
		/// <param name="failedGoal"></param>
		void PlanFailed(Dictionary<string, bool> failedGoal);

		/// <summary>
		/// Handles what happens when the agent finds a plan for the supplied goal. These are the actions the Agent will perform, in order in Idle State.
		/// Used for debuging.
		/// </summary>
		/// <param name="goal"></param>
		/// <param name="actions"></param>
		void PlanFound(KeyValuePair<string, bool> goal, Queue<GOAPAction> actions);

		/// <summary>
		/// Handles what happens when actions are finished and the goal was reached.
		/// Used for debuging.
		/// </summary>
		void ActionsFinished();

		/// <summary>
		/// Handles what happens when one of the actions caused the plan to abort.
		/// Used for debuging.
		/// </summary>
		/// <param name="aborter">Action that caused plan to be aborted</param>
		void PlanAborted(GOAPAction aborter);
	}
}
 
 