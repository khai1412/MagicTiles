﻿namespace GameCore.Core.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }

    public interface IState<in TModel> : IState
    {
        public TModel Model { set; }
    }
}