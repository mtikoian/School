﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.MinersStates
{
    public class QuenchThirst : State<Miner>
    {
        private static QuenchThirst instance;

        private QuenchThirst()
        {
        }

        public static QuenchThirst Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QuenchThirst();
                }
                return instance;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != Location.Saloon)
            {
                miner.ChangeLocation(Location.Saloon);
                miner.OutputStatusMessage("Boy, ah sure is thusty! Walking to the saloon");
            }
        }

        public override void Execute(Miner miner)
        {
            miner.BuyAndDrinkAWhiskey();
            miner.OutputStatusMessage("That's mighty fine sippin liquer");

            miner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance);
        }

        public override void Exit(Miner miner)
        {
            miner.OutputStatusMessage("Leaving the saloon, feelin' good");
        }
    }
}
