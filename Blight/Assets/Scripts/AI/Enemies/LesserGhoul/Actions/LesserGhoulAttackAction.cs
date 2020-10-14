using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: Represents attack action of Lesser Ghoul type of enemy.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class LesserGhoulAttackAction : GOAPAction
    {
        /// <summary>
        /// Indicates whether this action was finished or not.
        /// </summary>
        private bool _hasAttacked = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LesserGhoulAttackAction() : base()
        {
            AddEffect(LesserGhoulGoalStateKeys.AttackPlayer, true);
            //AddPrecondition("Alive", true);
            _cost = 1.0f;
        }

        public override void DoSubReset()
        {
            _hasAttacked = false;
            _target = null;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override bool IsDone()
        {
            return _hasAttacked;
        }

        public override bool CheckProceduralPrecondition(GameObject agent, DataStorage dataHolder)
        {
            _target = (GameObject)dataHolder.GetData("Player");

            if (_target != null)
            {
                return true;
            }

            return false;
        }

        public override bool Perform(GameObject agent, DataStorage dataHolder)
        {
            // Just chases the player.

            _hasAttacked = true;

            return true;
        }
    }
}