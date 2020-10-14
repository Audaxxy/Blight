using System.Collections.Generic;
using System.Linq;

//////////////////////////////////////////////////////////////////
// Script Purpose: Represents behaviour of Lesser Ghoul type of enemy.
// Determines which goals Lesser Ghoul has and their priority.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    /// <summary>
    /// All goal state keys.
    /// </summary>
    public class LesserGhoulGoalStateKeys
    {
        public static string AttackPlayer = "AttackPlayer";
    }

    /// <summary>
    /// All types of priority.
    /// </summary>
    public enum LesserGhoulPriorityWeight
    {
        LOW,
        NORMAL,
        HIGH
    }

    public class LesserGhoulBehaviour : IBehaviour
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
            _goalsWeight.Add(LesserGhoulGoalStateKeys.AttackPlayer, 0);
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
            _goalsWeight[LesserGhoulGoalStateKeys.AttackPlayer] = (int)LesserGhoulPriorityWeight.HIGH;

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