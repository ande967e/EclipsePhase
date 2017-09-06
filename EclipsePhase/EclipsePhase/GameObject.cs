using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace EclipsePhase
{
    public class GameObject
    {
        public List<Component> components;
        public Vector2 position;

        public GameObject(Vector2 position)
        {
            this.components = new List<Component>();
            this.position = position;
        }

        /// <summary>
        /// Adds a component to the list components.
        /// </summary>
        /// <param name="component"></param>
        public void AddComponnent(Component component)
        {
            components.Add(component);
        }

        /// <summary>
        /// Gets the first occuring component.
        /// </summary>
        /// <typeparam name="T">The type of the Component to find</typeparam>
        /// <returns>The component of specified type</returns>
        public T GetComponent<T>() where T : Component
            => components.Find(c => c is T) as T;

        /// <summary>
        /// Runs through all components and updates which are IUpdatable.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (Component c in components)
                if (c is IUpdateable)
                    (c as IUpdateable).Update(gameTime);
        }

        /// <summary>
        /// Runs through all components and loads their contetnt if ILoadable.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            foreach (Component c in components)
                if (c is Iloadable)
                    (c as Iloadable).LoadContent(content);
        }

        /// <summary>
        /// Runs through all components and draws which are IDrawable.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component c in components)
                if (c is IDrawable)
                    (c as IDrawable).Draw(spriteBatch);
        }
    }
}