using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is an interface, which is used to create behaviours for AI agents.
// Helps to determine which goal is more important at the moment.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public interface IBehaviour
    {
        /// <summary>
        /// Initializes dictionary with goals and weights.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Used to influence parametres, which determine weight of goals, over time.
        /// </summary>
        /// <param name="Agent">AI Agent (not always needed)</param>
        void Tick(IAIAgent Agent);

        /// <summary>
        /// Used for potential garbage collection.
        /// </summary>
        void Release();

        /// <summary>
        /// Used to determine priority order of goal in createGoalState() from AI Agent.
        /// </summary>
        Dictionary<string, bool> GoalsByPriority();
    }
}