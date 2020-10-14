using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: Represents Ravager type of enemy.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class Ravager : EnemyBase
    {
        // Fast on walking and attacking
        // Has 2 hp
        // Deals 3 damage
        // Chooses the furthest player
        // Moves and accelerates to his max velocity towards the player
        // If any player is in his way, while he moves, he attacks him

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Behaviour.Initialize();

            // Set NavMeshAgent values here
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            Behaviour.Tick(this);
        }

        #region Core

        public override DataStorage CreateDataStorage()
        {
            DataStorage dataHolder = new DataStorage();

            dataHolder.AddData("Behaviour", Behaviour);
            dataHolder.AddData("Player", GameObject.FindGameObjectWithTag("Player"));
            dataHolder.AddData("Players", GameObject.FindGameObjectsWithTag("Player"));

            return dataHolder;
        }
        public override IBehaviour CreateBehaviour()
        {
            return new RavagerBehaviour();
        }

        public override HashSet<GOAPAction> CreateActionSet()
        {
            HashSet<GOAPAction> availableActions = new HashSet<GOAPAction>();

            availableActions.Add(new RavagerAttackAction());

            return availableActions;
        }

        public override Dictionary<string, bool> CreateGoalState()
        {
            return Behaviour.GoalsByPriority();
        }

        #endregion
    }
}