using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Constants

        const int MaxEnemies = 3;
        const int MaxStandardAsteroids = 5;

        #endregion

        public GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }

        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        Texture2D backgroundTexture;
        public Enemy[] enemies;
        public StandardAsteroid[] standardAsteroids;
        Random random = new Random();
        int score;
        public Rectangle viewportRect
        {
            get;
            private set;
        }
        KeyboardState previousKeyboardState = Keyboard.GetState();
        ship theShip;
        HealthBar healthBar;
        private int lives;
        Vector2 currLaserPoint;
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Graphics.PreferMultiSampling = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            viewportRect = new Rectangle(0, 0, Graphics.GraphicsDevice.Viewport.Width, Graphics.GraphicsDevice.Viewport.Height);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            score = 0;
            lives = 3;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            healthBar = new HealthBar(this, Content, viewportRect, Content.Load<Texture2D>("HealthBar"));
            Components.Add(healthBar);
            theShip = new ship(this, Content, viewportRect, Content.Load<Texture2D>("Ship"), 100);
            enemies = new Enemy[MaxEnemies];
            for (int i = 0; i < MaxEnemies; i++)
            {
                enemies[i] = new Enemy(this, Content, viewportRect, Content.Load<Texture2D>("Enemy"), 3);
                Components.Add(enemies[i]);
            }
            
            standardAsteroids = new StandardAsteroid[MaxStandardAsteroids];
            for (int i = 0; i < MaxStandardAsteroids; i++)
            {
                standardAsteroids[i] = new StandardAsteroid(this, Content, viewportRect, Content.Load<Texture2D>("StandardAsteroid"), 10);
                standardAsteroids[i].position.X = random.Next(viewportRect.Right);
                standardAsteroids[i].position.Y = random.Next(viewportRect.Bottom);
                Components.Add(standardAsteroids[i]);
            }
            Components.Add(theShip);
            // TODO: use this.Content to load your game content here
            foreach (StandardAsteroid sa in standardAsteroids)
            {
                foreach (StandardAsteroid sa2 in standardAsteroids)
                {
                    if (!sa.Equals(sa2))
                    {
                        while (checkCollision(sa, sa2))
                        {
                            sa.position.X = random.Next(viewportRect.Right);
                            sa.position.Y = random.Next(viewportRect.Bottom);
                        }
                    }
                }
            }
            backgroundTexture = Content.Load<Texture2D>("Background");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //foreach (Enemy enemy in enemies)
            //{
            //    foreach (StandardAsteroid sa in standardAsteroids)
            //    {
            //        if (enemy.alive && sa.alive)
            //        {
            //            if (checkCollision(enemy, sa))
            //            {
            //                sa.alive = false;
            //            }
            //        }
            //    }
            //}

            //Make sure the enemies never overlap
            for (int i = 0; i < MaxEnemies; i++)
            {
                for (int j = 0; j < MaxEnemies; j++)
                {
                    if (i != j)
                    {
                        while (checkCollision(enemies[j], enemies[i]))
                        {
                            enemies[j].position.X = random.Next(0, viewportRect.Right);
                        }
                    }
                }
            }

            //Check if you've shot any enemies
            foreach (Bullet bullet in theShip.bullets)
            {
                if (bullet.alive)
                {
                    
                    foreach (Enemy enemy in enemies)
                    {
                        if ( checkCollision(bullet, enemy) )
                        {
                            bullet.alive = false;
                            enemy.alive = false;
                            score++;
                            break;
                        }
                    }
                }
            }
                
            //Check if you've been shot by any enemies
            foreach (Enemy enemy in enemies)
            {
                foreach (Bullet bullet in enemy.bullets)
                {
                    if (bullet.alive)
                    {
                        if ( checkCollision(bullet, theShip) )
                        {
                            bullet.alive = false;
                            theShip.health--;
                            healthBar.health--;
                        }
                    }
                }
            }

            if (theShip.health <= 0)
            {
                lives--;
                theShip.health = 100;
            }

            //Check to see if you've shot any asteroids
            foreach (StandardAsteroid sa in standardAsteroids)
            {
                foreach (Bullet bullet in theShip.bullets)
                {
                    if (bullet.alive)
                    {
                        if (checkCollision(bullet, sa))
                        {
                            bullet.alive = false;
                            sa.alive = false;
                        }
                    }
                }
            }

            //Check if any asteroids have hit each other
            
            foreach (StandardAsteroid sa in standardAsteroids)
            {
                foreach (StandardAsteroid sa2 in standardAsteroids)
                {
                    if (!sa.Equals(sa2) && (!sa.hasTouched || !sa2.hasTouched))
                    {
                        if (checkCollision(sa, sa2) )
                        {
                            //make them drift apart
                            //First deal with their velocities
                            Vector2 ov = sa2.velocity;
                            sa2.velocity = sa2.velocity * -1 +  (sa.velocity/2);//sa2.velocity - sa.velocity * 0.5f;
                            sa.velocity = sa.velocity * -1 + (ov/2);
                            sa.hasTouched = true;
                            sa2.hasTouched = true;
                            //Then deal with making them rotate accordingly
                            sa.rotationVelocity += sa2.velocity.X * 0.01f + sa2.velocity.Y * 0.01f;
                            //sa.health--;
                            continue;
                        }
                    }
                }
            }
            foreach (StandardAsteroid sa in standardAsteroids)
            {
                sa.hasTouched = false;
            }
            foreach (StandardAsteroid sa in standardAsteroids)
            {
                foreach (StandardAsteroid sa2 in standardAsteroids)
                {
                    if (!sa.Equals(sa2))
                    {
                        if (checkCollision(sa, sa2))
                        {
                            sa.hasTouched = true;
                        }
                        else
                        {
                            //sa.hasTouched = false;
                        }
                    }
                }
            }    
            //Firing the Ship's laser!
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && theShip.laser.lived < theShip.laser.life) //If Imma Firin' mah lazors!
            {
                    
                Boolean hitSomething = false;
                theShip.laser.laserLength = 0;
                theShip.laser.alive = true;
                do
                {
                    theShip.laser.startPoint = theShip.position + new Vector2((float)Math.Sin(theShip.Rotation),
                            (float)Math.Cos(theShip.Rotation) * -1) * (theShip.sprite.Height / 2);
                    currLaserPoint = theShip.laser.startPoint + new Vector2((float)Math.Sin(theShip.laser.Rotation),
                                (float)Math.Cos(theShip.laser.Rotation) * -1) * (theShip.laser.laserLength) * -1;
                    GameObject pixel = new GameObject(this, Content, viewportRect, Content.Load<Texture2D>("pixel"));
                    pixel.position = currLaserPoint;
                    theShip.laser.laserLength++;

                    foreach (StandardAsteroid asteroid in standardAsteroids)
                    {
                        if ( checkCollision(pixel, asteroid))
                        {
                            asteroid.health--;
                            hitSomething = true;
                            break;
                        }
                    }
                    if (hitSomething)
                    {
                        break;
                    } 

                }
                while (viewportRect.Contains((int)currLaserPoint.X, (int)currLaserPoint.Y)) ;

                //if (laser.lived < laser.life)
                //{
                    
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
                theShip.laser.canFire = true;
                theShip.laser.alive = false;
                if (Mouse.GetState().LeftButton != ButtonState.Pressed)
                {
                    theShip.laser.lived = 0;
                }
                //theShip.laser.laserLength = 0;
                currLaserPoint = theShip.position;
            }
            base.Update(gameTime);
        }

        public Boolean checkCollision ( GameObject o1, GameObject o2 )
        {
                    o1.spriteTransform =
                    Matrix.CreateTranslation(new Vector3(-o1.center, 0.0f)) *
                        // Matrix.CreateScale(block.Scale) *  would go here
                    Matrix.CreateRotationZ(o1.Rotation) *
                    Matrix.CreateTranslation(new Vector3(o1.position, 0.0f));
                    o1.spriteRectangle = CalculateBoundingRectangle(
                        new Rectangle(0, 0, o1.sprite.Width, o1.sprite.Height),
                        o1.spriteTransform);
                    
                    o2.spriteTransform =
                    Matrix.CreateTranslation(new Vector3(-o2.center, 0.0f)) *
                            // Matrix.CreateScale(block.Scale) *  would go here
                    Matrix.CreateRotationZ(o2.Rotation) *
                    Matrix.CreateTranslation(new Vector3(o2.position, 0.0f));
                    o2.spriteRectangle = CalculateBoundingRectangle(
                        new Rectangle(0, 0, o2.sprite.Width, o2.sprite.Height),
                        o2.spriteTransform);
                        if (o1.spriteRectangle.Intersects(o2.spriteRectangle))
                        {
                            if (IntersectPixels(o1.spriteTransform, o1.sprite.Width,
                                               o1.sprite.Height, o1.spriteTextureData,
                                               o2.spriteTransform, o2.sprite.Width,
                                               o2.sprite.Height, o2.spriteTextureData))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
            return false;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            SpriteBatch.Begin();

            SpriteBatch.Draw(backgroundTexture, viewportRect, Color.White);
            SpriteBatch.DrawString(Content.Load<SpriteFont>(@"GameFont"), "Health: " + theShip.health, Vector2.Zero, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            SpriteBatch.DrawString(Content.Load<SpriteFont>(@"GameFont"), "Score: " + score, new Vector2(180, 0), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            SpriteBatch.DrawString(Content.Load<SpriteFont>(@"GameFont"), "Lives: ", new Vector2(320, 0), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            for (int i = 0; i < lives; i++)
            {
                Rectangle lifeRectangle = new Rectangle(400 + i * 30, 0, 30, 30);
                SpriteBatch.Draw(Content.Load<Texture2D>("Ship"),lifeRectangle, Color.White);
            }
            if ( theShip.laser.alive)
            {
            SpriteBatch.Draw(theShip.sprite, new Rectangle((int)currLaserPoint.X, (int)currLaserPoint.Y, 3, 3), Color.White);
                }
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                          Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
    }
}
