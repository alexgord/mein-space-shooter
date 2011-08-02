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
    class Laser : GameObject
    {
        public Rectangle laserRect;
        public Vector2 destPoint;
        public Vector2 startPoint;
        public int life;
        public int lived;
        public Color laserColor;
        public int laserLength;
        public Laser(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(_game, _content, viewPort, _sprite)
        {
            lived = 0;
            destPoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            laserRect = new Rectangle(0, 0, 1, 100);//(int)destPoint.Length());
            laserLength = 0;
            life = 50;
        }

        public override void Update(GameTime gameTime)
        {
            //life = 50;
            //destPoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Rotation = (float)Math.Atan2((double)startPoint.Y - (double)Mouse.GetState().Y, (double)startPoint.X - (double)Mouse.GetState().X);//((float)Math.Atan2((double)destPoint.Y, (double)destPoint.X)) - (float)Math.PI / 2;
            Rotation += (float)Math.PI / 2;
            //laserRect = new Rectangle((int)startPoint.X, (int)startPoint.Y, 1, (int)(startPoint - destPoint).Length());
            laserRect = new Rectangle((int)startPoint.X, (int)startPoint.Y, 1, laserLength);

            if (alive)
            {
                if (lived < life)
                {
                    lived++;
                    alive = true;
                }
                else
                {
                    alive = false;
                }
            }
            else
            {
                lived = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();

            if (alive)
            {
                spritebatch.Draw(sprite, laserRect, null, laserColor, Rotation, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            spritebatch.End();
            base.Draw(gameTime);
        }        
    }
}
