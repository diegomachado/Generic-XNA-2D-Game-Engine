﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetoFinal.Managers
{
    public enum TextureList {
        Bear,
        Ranger,
        CollisionBoxBorder
    }

    class TextureManager
    {
        private static TextureManager instance;

        ContentManager Content;
        GraphicsDevice GraphicsDevice;

        public void setContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.Content = content;
            this.GraphicsDevice = graphicsDevice;
        }

        public static TextureManager Instance
        {
            get 
            {
                if (instance == null)
                    instance = new TextureManager();

                return instance;
            }
        }

        public Texture2D getTexture(TextureList texture)
        {
            String textureName = "bear";

            switch (texture)
            {
                case TextureList.Bear:
                    textureName = "bear";
                    break;

                case TextureList.Ranger:
                    textureName = "ranger";
                    break;
            }

            return this.Content.Load<Texture2D>( String.Format(@"sprites/{0}", textureName) );       
        }

        public Texture2D getPixelTextureByColor(Color color)
        {
            var t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { color });

            return t;
        }
    }
}