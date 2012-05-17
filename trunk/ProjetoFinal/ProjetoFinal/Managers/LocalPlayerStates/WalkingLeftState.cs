﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class WalkingLeftState : LocalPlayerState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = false;

                return new JumpingLeftState();
            }
            
            localPlayer.SpeedX *= localPlayer.Friction;
            
            // So player doesn't slide forever
            if (Math.Abs(localPlayer.Speed.X) < 0.2)
            {
                localPlayer.SpeedX = 0;

                return new IdleState();
            }

            if (handleHorizontalCollision(localPlayer, collisionLayer))
                return new IdleState();
            else
                return this;
        }

        public override LocalPlayerState Jumped(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.JumpForce;

            return new JumpingLeftState();
        }

        public override LocalPlayerState MovingLeft(Player localPlayer)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState MovingRight(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return new WalkingRightState();
        }

        public override string ToString()
        {
            return "WalkingLeft";
        }
    }
}
