﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.PlayerStateMachine.ActionStates
{
    class ActionIdleState : ActionState
    {
        public override ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return this;
        }

        public override ActionState PreparingShot(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.PreparingShot];
        }

        public override ActionState Defended(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.Defending];
        }

        public override ActionState Attacked(short playerId, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            return playerStates[ActionStateType.Attacking];
        }

        public override string ToString()
        {
            return "Action Idle";
        }
    }
}
