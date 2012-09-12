﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities.Utils;
using OgmoLibrary;

namespace ProjetoFinal.Entities
{
    // TODO: Setar todo mundo que não muda como CONST
    class Entity
    {
        public enum Type
        {
            Generic,
            Player,
            Arrow,
            Sword
        }

        [Flags]
        public enum Flags
        {
            None = 0,
            Gravity = 1,
            Ghost = 2,
            MapOnly = 4
        }

        static public List<Entity> Entities = new List<Entity>();

        public Type type;
        public Flags flags;             
        public Animation baseAnimation;

        public bool visible = true;
        public bool collidable = true;

        public Vector2 position;
        public Vector2 origin;
     
        public int Width      { get { return baseAnimation.FrameSize.X; } }
        public int Height     { get { return baseAnimation.FrameSize.Y; } }
        public Vector2 Center { get { return position + baseAnimation.TextureCenter; } }        

        protected Rectangle boundingBox;
        public virtual Rectangle BoundingBox
        {
            get { return boundingBox; }
            protected set { boundingBox = value; }
        }
        protected Rectangle collisionBox;
        public Rectangle CollisionBox
        {
            get
            {
                collisionBox = BoundingBox;
                collisionBox.Offset((int)position.X, (int)position.Y);
                return collisionBox;
            }
        }

        public Entity(Vector2 _position, Rectangle _boundingBox = new Rectangle())
        {
            type = Type.Generic;
            flags = Flags.None;
            position = _position;
            origin = Vector2.Zero;
            boundingBox = _boundingBox;
            Entities.Add(this);
        }

        ~Entity()
        {
            Entity.Entities.Remove(this);
        }

        #region Game Logic
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void LoadContent()
        {
            baseAnimation = new Animation(TextureManager.Instance.getTexture(TextureList.Bear), 1, 1);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            baseAnimation.Draw(spriteBatch, new Vector2(0,0), true);
        }
        #endregion

        #region Collision
        private int sign;
        private Tile tileCheck;
        private Point tilePosition;
        private MapManager mapManager;
        private Rectangle collisionBoxOffset;
        private Point corner1, corner2;
        Vector2 moveTemp = Vector2.Zero;
        float moveTempX = 0;
        float moveTempY = 0;

        public void MoveBy(Vector2 moveAmount)
        {
            if (moveAmount == Vector2.Zero) return;

            moveTemp += moveAmount;
            moveAmount.X = (float)Math.Round(moveAmount.X, MidpointRounding.AwayFromZero);
            moveAmount.Y = (float)Math.Round(moveAmount.Y, MidpointRounding.AwayFromZero);
            moveTemp -= moveAmount;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position += moveAmount;
            }
            else
            {
                if (moveAmount.X != 0)
                {
                    sign = Math.Sign(moveAmount.X);

                    while (moveAmount.X != 0)
                    {
                        if (MapCollideX(sign)) break;
                        else position.X += sign;
                        moveAmount.X -= sign;
                    }
                }
                if (moveAmount.Y != 0)
                {
                    sign = Math.Sign(moveAmount.Y);

                    while (moveAmount.Y != 0)
                    {
                        if (MapCollideY(sign)) break;
                        else position.Y += sign;
                        moveAmount.Y -= sign;
                    }
                }
            }
        }
        public void MoveXBy(float moveAmountX)
        {
            if (moveAmountX == 0) return;

            moveTempX += moveAmountX;
            moveAmountX = (float)Math.Round(moveAmountX, MidpointRounding.AwayFromZero);
            moveTempX -= moveAmountX;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position.X += moveAmountX;
            }
            else
            {
                if (moveAmountX != 0)
                {
                    sign = Math.Sign(moveAmountX);

                    while (moveAmountX != 0)
                    {
                        if (MapCollideX(sign)) break;
                        else position.X += sign;
                        moveAmountX -= sign;
                    }
                }
            }
        }
        public void MoveYBy(float moveAmountY)
        {
            if (moveAmountY == 0) return;

            moveTempY += moveAmountY;
            moveAmountY = (float)Math.Round(moveAmountY, MidpointRounding.AwayFromZero);
            moveTempY -= moveAmountY;

            if ((flags & ~Flags.Ghost) == Flags.Ghost)
            {
                position.Y += moveAmountY;
            }
            else
            {
                if (moveAmountY != 0)
                {
                    sign = Math.Sign(moveAmountY);

                    while (moveAmountY != 0)
                    {
                        if (MapCollideY(sign)) break;
                        else position.Y += sign;
                        moveAmountY -= sign;
                    }
                }
            }
        }

        public bool MapCollideX(float moveAmount)
        {
            if (moveAmount < 0)
            {
                corner1.X = CollisionBox.Left;
                corner1.Y = CollisionBox.Top + 1;
                corner2.X = CollisionBox.Left;
                corner2.Y = CollisionBox.Bottom - 1;
            }
            else
            {
                corner1.X = CollisionBox.Right;
                corner1.Y = CollisionBox.Top + 1;
                corner2.X = CollisionBox.Right;
                corner2.Y = CollisionBox.Bottom - 1;
            }

            return (TilePixelCollision(corner1) || TilePixelCollision(corner2));
        }
        public bool MapCollideY(float moveAmount)
        {
            if (moveAmount < 0)
            {
                corner1.X = CollisionBox.Left + 1;
                corner1.Y = CollisionBox.Top;
                corner2.X = CollisionBox.Right - 1;
                corner2.Y = CollisionBox.Top;
            }
            else
            {
                corner1.X = CollisionBox.Left + 1;
                corner1.Y = CollisionBox.Bottom;
                corner2.X = CollisionBox.Right - 1;
                corner2.Y = CollisionBox.Bottom;
            }

            return (TilePixelCollision(corner1) || TilePixelCollision(corner2));
        }        
        private bool TileCollision(Point tilePosition)
        {
            tileCheck = MapManager.Instance.CollisionLayer.Tiles[tilePosition];
            return (tileCheck.Id == 0) ? false : true;
        }
        private bool TilePixelCollision(Point pixelPosition)
        {
            mapManager = MapManager.Instance;
            tilePosition.X = pixelPosition.X / mapManager.TileSize.X;
            tilePosition.Y = pixelPosition.Y / mapManager.TileSize.Y;
            tileCheck = mapManager.CollisionLayer.Tiles[tilePosition];
            return (tileCheck.Id == 0) ? false : true;
        }        

        public bool PositionValidOnEntity(Entity entity, Point offset)
        {
            collisionBoxOffset = entity.CollisionBox;
            collisionBoxOffset.Offset(offset);

            if (this != entity && entity.Collides(collisionBoxOffset))
            {
                EntityCollision entityCollision = new EntityCollision(this, entity);
                EntityCollision.EntityCollisions.Add(entityCollision);

                return false;
            }

            return true;
        }
        public bool Collides(Rectangle collisionBox)
        {
            return CollisionBox.Intersects(collisionBox);
        }
        public virtual bool OnCollision(Entity entity)
        {
            return true;
        }
        #endregion
    }
}
