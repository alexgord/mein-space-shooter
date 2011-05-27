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
    class Bullet : GameObject
    {
        public Bullet(Game1 theGame, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(theGame, _content, viewPort, _sprite)
        {
            alive = false;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            if (!game.viewportRect.Contains(new Point((int)position.X, (int)position.Y)))
            {
                alive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            if (alive)
            {
                game.spriteBatch.Draw(sprite, position, null, Color.White, rotation, center, 1.0f, SpriteEffects.None, 0);
            }
            spritebatch.End();
            base.Draw(gameTime);
        }        
    }
}
