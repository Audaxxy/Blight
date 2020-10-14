using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//////////////////////////////////////////////////////////////////
// Script Purpose: A general enemy class.
// You should subclass this for specific enemy classes and implement the createGoalState() method that will populate the goal for the GOAP planner.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    /// <summary>
    /// All world state keys.
    /// </summary>
    public class EnemyBaseWorldStateKeys
    {
        public static string Alive = "Alive";
        public static string Injured = "Injured";
        public static string EnemyVisible = "EnemyVisible";
        public static string GameFinished = "GameFinished";
        public static string Test = "Test";
    }

    [RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
    public abstract class EnemyBase : MonoBehaviour, IAIAgent
    {
        #region Getters & Setters

        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }

        public DataStorage DataHolder { get; set; }

        public IBehaviour Behaviour { get; set; }

        public IGOAPAgent GOAPAgent { get; set; }

        #endregion

        protected virtual void Start()
        {
            // Getting Components
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();

            // Actually done in subclass
            DataHolder = CreateDataStorage();
            Behaviour = CreateBehaviour();

            // Optimizations
            _worldState = InitializeWorldState();
            FindAndLinkGOAPAgent(); // Just in case
        }

        protected virtual void Update()
        {

        }

        #region Core

        public Dictionary<string, bool> CreateWorldState()
        {
            // We are using initialized variable _worldState instead of creating new value ech time to optimize everything.

            // 1. Info about self.
            // -------------------------------
            //_worldState[WorldStateKeys.Alive] = defenseStats.Health.value != defenseStats.MaxHealth.value;
            //_worldState[WorldStateKeys.Injured] = defenseStats.alive;


            // 2. Vision.
            // -----------------------
            //_worldState[WorldStateKeys.EnemyVisible] = _vision.IsEnemyVisible();


            // 3. Gameplay Data.
            // -----------------------
            //_worldState[WorldStateKeys.GameFinished] = _gameplayData.IsGameFinished();


            // 4. Other.
            // -----------------------
            _worldState[EnemyBaseWorldStateKeys.Test] = false;

            return _worldState;
        }

        public bool MoveAgent(GOAPAction nextAction)
        {
            NavMeshAgent.SetDestination(nextAction._target.transform.position);

            // Check if we've reached the destination
            if (!NavMeshAgent.pathPending)
            {
                if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
                {
                    if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        nextAction.SetInRange(true);
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Subclass Core

        public abstract DataStorage CreateDataStorage();

        public abstract IBehaviour CreateBehaviour();

        public abstract HashSet<GOAPAction> CreateActionSet();

        public abstract Dictionary<string, bool> CreateGoalState();

        #endregion

        #region Optimizations

        /// <summary>
        /// Used to store worldState dictionary insteaead of creating a new one each time.
        /// </summary>
        private Dictionary<string, bool> _worldState;

        /// <summary>
        /// Used to initialize worldState dictionary only ones instead of each time.
        /// </summary>
        private Dictionary<string, bool> InitializeWorldState()
        {
            Dictionary<string, bool> worldState = new Dictionary<string, bool>();

            // 1. Info about self.
            // -------------------------------
            //_worldState.Add(WorldStateKeys.Alive, defenseStats.Health.value != defenseStats.MaxHealth.value);
            //_worldState.Add(WorldStateKeys.Injured, defenseStats.alive);


            // 2. Vision.
            // -----------------------
            //_worldState.Add(WorldStateKeys.EnemyVisible, _vision.IsEnemyVisible());


            // 3. Gameplay Data.
            // -----------------------
            //_worldState.Add(WorldStateKeys.GameFinished, _gameplayData.IsGameFinished());


            // 4. Other.
            // -----------------------
            worldState.Add(EnemyBaseWorldStateKeys.Test, false);

            return worldState;
        }

        /// <summary>
        /// Finds the GOAPAgent component and links it together with itself if it wasn't done by GOAPAgent.
        /// </summary>
        private void FindAndLinkGOAPAgent()
        {
            if (GOAPAgent == null)
            {
                this.gameObject.TryGetComponent(out IGOAPAgent tmp);
                if (tmp != null)
                {
                    GOAPAgent = tmp;
                }
            }
        }

        #endregion

        #region Debugging
        public void ActionsFinished()
        {
            // For debugging.
        }

        public void PlanAborted(GOAPAction aborter)
        {
            // For debugging.
        }

        public void PlanFailed(Dictionary<string, bool> failedGoal)
        {
            // For debugging.
        }

        public void PlanFound(KeyValuePair<string, bool> goal, Queue<GOAPAction> actions)
        {
            // For debugging.
        }

        #endregion
    }
}