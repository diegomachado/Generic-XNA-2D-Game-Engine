﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class JumpingState : VerticalMovementState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Vertical, (short)VerticalStateType.Jumping);
            player.spriteMap.Play("idle");

            if (player.MapCollideY(-1))
                player.speed.Y = 0;
            
            player.speed.Y += player.gravity;
            player.speed.Y = MathHelper.Clamp(player.speed.Y, player.minSpeed.Y, player.maxSpeed.Y);
            player.MoveYBy(player.speed.Y);

            if (player.OnGround())
            {
                player.speed.Y = 0;
                return playerStates[VerticalStateType.Idle];
            }
            
            return this;
        }

        public override string ToString()
        {
            return "Jumping";
        }
    }
}
