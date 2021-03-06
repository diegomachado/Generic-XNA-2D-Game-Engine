﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.GameStateEngine.GameStates
{
    class PauseState : GameState
    {
        public override void Update(GameTime gameTime)
        {
            if (inputManager.Exit)
                GameStatesManager.ExitGame();

            if (inputManager.Pause)
                GameStatesManager.ResignState(this);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont,
                                   "GAME PAUSED",
                                   new Vector2(graphicsManager.screen.X / 2, graphicsManager.screen.Y / 2),
                                   Color.Red);
        }
    }
}
