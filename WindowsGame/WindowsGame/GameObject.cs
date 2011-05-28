using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame
{
    public class GameObject : DrawableGameComponent
    {
        public Texture2D sprite;
        public Vector2 position;
        float rotation;

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;

                if (rotation >= (float)Math.PI * 2f || rotation <= (float)Math.PI * -2f)
                    rotation = 0f;
            }
        }
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public Color[] spriteTextureData
        {
            get;
            set;
        }
        public Matrix spriteTransform
        {
            get;
            set;
        }
        public Rectangle spriteRectangle
        {
            get;
            set;
        }
        public Game1 game
        {
            get;
            set;
        }
        public SpriteBatch spritebatch
        {
            get;
            set;
        }
        public ContentManager content
        {
            get;
            private set;
        }
        private Rectangle viewPort;
        public GameObject(Game1 _game, ContentManager _content, Rectangle _viewPort, Texture2D _sprite) : base(_game)
        {
            game = _game;
            sprite = _sprite;
            Rotation = 0.0f;
            position = Vector2.Zero;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
            content = _content;
            viewPort = _viewPort;            
            spritebatch = game.SpriteBatch;
            spriteTextureData =
              new Color[sprite.Width * sprite.Height];
            sprite.GetData(spriteTextureData);
            //spriteTransform =
            //        Matrix.CreateTranslation(new Vector3(center, 0.0f)) *
            //    // Matrix.CreateScale(block.Scale) *  would go here
            //        Matrix.CreateRotationZ(rotation) *
            //        Matrix.CreateTranslation(new Vector3(position, 0.0f));
            //spriteRectangle = Game1.CalculateBoundingRectangle(
            //            new Rectangle(0,0, sprite.Width, sprite.Height),
            //            spriteTransform);
        }

        protected override void LoadContent()
        {
           
        }
        public override void Update(GameTime gameTime)
        {
            //spriteTransform =
            //        Matrix.CreateTranslation(new Vector3(center, 0.0f)) *
            //    // Matrix.CreateScale(block.Scale) *  would go here
            //        Matrix.CreateRotationZ(rotation) *
            //        Matrix.CreateTranslation(new Vector3(position, 0.0f));
            //spriteRectangle = Game1.CalculateBoundingRectangle(
            //            new Rectangle(0, 0, sprite.Width, sprite.Height),
            //            spriteTransform);
            //spriteRectangle = Game1.CalculateBoundingRectangle(
            //            new Rectangle((int) position.X, (int) position.Y, sprite.Width, sprite.Height),
            //            spriteTransform);
            //base.Update(gameTime);
        }      
        
    }
}
