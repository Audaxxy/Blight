using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////
// Script Purpose: Stack-based Finite State Machine.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
	public class FSM
	{
		// Creation of FSMState delegate.
		public delegate void FSMState(FSM fsm, GameObject obj);

		// Stack to hold the states in.
		private Stack<FSMState> _stateStack = new Stack<FSMState>();

		/// <summary>
		/// Invokes FSMState delegate from the top of the stack.
		/// </summary>
		/// <param name="obj">GOAPAgent</param>
		public void Update(GameObject obj)
		{
			if (_stateStack.Peek() != null)
			{
				// FSMState delegates are declared and initialized in GOAPAgent.
				// There are only 3 of them: idleState, moveToState and performActionState.
				_stateStack.Peek().Invoke(this, obj);
			}
		}

		/// <summary>
		/// Pushes the state to stack.
		/// </summary>
		/// <param name="state">State of the FSM</param>
		public void PushState(FSMState state)
		{
			_stateStack.Push(state);
		}

		/// <summary>
		/// Pops the state stack.
		/// </summary>
		public void PopState()
		{
			_stateStack.Pop();
		}

		/// <summary>
		/// Clears the stack.
		/// </summary>
		public void ClearState()
		{
			_stateStack.Clear();
		}
	}
}