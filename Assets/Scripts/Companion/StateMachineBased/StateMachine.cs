using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public interface IState
    {
        public void Enter();
        public void Execute();
        public void Exit();
    }

    public class StateMachine
    {
        IState currentState;

        public void ChangeState(IState newState)
        {
            if(currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            if(currentState != null) { currentState.Execute(); }
        }
    }

    public class TestState : IState
    {
        private Unit owner;
        public TestState(Unit owner) { this.owner = owner; }

        void IState.Enter()
        {
            throw new System.NotImplementedException();
        }

        void IState.Execute()
        {
            throw new System.NotImplementedException();
        }

        void IState.Exit()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Unit : MonoBehaviour
    {
        StateMachine stateMachine = new StateMachine();

        private void Start()
        {
            stateMachine.ChangeState(new TestState(this));
        }

        private void Update()
        {
            stateMachine.Update();
        }
    }
}

