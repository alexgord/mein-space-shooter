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
        public HealthBar(Game1 theGame, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(theGame, _content, viewPort, _sprite, false)
        {
            healthRect = new Rectangle(0, 30, 100, 10);
            spriteRectangle = healthRect;
            health = 100;
        }
        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                health = 100;
            }
            healthRect.Width = health;
        }

        public override void Draw(GameTime gameTime)
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
            spritebatch.Begin();
            spritebatch.Draw(sprite, spriteRectangle, Color.White);
            spritebatch.Draw(sprite, healthRect, healthColor);            
            spritebatch.End();
            //base.Draw(gameTime);
        }        
    }
}
