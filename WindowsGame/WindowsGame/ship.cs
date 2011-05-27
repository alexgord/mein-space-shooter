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
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

namespace WindowsGame
{
    class ship : GameObject
    {
       // private Vector2 position;
       // private Vector2 velocity;
        //private Game1 game;
        //private Texture2D shipTexture;
        //public float rotation;
        private const float scale = 0.25f;
        const int MAX_BULLETS = 10;
        public int health;
        public Bullet[] bullets
        {
            get;
            set;
        }
        //private Vector2 origin;
        
        KeyboardState previousKeyboardState = Keyboard.GetState();
        public ship(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(_game, _content, viewPort, _sprite)
        {
            //this.position = new Vector2(200f,100f);
            //this.velocity = new Vector2(3f, 0f);
            health = 100;
            bullets = new Bullet[MAX_BULLETS];
            for (int count = 0; count < MAX_BULLETS; count++)
            {
                bullets[count] = new Bullet(game, game.Content, game.viewportRect, game.Content.Load<Texture2D>("Bullet"));
                game.Components.Add(bullets[count]);
            }
            //this.content  = content;
            position = new Vector2(120, 280);
        }

        protected override void LoadContent()
        {
            //sprite = content.Load<Texture2D>("Ship");
            
                //origin = new Vector2(shipTexture.Width / 2, shipTexture.Height / 2);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //var spritebatch = game.spriteBatch;
            spritebatch.Begin();
            spritebatch.Draw(sprite, position, null, Color.White, rotation, center, 1.0f, SpriteEffects.None, 0);
            //spritebatch.Draw(shipTexture,
            //                    position,
            //                    null,
            //                    Color.White,
            //                    0.0f,
            //                    origin,
            //                    scale,
            //                    SpriteEffects.None,
            //                    0.0f);

            spritebatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if (game.viewportRect.Contains(new Rectangle((int)position.X + (int)Math.Sin(rotation),
                    (int)position.Y + (int)Math.Cos(rotation) * -1, 0, 0)))
                {
                    velocity = new Vector2((float)Math.Sin(rotation),
                                        (float)Math.Cos(rotation) * -1) * 5.0f;
                    position += velocity;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                //ship.position.Y += ship.velocity.Y;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                rotation += 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                rotation -= 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                FireBullet();
            }

            position.X = MathHelper.Clamp(position.X, 0, game.viewportRect.Right);
            position.Y = MathHelper.Clamp(position.Y, 0, game.viewportRect.Bottom);
            previousKeyboardState = keyboardState;
            //base.Update(gameTime);
        }

        void FireBullet()
        {
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = position + new Vector2((float)Math.Sin(rotation),
                               (float)Math.Cos(rotation) * -1);

                    bullet.velocity = new Vector2((float)Math.Sin(rotation),
                               (float)Math.Cos(rotation) * -1) * 10.0f;
                    bullet.rotation = rotation;
                    return;
                }
            }
        }
    }
}
