﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class ActionShootingState : ActionState
    {
        public override ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionType, ActionState> playerStates)
        {
            // TODO: Sincronizar animação de atirar com criação e disparo da flecha
            return this;
        }

        public override string ToString()
        {
            return "Action Shooting";
        }
    }
}