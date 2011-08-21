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
    class HealthBar : GameObject
    {
        public int health;
        Rectangle healthRect;
        public HealthBar(Game1 theGame, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(theGame, _content, viewPort, _sprite, false, 0.1f)
        {
            healthRect = new Rectangle(0, 30, 100, 10);
            spriteRectangle = healthRect;
            health = 100;
        }
        public override void Update(float elapsedTime)
        {
            if (health <= 0)
            {
                health = 100;
            }
            healthRect.Width = health;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color healthColor = Color.Green;
            if (health < 50)
            {
                healthColor = Color.Yellow;
            }
            else
            {
                if (health < 20)
                {
                    healthColor = Color.Firebrick;
                }
            }
            //spriteBatch.Begin();
            spriteBatch.Draw(sprite, healthRect, null, healthColor, 0, Vector2.Zero, SpriteEffects.None, layer + 0.1f);
            spriteBatch.Draw(sprite, spriteRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layer);                         
            //spriteBatch.End();
            //base.Draw(gameTime);
        }        
    }
}
