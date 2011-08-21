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
    
    public class ObjectManager
    {
	    // The central SpriteBatch to help
	    // draw every game objects.
	    SpriteBatch spriteBatch;

	    // Holds all the gameObjects
	    List<GameObject> gameObjects = new List<GameObject>();

	    // A collection of textures.
	    Dictionary<string, Texture2D> textureCollection = new Dictionary<string, Texture2D>();

	    public List<GameObject> GameObjects
	    {
		    get { return gameObjects; }
	    }

	    public Dictionary<string, Texture2D> TextureCollection
	    {
		    get { return textureCollection; }
	    }

	    public ObjectManager(GraphicsDevice device)
	    {
		    spriteBatch = new SpriteBatch(device);
	    }

	    public void Load(ContentManager content)
	    {
		    // This implementation uses more of a
		    // "centralized" content management
		    // pipeline. Instead of letting each
		    // GameObject load texture, this
		    // this textureCollection object is
		    // going to preload all content and
		    // then expose them to each GameObject.
		    //textureCollection.Add("some-texture", content.Load<Texture2D>("texture\location"));
	    }

	    public void Update(float elapsedTime)
	    {
		    for (int i = 0, length = gameObjects.Count; i < length; i++)
		    {
			    gameObjects[i].Update(elapsedTime);
		    }
	    }

	    public void Draw( SpriteBatch spriteBatch)
	    {
		    for (int i = 0, length = gameObjects.Count; i < length; i++)
		    {
			    gameObjects[i].Draw(spriteBatch);
		    }
	    }

	    public void Add(GameObject someObject)
	    {
		    gameObjects.Add(someObject);
	    }

	    // And, go ahead, add more to this class
	    // if you want.
    }

}
