﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using ProjetoFinal.Entities;
using ProjetoFinal.EventHeaders;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework.Input;
using OgmoLibrary;
using ProjetoFinal.PlayerStateMachine.VerticalMovementStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers
{
    class ArrowManager
    {
        EventManager eventManager = EventManager.Instance;
        List<Arrow> arrows;

        public ArrowManager()
        {
            arrows = new List<Arrow>();

            eventManager.ArrowShot += OnArrowShot;
        }

        public void Update(GameTime gameTime, Layer collisionLayer)
        {
            foreach (Arrow arrow in arrows)
            {
                arrow.Speed += (arrow.Gravity / 200);
                arrow.Position += arrow.Speed;
                //TODO: arrow.LastUpdateTime = updateTime;

                // TODO: Testar Colisão das flechas e destruí-las
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(spriteBatch);
                
                //spriteBatch.Draw(TextureManager.Instance.getPixelTextureByColor(Color.Black), new Rectangle(0, 430, 170, 170), new Color(0, 0, 0, 0.2f));
            }
        }

        private void OnArrowShot(object sender, ArrowShotEventArgs arrowShotEventArgs)
        {
            // TODO: Fazer a boundingBox corretamente
            arrows.Add(new Arrow(arrowShotEventArgs.playerId, TextureManager.Instance.getTexture(TextureList.Bear), arrowShotEventArgs.position, new Rectangle(5, 1, 24, 30), arrowShotEventArgs.speed));
        }
    }
}