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
    public class Enemy : DestructibleGameObject
    {
        const int MAX_ENEMY_BULLETS = 10;
        public int ENEMY_DELAY = 20;
        public GameObject[] bullets;
        public int delay;
        public bool hasFired;
        Random random = new Random();

        public Enemy(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite, int _health) : base(_game, _content, viewPort, _sprite, _health, true, true, 1)
        {
            delay = game.random.Next(1, ENEMY_DELAY);
            bullets = new GameObject[MAX_ENEMY_BULLETS];
            hasFired = false;
            for (int i = 0; i < MAX_ENEMY_BULLETS; i++)
            {
                bullets[i] = new Bullet(game, game.Content, game.viewportRect, game.Content.Load<Texture2D>("EnemyBullet"), 1);
                game.objectManager.Add(bullets[i]);
            }
            float randPos = game.random.Next(0, 800);
            float randSpeed = game.random.Next(1, 3);
            position = new Vector2(randPos, 10);
            velocity = new Vector2(0, randSpeed);
        }

        public override void Update(float elapsedTime)
        {
             if (alive)
             {
                 position += velocity;
                 if (delay == 0)
                 {
                     if (!hasFired)
                     {
                         FireBullet();
                         delay = ENEMY_DELAY;
                         hasFired = true;
                     }
                     else
                     {
                         hasFired = false;
                     }
                     delay = ENEMY_DELAY;
                 }
                 delay -= 1;
             }
             else
             {
                 float randPos = random.Next(0, 800);
                 float randSpeed = random.Next(1, 3);
                 position = new Vector2(randPos, 0);
                 velocity = new Vector2(0, randSpeed);
             }

             base.Update(elapsedTime);
        }

        public void FireBullet()
        {
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = position; //- center;//+ new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));

                    bullet.velocity = new Vector2((float)Math.Sin(Rotation),
                               (float)Math.Cos(Rotation)) * 5.0f;
                    bullet.Rotation = Rotation;
                    return;
                }
            }
        }
    }
}
