﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;

namespace SoccerXNA.Teams.BucklandTeam.GoalKeeperStates
{
    public class InterceptBall : State<GoalKeeper>
    {
        private static InterceptBall instance;

        private InterceptBall()
        {
        }

        public static InterceptBall Instance()
        {
            if (instance == null)
            {
                instance = new InterceptBall();
            }
            return instance;
        }

        public override void Enter(GoalKeeper keeper)
        {
            keeper.Steering().PursuitOn();
            Debug.WriteLine("Goalie " + keeper.ID() + " enters InterceptBall");
        }

        public override void Execute(GoalKeeper keeper)
        {
            //if the goalkeeper moves to far away from the goal he should return to his
            //home region UNLESS he is the closest player to the ball, in which case,
            //he should keep trying to intercept it.
            if (keeper.TooFarFromGoalMouth() && !keeper.isClosestPlayerOnPitchToBall())
            {
                keeper.GetFSM().ChangeState(ReturnHome.Instance());

                return;
            }

            //if the ball becomes in range of the goalkeeper's hands he traps the 
            //ball and puts it back in play
            if (keeper.BallWithinKeeperRange())
            {
                keeper.Ball().Trap();

                keeper.Pitch().SetGoalKeeperHasBall(true);

                keeper.GetFSM().ChangeState(PutBallBackInPlay.Instance());

                return;
            }
        }

        public override void Exit(GoalKeeper keeper)
        {
            keeper.Steering().PursuitOff();
        }

        public override bool OnMessage(GoalKeeper keeper, Telegram message)
        {
            return false;
        }
    }
}