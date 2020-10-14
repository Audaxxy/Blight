using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: Represents Lesser Ghoul type of enemy.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class LesserGhoul : EnemyBase
    {
        // Slow on walking and attacking
        // Has 1 hp
        // Deals 1 damage
        // Chooses the closest player
        // Moves by bursts(lurch, stop, lurch, stop, like zombie)

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
            return new LesserGhoulBehaviour();
        }

        public override HashSet<GOAPAction> CreateActionSet()
        {
            HashSet<GOAPAction> availableActions = new HashSet<GOAPAction>();

            availableActions.Add(new LesserGhoulAttackAction());

            return availableActions;
        }

        public override Dictionary<string, bool> CreateGoalState()
        {
            return Behaviour.GoalsByPriority();
        }

        #endregion
    }
}