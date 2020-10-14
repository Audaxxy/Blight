using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is used to represent node for GOAP system.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class GOAPNode
    {
        private static int MaxID;
        public int _ID;

        public GOAPAction _action;
        public GOAPNode _parent;
        public float _runningCost;
        public Dictionary<string, bool> _state;
        public float _weight;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GOAPNode(GOAPNode parent, float runningCost, float weight, Dictionary<string, bool> state,
            GOAPAction action)
        {
            _ID = MaxID++;
            Reinitialize(parent, runningCost, weight, state, action);
        }

        /// <summary>
        /// Reinitializes node with new values.
        /// </summary>
        public void Reinitialize(GOAPNode parent, float runningCost, float weight, Dictionary<string, bool> state,
            GOAPAction action)
        {
            Clear();
            this._parent = parent;
            this._runningCost = runningCost;
            this._weight = weight;
            this._state = state;
            this._action = action;
        }

        /// <summary>
        /// Crears all values.
        /// </summary>
        private void Clear()
        {
            this._parent = null;
            this._runningCost = 0;
            this._weight = 0;
            this._state = null;
            this._action = null;
        }

        /// <summary>
        /// Compares nodes.
        /// </summary>
        /// <param name="valueToCompareWith"></param>
        public bool BetterThen(GOAPNode valueToCompareWith)
        {
            if (_weight > valueToCompareWith._weight && _runningCost < valueToCompareWith._runningCost)
                return true;

            if (_weight < valueToCompareWith._weight && _runningCost > valueToCompareWith._runningCost)
                return false;

            // Make weight > cost.
            bool better = (_weight / valueToCompareWith._weight - 1) >= (_runningCost / valueToCompareWith._runningCost - 1);

            return better;
        }
    }
}