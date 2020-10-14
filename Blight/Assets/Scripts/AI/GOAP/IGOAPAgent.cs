using System;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is an interface, which we need to access the only parts of GOAP Agent that we should be accessing from AI Agent.
// Basically needed to interrupt actions.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public interface IGOAPAgent
    {
        /// <summary>
        /// Adds the given action to the list of available actions.
        /// </summary>
        /// <param name="action">action to add</param>
        void AddAction(GOAPAction action);

        /// <summary>
        /// Gets the action from the list of available actions.
        /// </summary>
        /// <param name="action">action to get</param>
        GOAPAction GetAction(Type action);

        /// <summary>
        /// Removes the action from the list of available actions.
        /// </summary>
        /// <param name="action">action to remove</param>
        void RemoveAction(GOAPAction action);

        /// <summary>
        /// Clears FSM stack and pushes idle state into it.
        /// </summary>
        void AbortFSM();
    }
}
