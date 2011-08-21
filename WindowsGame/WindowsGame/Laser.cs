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
        public Boolean canFire;
        public Laser(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite) : base(_game, _content, viewPort, _sprite, false, 1)
        {
            lived = 0;
            destPoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            laserRect = new Rectangle(0, 0, 1, 100);
            laserLength = 0;
            life = 50;
            canFire = true;
            alive = false;
        }

        public override void Update(float elapsedTime)
        {
            Rotation = (float)Math.Atan2((double)startPoint.Y - (double)Mouse.GetState().Y, (double)startPoint.X - (double)Mouse.GetState().X);//((float)Math.Atan2((double)destPoint.Y, (double)destPoint.X)) - (float)Math.PI / 2;
            Rotation += (float)Math.PI / 2;
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            if (alive)
            {
                spritebatch.Draw(sprite, laserRect, null, laserColor, Rotation, new Vector2(0, 0), SpriteEffects.None, layer);
            }
            //spriteBatch.End();
        }        
    }
}
