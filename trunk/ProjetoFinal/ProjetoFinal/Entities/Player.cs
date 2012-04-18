﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Entities
{
    public enum PlayerState : short
    {
        Idle,
        WalkingLeft,
        WalkingRight,
        Jumping,
        JumpingRight,
        JumpingLeft
    }

    class Player
    {
        public PlayerState State { get; set; }
        public Texture2D Skin   { get; set; }
        public Vector2 Position { get; set; }

        public float Friction   { get; set; }
        public float Gravity    { get; set; }
        public float JumpForce  { get; set; }

        public Vector2 speed = Vector2.Zero;

        public double LastUpdateTime { get; set; }

        public Player(Texture2D playerSkin, Vector2 playerPosition)
        {
            Skin      = playerSkin;
            Position  = playerPosition;

            speed = Vector2.Zero;
            Friction  = 0.85f;
            Gravity   = 0.3f;
            JumpForce = -8.0f;
            State = PlayerState.Idle;
        }

        public int Width
        {
            get { return Skin.Width; }
        }

        public int Height
        {
            get { return Skin.Height; }
        }

        public void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Skin, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}