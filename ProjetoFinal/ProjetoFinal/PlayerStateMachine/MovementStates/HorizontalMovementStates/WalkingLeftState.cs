﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.PlayerStateMachine.MovementStates.HorizontalMovementStates
{
    class WalkingLeftState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {            
            player.FacingRight = false;
            player.speed.X -= player.moveSpeed;
            player.speed.X = MathHelper.Clamp(player.speed.X, player.minSpeed.X, player.maxSpeed.X);

            player.MoveXBy(player.speed.X);
            
            return this;
        }

        public override HorizontalMovementState StoppedMovingLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.StoppingWalkingLeft);
            return playerStates[HorizontalStateType.StoppingWalkingLeft];
        }

        public override HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.WalkingRight);
            return playerStates[HorizontalStateType.WalkingRight];
        }
        
        public override string ToString()
        {
            return "WalkingLeft";
        }
    }
}
