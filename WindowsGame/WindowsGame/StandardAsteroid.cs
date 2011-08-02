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
    public class StandardAsteroid : GameObject
    {
        Random rand = new Random();
        public float rotationVelocity;
        public Boolean hasTouched;
        public int health;
        public StandardAsteroid(Game1 theGame, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(theGame, _content, viewPort, _sprite)
        {
            alive = true;
            hasTouched = false;
            rotationVelocity = 0;
            health = 10;
            position.X = rand.Next(game.viewportRect.Right);
            position.Y = rand.Next(game.viewportRect.Top);
            velocity.X = rand.Next(6) - 3;
            velocity.Y = rand.Next(6) - 3;
            while (velocity.X == 0 && velocity.Y == 0)
            {
                velocity.X = rand.Next(4) - 2;
                velocity.Y = rand.Next(4) - 2;
            }
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
                velocity.X = rand.Next(6) - 3;
                velocity.Y = rand.Next(6) - 3;
                while (velocity.X == 0 && velocity.Y == 0)
                {
                    velocity.X = rand.Next(6) - 3;
                    velocity.Y = rand.Next(6) - 3;
                }
            }
            position += velocity;
            Rotation += rotationVelocity;
            if (!game.viewportRect.Contains(new Point((int)position.X, (int)position.Y)) || health <= 0)
            {
                alive = false;
                health = 10;
            }
            if (health <= 0)
            {
                alive = false;
                health = 10;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            if (alive)
            {
                game.SpriteBatch.Draw(sprite, position, null, Color.White, Rotation, center, 1.0f, SpriteEffects.None, 0);
                //game.spriteBatch.Draw(content.Load<Texture2D>("Ship"), spriteRectangle, Color.White);
            }
            spritebatch.End();
            base.Draw(gameTime);
        }
    }
}
