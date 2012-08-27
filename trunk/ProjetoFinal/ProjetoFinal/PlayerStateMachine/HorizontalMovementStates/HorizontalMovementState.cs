﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    enum HorizontalStateType : short
    {
        Idle,
        WalkingLeft,
        WalkingRight,
        StoppingWalkingLeft,
        StoppingWalkingRight
    }

    abstract class HorizontalMovementState : PlayerState
    {
        public abstract HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates);

        #region Public Messages
        public virtual HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }

        public virtual HorizontalMovementState StoppedMovingLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }

        public virtual HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }

        public virtual HorizontalMovementState StoppedMovingRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return this;
        }
        #endregion

        #region Protected Methods

        protected bool checkHorizontalCollision(Rectangle collisionBox, Vector2 speed, Layer collisionLayer)
        {
            Point corner1, corner2;

            if (speed.X < 0)
            {
                corner1 = new Point(collisionBox.Left, collisionBox.Top);
                corner2 = new Point(collisionBox.Left, collisionBox.Bottom);
            }
            else
            {
                corner1 = new Point(collisionBox.Right, collisionBox.Top);
                corner2 = new Point(collisionBox.Right, collisionBox.Bottom);
            }

            return (collisionLayer.TileIdByPixelPosition(corner1) || collisionLayer.TileIdByPixelPosition(corner2));
        }

        protected bool handleHorizontalCollision(Player localPlayer, Layer collisionLayer, double elapsedTime)
        {
            Rectangle collisionBoxOffset = localPlayer.CollisionBox;

            for (int i = 0; i < Math.Abs(localPlayer.Speed.X * elapsedTime); ++i)
            {
                collisionBoxOffset.Offset(Math.Sign(localPlayer.Speed.X), 0);
                if (!checkHorizontalCollision(collisionBoxOffset, localPlayer.Speed, collisionLayer))
                {
                    localPlayer.Position += new Vector2(Math.Sign(localPlayer.Speed.X), 0);
                }
                else
                {
                    localPlayer.SpeedX = 0;
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
