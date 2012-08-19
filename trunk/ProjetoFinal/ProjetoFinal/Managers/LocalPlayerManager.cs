﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;

namespace ProjetoFinal.Managers
{
    class LocalPlayerManager
    {
        Camera camera = Camera.Instance;
        Player localPlayer;
        float shootingTimer;

        HorizontalMovementState localPlayerHorizontalState;
        VerticalMovementState localPlayerVerticalState;
        ActionState localPlayerActionState;
        Dictionary<HorizontalStateType, HorizontalMovementState> localPlayerHorizontalStates = new Dictionary<HorizontalStateType, HorizontalMovementState>();
        Dictionary<VerticalStateType, VerticalMovementState> localPlayerVerticalStates = new Dictionary<VerticalStateType, VerticalMovementState>();
        Dictionary<ActionStateType, ActionState> localPlayerActionStates = new Dictionary<ActionStateType, ActionState>();

        public short playerId { get; set; }
        public Player LocalPlayer { get { return localPlayer; } }

        public LocalPlayerManager()
        {
            // Horizontal
            
            localPlayerHorizontalStates[HorizontalStateType.Idle] = new HorizontalIdleState();
            localPlayerHorizontalStates[HorizontalStateType.WalkingLeft] = new WalkingLeftState();
            localPlayerHorizontalStates[HorizontalStateType.WalkingRight] = new WalkingRightState();
            localPlayerHorizontalStates[HorizontalStateType.StoppingWalkingLeft] = new StoppingWalkingLeftState();
            localPlayerHorizontalStates[HorizontalStateType.StoppingWalkingRight] = new StoppingWalkingRightState();

            localPlayerHorizontalState = localPlayerHorizontalStates[HorizontalStateType.Idle];

            // Vertical

            localPlayerVerticalStates[VerticalStateType.Idle] = new VerticalIdleState();
            localPlayerVerticalStates[VerticalStateType.Jumping] = new JumpingState();
            localPlayerVerticalStates[VerticalStateType.StartedJumping] = new StartedJumpingState();

            localPlayerVerticalState = localPlayerVerticalStates[VerticalStateType.Jumping];

            // Action

            localPlayerActionStates[ActionStateType.Idle] = new ActionIdleState();
            localPlayerActionStates[ActionStateType.Attacking] = new AttackingState();
            localPlayerActionStates[ActionStateType.Defending] = new DefendingState();
            localPlayerActionStates[ActionStateType.Shooting] = new ShootingState();
            localPlayerActionStates[ActionStateType.PreparingShot] = new PreparingShotState();

            localPlayerActionState = localPlayerActionStates[ActionStateType.Idle];
        }

        public void createLocalPlayer(short id)
        {
            playerId = id;
            localPlayer = new Player(TextureManager.Instance.getTexture(TextureList.Bear), new Vector2(240, 40), new Rectangle(5, 1, 24, 30));  
        }

        public void Update(GameTime gameTime, InputManager inputManager, Layer collisionLayer)
        {
            if (localPlayer == null)
                return;

            // Vertical Movement Input

            if (inputManager.Jump)
            {
                localPlayerVerticalState = localPlayerVerticalState.Jumped(playerId, localPlayer, localPlayerVerticalStates);
            }

            // Horizontal Movement Input

            if (inputManager.Left)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.MovedLeft(playerId, localPlayer, localPlayerHorizontalStates);
            }
            else if (inputManager.PreviouslyLeft)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.StoppedMovingLeft(playerId, localPlayer, localPlayerHorizontalStates);
            }

            if (inputManager.Right)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.MovedRight(playerId, localPlayer, localPlayerHorizontalStates);
            }
            else if (inputManager.PreviouslyRight)
            {
                localPlayerHorizontalState = localPlayerHorizontalState.StoppedMovingRight(playerId, localPlayer, localPlayerHorizontalStates);
            }

            // Action Input

            if (inputManager.PreparingShot)
            {
                localPlayerActionState = localPlayerActionState.PreparingShot(playerId, localPlayer, localPlayerActionStates);

                shootingTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                localPlayerActionState = localPlayerActionState.ShotReleased(playerId, localPlayer, shootingTimer, inputManager.MousePosition, localPlayerActionStates);

                shootingTimer = 0f;
            }

            // Updates

            // TODO: Updates devem ficar depois ou antes?
            localPlayerHorizontalState = localPlayerHorizontalState.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerHorizontalStates);
            localPlayerVerticalState = localPlayerVerticalState.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerVerticalStates);
            localPlayerActionState = localPlayerActionState.Update(playerId, gameTime, localPlayer, localPlayerActionStates);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (localPlayer != null)
            {
                localPlayer.Draw(spriteBatch);
                
                spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 0, 230, 170), new Color(0, 0, 0, 0.2f));

                spriteBatch.DrawString(spriteFont, "" + localPlayerHorizontalState, new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 20) - camera.Position, Color.White);
                spriteBatch.DrawString(spriteFont, "" + localPlayerVerticalState, new Vector2(localPlayer.Position.X + 8, localPlayer.Position.Y - 40) - camera.Position, Color.White);

                spriteBatch.DrawString(spriteFont, "X: " + (int)localPlayer.Position.X, new Vector2(5f, 05f), Color.White);
                spriteBatch.DrawString(spriteFont, "Y: " + (int)localPlayer.Position.Y, new Vector2(5f, 25f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.X: " + (int)localPlayer.Speed.X, new Vector2(5f, 45f), Color.White);
                spriteBatch.DrawString(spriteFont, "Speed.Y: " + (int)localPlayer.Speed.Y, new Vector2(5f, 65f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.X: " + (int)camera.Position.X, new Vector2(5f, 85f), Color.White);
                spriteBatch.DrawString(spriteFont, "Camera.Y: " + (int)camera.Position.Y, new Vector2(5f, 105f), Color.White);
                spriteBatch.DrawString(spriteFont, "Horizontal State: " + localPlayerHorizontalState, new Vector2(5f, 125f), Color.White);
                spriteBatch.DrawString(spriteFont, "Vertical State: " + localPlayerVerticalState, new Vector2(5f, 145f), Color.White);
           }
        }

        public void DrawPoint(SpriteBatch spriteBatch, Point position, int size, Color color)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(position.X - (int)camera.Position.X, position.Y - (int)camera.Position.Y, size, size), Color.White);
        }

        public void DrawBoundingBox(Rectangle r, int borderWidth, Color color, SpriteBatch spriteBatch)
        {
            // TODO: Transformar conta com Camera em uma funcao de Camera
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)camera.Position.X, r.Top - (int)camera.Position.Y, borderWidth, r.Height), Color.White); // Left
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Right - (int)camera.Position.X, r.Top - (int)camera.Position.Y, borderWidth, r.Height), Color.White); // Right
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)camera.Position.X, r.Top - (int)camera.Position.Y, r.Width, borderWidth), Color.White); // Top
            spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(color), new Rectangle(r.Left - (int)camera.Position.X, r.Bottom - (int)camera.Position.Y, r.Width, borderWidth), Color.White); // 
        }
    }
}