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

        public Enemy(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite, int _health) : base(_game, _content, viewPort, _sprite, _health)
        {
            delay = random.Next(1, ENEMY_DELAY);
            bullets = new GameObject[MAX_ENEMY_BULLETS];
            hasFired = false;
            for (int i = 0; i < MAX_ENEMY_BULLETS; i++)
            {
                bullets[i] = new Bullet(game, game.Content, game.viewportRect, game.Content.Load<Texture2D>("EnemyBullet"), 1);
                game.Components.Add(bullets[i]);
            }
            float randPos = random.Next(0, 800);
            float randSpeed = random.Next(1, 3);
            position = new Vector2(randPos, 10);
            velocity = new Vector2(0, randSpeed);
        }

        public override void Update(GameTime gameTime)
        {
             if (alive)
             {
                 position += velocity;
                 //if (!game.viewportRect.Contains(new Point((int)position.X,
                 //                                     (int)position.Y)))
                 //{
                 //    alive = false;
                 //}
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
                 //alive = true;
                 position = new Vector2(randPos, 100);
                 velocity = new Vector2(0, randSpeed);
             }

             UpdateBullets();
             base.Update(gameTime);
        }

        //public override void Draw(GameTime gameTime)
        //{
            //spritebatch.Begin();
            //spritebatch.Draw(sprite, position, null, Color.White, Rotation, center, 1.0f, SpriteEffects.None, 0);
            ////spritebatch.Draw(content.Load<Texture2D>("Ship"), spriteRectangle, Color.White);
            //spritebatch.End();
            //base.Draw(gameTime);
        //}

        public void UpdateBullets()
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.alive)
                {
                    bullet.position += bullet.velocity;
                    if (!game.viewportRect.Contains(new Point((int)bullet.position.X, (int)bullet.position.Y)))
                    {
                        bullet.alive = false;
                        continue;
                    }
                    Rectangle bulletRect = new Rectangle(
                         (int)bullet.position.X,
                         (int)bullet.position.Y,
                         bullet.sprite.Width,
                         bullet.sprite.Height);
                }
            }
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
