using IState = GameCore.Core.StateMachine.IState;
using IStateMachine = GameCore.Core.StateMachine.IStateMachine;

namespace GameCore.Services.Implementations.StateMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VContainer.Unity;
    using GameCore.Core.StateMachine;

    public abstract class StateMachine : IStateMachine, ITickable
    {
        #region inject

        protected readonly Dictionary<Type, IState> TypeToState;

        #endregion

        protected StateMachine(List<IState> listState)
        {
            this.TypeToState = listState.ToDictionary(state => state.GetType(), state => state);
        }

        public IState CurrentState { get; private set; }

        public void TransitionTo<T>() where T : class, IState
        {
            this.TransitionTo(typeof(T));
        }

        public void TransitionTo<TState, TModel>(TModel model) where TState : class, IState<TModel>
        {
            var stateType = typeof(TState);
            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            if (nextState is not TState nextStateT) return;
            nextStateT.Model = model;

            this.InternalStateTransition(nextState);
        }

        public virtual void TransitionTo(Type stateType)
        {
            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            this.InternalStateTransition(nextState);
        }

        private void InternalStateTransition(IState nextState)
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.Exit();
            }

            this.CurrentState = nextState;
            nextState.Enter();
        }

        public void Tick()
        {
            if (this.CurrentState is not ITickable tickableState) return;
            tickableState.Tick();
        }
    }
}