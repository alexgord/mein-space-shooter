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
    class ship : DestructibleGameObject
    {
        private const float scale = 0.25f;
        const int MAX_BULLETS = 10;
        GameObject mainGun;
        public Bullet[] bullets
        {
            get;
            set;
        }
        
        KeyboardState previousKeyboardState = Keyboard.GetState();
        public Laser laser;
        public ship(Game1 _game, ContentManager _content, Rectangle viewPort, Texture2D _sprite, int _health) : base(_game, _content, viewPort, _sprite, _health, false, false)
        {
            //this.position = new Vector2(200f,100f);
            //this.velocity = new Vector2(3f, 0f);
            laser = new Laser(game, content, viewPort, content.Load<Texture2D>("pixel"));
            game.Components.Add(laser);
            laser.laserColor = Color.Firebrick;
            mainGun = new GameObject(game, content, viewPort, content.Load<Texture2D>("MainGun"), false);
            game.Components.Add(mainGun);
            bullets = new Bullet[MAX_BULLETS];
            for (int count = 0; count < MAX_BULLETS; count++)
            {
                bullets[count] = new Bullet(game, game.Content, game.viewportRect, game.Content.Load<Texture2D>("Bullet"), 1);
                game.Components.Add(bullets[count]);
            }
            position = new Vector2(120, 280);
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
           
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (game.viewportRect.Contains(new Rectangle((int)position.X + (int)Math.Sin(Rotation),
                    (int)position.Y + (int)Math.Cos(Rotation) * -1, 0, 0)))
                {
                    velocity += new Vector2((float)Math.Sin(Rotation),
                                        (float)Math.Cos(Rotation) * -1) * 0.3f;

                }
            }
            velocity.X = MathHelper.Clamp(velocity.X, -4, 4);
            velocity.Y = MathHelper.Clamp(velocity.Y, -4, 4);
            position += velocity;

            if (!game.viewportRect.Contains(new Point((int)position.X, (int)position.Y)))
            {
                velocity.Y *= -0.5f;
                velocity.X *= -0.5f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                //ship.position.Y += ship.velocity.Y;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                Rotation += 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Rotation -= 0.1f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
            {
                FireBullet();
            }

            
            position.X = MathHelper.Clamp(position.X, 0, game.viewportRect.Right);
            position.Y = MathHelper.Clamp(position.Y, 0, game.viewportRect.Bottom);
            previousKeyboardState = keyboardState;
            mainGun.position = new Vector2((float)Math.Sin(Rotation),
                                        (float)Math.Cos(Rotation) * -1) * 20.0f;
            mainGun.position += position;
            AimMainGun();
            //base.Update(gameTime);
        }

        public void FireLaser()
        {
            laser.startPoint = position + new Vector2((float)Math.Sin(Rotation),
                               (float)Math.Cos(Rotation) * -1) * (sprite.Height / 2);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //if (laser.lived < laser.life)
                //{
                    laser.alive = true;
                    //laser.lived++;
               //}
                //else
                //{
                    //laser.alive = false;
                    //laser.laserLength = 0;
                //}
            }
            else
            {
                laser.alive = false;
                laser.lived = 0;
                laser.laserLength = 0;
            }
        }

        void AimMainGun()
        {
            foreach (Enemy enemy in game.enemies)
            {
                if (enemy.alive)
                {
                    mainGun.Rotation = (float)Math.Atan2(mainGun.position.Y - (double)enemy.position.Y, mainGun.position.X - (double)enemy.position.X);
                    //mainGun.Rotation = (float)Math.Atan2(mainGun.position.Y - (double)Mouse.GetState().Y, mainGun.position.X - (double)Mouse.GetState().X);
                    mainGun.Rotation -= (float)Math.PI / 2;
                    //if ((rotation - mainGun.rotation) / (180 / Math.PI) < - 10 / (180 / Math.PI) || (rotation - mainGun.rotation) / (180 / Math.PI) > 10 / (180 / Math.PI))
                    if (!((mainGun.Rotation - Rotation) > -0.3 && (mainGun.Rotation - Rotation) < 0.3))
                    {
                        mainGun.Rotation = Rotation;
                    }
                   else
                   {
                        break;
                   }                    
                }

            }
        }

        void FireBullet()
        {
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.position = mainGun.position + new Vector2((float)Math.Sin(Rotation),
                               (float)Math.Cos(Rotation) * -1) * 15;
                    bullet.velocity = new Vector2((float)Math.Sin(mainGun.Rotation),
                               (float)Math.Cos(mainGun.Rotation) * -1) * 10.0f;
                    bullet.Rotation = mainGun.Rotation;
                    return;
                }
            }
        }
    }
}
