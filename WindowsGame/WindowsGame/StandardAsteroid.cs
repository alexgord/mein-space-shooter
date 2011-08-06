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
    public class StandardAsteroid : DestructibleGameObject
    {
        const int FullHealth = 10;
        Random rand = new Random();
        public float rotationVelocity;
        public Boolean hasTouched;
        public StandardAsteroid(Game1 theGame, ContentManager _content, Rectangle viewPort, Texture2D _sprite, int _health) : base(theGame, _content, viewPort, _sprite, _health)
        {
            //alive = true;
            hasTouched = false;
            rotationVelocity = 0;
            health = FullHealth;
            position.X = rand.Next(game.viewportRect.Right);
            position.Y = rand.Next(game.viewportRect.Top);
            GenerateVelocity();
        }

        public override void Update(GameTime gameTime)
        {
            if (!alive)
            {
                alive = true;
                rotationVelocity = 0;
                position.X = rand.Next(game.viewportRect.Right);
                position.Y = rand.Next(game.viewportRect.Bottom);
                foreach (StandardAsteroid sa in game.standardAsteroids)
                {
                    if (!sa.Equals(this))
                    {
                        while (game.checkCollision(sa, this))
                        {
                            this.position.X = rand.Next(game.viewportRect.Right);
                            this.position.Y = rand.Next(game.viewportRect.Bottom);
                        }
                    }

                }
                GenerateVelocity();
            }
            position += velocity;
            Rotation += rotationVelocity;

            base.Update(gameTime);
        }

        //public override void Draw(GameTime gameTime)
        //{
            //spritebatch.Begin();
            //if (alive)
            //{
            //    game.SpriteBatch.Draw(sprite, position, null, Color.White, Rotation, center, 1.0f, SpriteEffects.None, 0);
            //    //game.spriteBatch.Draw(content.Load<Texture2D>("Ship"), spriteRectangle, Color.White);
            //}
            //spritebatch.End();
            //base.Draw(gameTime);
        //}

        public void GenerateVelocity()
        {
            velocity.X = rand.Next(4) - 2;
            velocity.Y = rand.Next(4) - 2;
            while (velocity.X == 0 && velocity.Y == 0)
            {
                velocity.X = rand.Next(4) - 2;
                velocity.Y = rand.Next(4) - 2;
            }
        }
    }
}
