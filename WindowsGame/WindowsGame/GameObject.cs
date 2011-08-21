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
    public class GameObject //: DrawableGameComponent
    {
        public Texture2D sprite;
        public Vector2 position;
        float rotation;
        public float layer;
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
        private bool dieOnExit;
        public GameObject(Game1 _game, ContentManager _content, Rectangle _viewPort, Texture2D _sprite, bool _dieOnExit, float _layer) //: base(_game)
        {
            layer = _layer;
            game = _game;
            sprite = _sprite;
            Rotation = 0.0f;
            position = Vector2.Zero;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = true;
            content = _content;
            viewPort = _viewPort;            
            spritebatch = game.SpriteBatch;
            spriteTextureData =
              new Color[sprite.Width * sprite.Height];
            sprite.GetData(spriteTextureData);
            dieOnExit = _dieOnExit;
        }

        protected /*override*/ void LoadContent()
        {
           
        }
        public virtual void Update(float elapsedTime)
        {

            if (!game.viewportRect.Contains(new Point((int)position.X, (int)position.Y)) && dieOnExit)
            {
                alive = false;
            }
            //base.Update(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            if (alive)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, Rotation, center, 1.0f, SpriteEffects.None, layer);
            }
            //spriteBatch.End();
            //base.Draw(gameTime);
        }
        
    }
}
