using System.Collections.Generic;
using System.Linq;

//////////////////////////////////////////////////////////////////
// Script Purpose: Represents behaviour of Ravager type of enemy.
// Determines which goals Ravager has and their priority.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    /// <summary>
    /// All goal state keys.
    /// </summary>
    public class RavagerGoalStateKeys
    {
        public static string AttackPlayer = "AttackPlayer";
    }

    /// <summary>
    /// All types of priority.
    /// </summary>
    public enum RavagerPriorityWeight
    {
        LOW,
        NORMAL,
        HIGH
    }

    public class RavagerBehaviour : IBehaviour
    {
        /// <summary>
        /// Used to store goals and their weights.
        /// </summary>
        readonly Dictionary<string, int> _goalsWeight = new Dictionary<string, int>();

        /// <summary>
        /// Used to store ordered goal states.
        /// </summary>
        readonly Dictionary<string, bool> _sortedTags = new Dictionary<string, bool>();

        public void Initialize()
        {
            _goalsWeight.Add(RavagerGoalStateKeys.AttackPlayer, 0);
        }

        public void Tick(IAIAgent Agent)
        {
            // in case if we want to update something.
        }

        public void Release()
        {
            // in case if we will do something similar to GOAPPlannerHelper.
        }

        public Dictionary<string, bool> GoalsByPriority()
        {
            _goalsWeight[RavagerGoalStateKeys.AttackPlayer] = (int)RavagerPriorityWeight.HIGH;

            var items = from pair in _goalsWeight
                        orderby pair.Value descending
                        select pair;

            _sortedTags.Clear();
            foreach (KeyValuePair<string, int> pair in items)
            {
                _sortedTags.Add(pair.Key, true);
            }

            return _sortedTags;
        }
    }
}