﻿using System;
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
    public class DestructibleGameObject : GameObject
    {
        public int health;
        int fullHealth;
        public DestructibleGameObject(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite, int _health) : base(_game, _content, viewPort, _sprite)
        {
            health = _health;
            fullHealth = health;
        }
        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                alive = false;
                health = fullHealth;
            }
            else
            {
                alive = true;
            }

            if (!alive)
            {
                health = fullHealth;
            }
            base.Update(gameTime);
        }

    }
}