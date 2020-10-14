using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is used to optimize the use of nodes, states and action sets for GOAP Planner.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class GOAPPlannerHelper
    {
        #region Nodes

        static Stack<GOAPNode> _usedNodes = new Stack<GOAPNode>();
        static Stack<GOAPNode> _freeNodes = new Stack<GOAPNode>();

        /// <summary>
        /// Reinitializes free node, turns it into used one and returns it. If there is not enough free nodes, creates a new one.
        /// </summary>
        public static GOAPNode GetFreeNode(GOAPNode parent, float runningCost, float weight, Dictionary<string, bool> state,
            GOAPAction action)
        {
            GOAPNode free = null;
            if (_freeNodes.Count <= 0)
                free = new GOAPNode(parent, runningCost, weight, state, action);
            else
            {
                free = _freeNodes.Pop();
                free.Reinitialize(parent, runningCost, weight, state, action);
            }

            _usedNodes.Push(free);
            return free;
        }

        /// <summary>
        /// Moves used nodes into free nodes.
        /// </summary>
        public static void ReleaseNode()
        {
            while (_usedNodes.Count > 0)
            {
                _freeNodes.Push(_usedNodes.Pop());
            }
        }

        #endregion

        #region States

        static Stack<Dictionary<string, bool>> _usedState = new Stack<Dictionary<string, bool>>();
        static Stack<Dictionary<string, bool>> _freeState = new Stack<Dictionary<string, bool>>();

        /// <summary>
        /// Reinitializes free state, turns it into used one and returns it. If there is not enough free states, creates a new one.
        /// </summary>
        public static Dictionary<string, bool> GetFreeState()
        {
            Dictionary<string, bool> free = null;
            if (_freeState.Count > 0)
                free = _freeState.Pop();
            else
                free = new Dictionary<string, bool>();

            _usedState.Push(free);
            return free;
        }

        /// <summary>
        /// Moves used states into free states.
        /// </summary>
        private static void ReleaseState()
        {
            while (_usedState.Count > 0)
            {
                _freeState.Push(_usedState.Pop());
            }
        }

        #endregion

        #region Actions

        static Stack<HashSet<GOAPAction>> _usedActionSet = new Stack<HashSet<GOAPAction>>();
        static Stack<HashSet<GOAPAction>> _freeActionSet = new Stack<HashSet<GOAPAction>>();

        /// <summary>
        /// Reinitializes free action set, turns it into used one and returns it. If there is not enough free action sets, creates a new one.
        /// </summary>
        public static HashSet<GOAPAction> GetFreeActionSet()
        {
            HashSet<GOAPAction> free = null;
            if (_freeActionSet.Count > 0)
            {
                free = _freeActionSet.Pop();
                free.Clear();
            }
            else
                free = new HashSet<GOAPAction>();

            _usedActionSet.Push(free);
            return free;
        }

        /// <summary>
        /// Moves used action sets into free action sets.
        /// </summary>
        private static void ReleaseSubset()
        {
            while (_usedActionSet.Count > 0)
            {
                _freeActionSet.Push(_usedActionSet.Pop());
            }
        }

        #endregion

        #region Clean Up

        public static void Release()
        {
            ReleaseNode();
            ReleaseState();
            ReleaseSubset();
        }

        #endregion
    }
}