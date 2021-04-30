using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    NoGoalsCompleted,
    FewGoalsCompleted,
    SomeGoalsCompleted,
    ManyGoalsCompleted,
    AlmostGoalsCompleted,
    AllGoalsCompleted,
    CaughtByGnod
}

public enum Transition
{
    CompletedGoal,
    GotCaughtByGnod
}

public class GameStateMachine
{
    private State state;

    public State GetCurrentState() {
        return state;
    }

    public void MakeTransition(Transition tx) {
        if (tx == Transition.CompletedGoal) {
            switch (state) {
                case State.NoGoalsCompleted:
                    state = State.FewGoalsCompleted;
                    break;
                case State.FewGoalsCompleted:
                    state = State.SomeGoalsCompleted;
                    break;
                case State.SomeGoalsCompleted:
                    state = State.ManyGoalsCompleted;
                    break;
                case State.ManyGoalsCompleted:
                    state = State.AlmostGoalsCompleted;
                    break;
                case State.AlmostGoalsCompleted:
                    state = State.AllGoalsCompleted;
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        if (tx == Transition.GotCaughtByGnod) {
            state = State.CaughtByGnod;
        }
    }
}
