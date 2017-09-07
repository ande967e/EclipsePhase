using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace EclipsePhase
{
    class GameObjectPool
    {
        public List<GameObject> AddActive { get; set; }
        public List<GameObject> RemoveActive { get; set; }
        public List<GameObject> CollisionForEnemies { get; set; }

        // Enemy

        //Projectile
        public List<GameObject> ActiveProjectileList { get; set; }
        private List<GameObject> InactiveProjectileList;
        //Clutter
        private List<GameObject> ActiveClutterList;
        private List<GameObject> InactiveClutterList;
        //Environment
        public List<GameObject> ActiveEnvironmentList { get; set; }
        private List<GameObject> InactiveEnvironmentList;

        public GameObject player;

        public GameObjectPool()
        {
            ActiveClutterList = new List<GameObject>();
            ActiveProjectileList = new List<GameObject>();
            ActiveEnvironmentList = new List<GameObject>();

            InactiveClutterList = new List<GameObject>();
            InactiveProjectileList = new List<GameObject>();
            InactiveEnvironmentList = new List<GameObject>();

            AddActive = new List<GameObject>();
            RemoveActive = new List<GameObject>();
        }

        public void CreateClutter(Vector2 clutter)
        {

        }

        /// <summary>
        /// Creates an environment at the given location, pos.
        /// </summary>
        /// <param name="towerPos"></param>
        public void CreateEnvironment(Vector2 pos, Vector2 scale)
        {
            if (InactiveEnvironmentList.Count > 0)
            {
                InactiveEnvironmentList[0].position = pos;
                AddActive.Add(InactiveEnvironmentList[0]);
                InactiveEnvironmentList.Remove(InactiveEnvironmentList[0]);
            }
            else
            {
                GameObject obj = new GameObject(pos);
                obj.AddComponnent(new SpriteRenderer(obj, "rectangle", scale, 0f, 1f));
                obj.GetComponent<SpriteRenderer>().Color = Color.Black;
                obj.AddComponnent(new Environment(obj));
                obj.LoadContent(GameWorld.Instance.Content);
                obj.AddComponnent(new CollisionRectangle(obj));
                obj.GetComponent<CollisionRectangle>().LoadContent(GameWorld.Instance.Content);
                AddActive.Add(obj);
            }
        }

        /// <summary>
        /// Creates a projectile at the given location, towerPos.
        /// </summary>
        /// <param name="towerPos"></param>
        public void CreateProjectile(Vector2 projectileStartPos, GameObject target)
        {

        }

        /// <summary>
        /// Creates a player.
        /// </summary>
        public void CreatePlayer(Vector2 pos)
        {
            GameObject obj = new GameObject(pos);
            obj.AddComponnent(new Player(obj, new Vector2(1, 0), 3, false));
            obj.AddComponnent(new SpriteRenderer(obj, "circle", 0.1f, 0f, 1f));
            obj.GetComponent<SpriteRenderer>().Color = Color.Black;
            obj.AddComponnent(new PlayerAim(obj));

            obj.AddComponnent(new Gravity(obj));
            obj.AddComponnent(new Collider(obj));
            obj.AddComponnent(new Translation(obj));

            obj.LoadContent(GameWorld.Instance.Content);
            obj.AddComponnent(new CollisionRectangle(obj));
            obj.GetComponent<CollisionRectangle>().LoadContent(GameWorld.Instance.Content);
            player = obj;
        }

        /// <summary>
        /// Updates all gameObjects in all the lists.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            for (int i = 0; i < ActiveClutterList.Count(); i++)
                ActiveClutterList[i].Update(gameTime);
            for (int i = 0; i < ActiveProjectileList.Count(); i++)
                ActiveProjectileList[i].Update(gameTime);
            for (int i = 0; i < ActiveEnvironmentList.Count(); i++)
                ActiveEnvironmentList[i].Update(gameTime);
        }

        /// <summary>
        /// Draws all gameObjects in all the lists.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            for (int i = 0; i < ActiveClutterList.Count(); i++)
                ActiveClutterList[i].Draw(spriteBatch);
            for (int i = 0; i < ActiveProjectileList.Count(); i++)
                ActiveProjectileList[i].Draw(spriteBatch);
            for (int i = 0; i < ActiveEnvironmentList.Count(); i++)
                ActiveEnvironmentList[i].Draw(spriteBatch);
        }

        /// <summary>
        /// Adds gameObjects from AddActive list to their correct active lists.
        /// </summary>
        public void AddToActive()
        {
            for (int i = 0; i < AddActive.Count(); i++)
            {
                GameObject go = AddActive[i];


                if (go.GetComponent<Clutter>() != null)
                {
                    ActiveClutterList.Add(go);
                    if (InactiveClutterList.Contains(go))
                        InactiveClutterList.Remove(go);
                }
                if (go.GetComponent<Projectile>() != null)
                {
                    ActiveProjectileList.Add(go);
                    if (InactiveProjectileList.Contains(go))
                        InactiveProjectileList.Remove(go);
                }
                if (go.GetComponent<Environment>() != null)
                {
                    ActiveEnvironmentList.Add(go);
                    if (InactiveEnvironmentList.Contains(go))
                        InactiveEnvironmentList.Remove(go);
                }
            }
            AddActive.Clear();
        }

        /// <summary>
        /// Moves gameObjects from RemoveActive lists to inactive lists  
        /// </summary>
        public void RemoveFromActive()
        {
            for (int i = 0; i < RemoveActive.Count(); i++)
            {
                GameObject go = RemoveActive[i];

                if (go.GetComponent<Clutter>() != null)
                {
                    InactiveClutterList.Add(go);
                    ActiveClutterList.Remove(go);
                }
                if (go.GetComponent<Projectile>() != null)
                {
                    InactiveProjectileList.Add(go);
                    ActiveProjectileList.Remove(go);
                }
                if (go.GetComponent<Environment>() != null)
                {
                    InactiveEnvironmentList.Add(go);
                    ActiveEnvironmentList.Remove(go);
                }
            }
            RemoveActive.Clear();
        }

        /// <summary>
        /// Returns a list of all GameObjects the player shall check collision with.
        /// </summary>
        /// <returns></returns>
        public List<GameObject> CollisionListForPlayer()
        {
            List<GameObject> list = new List<GameObject>();

            var allObjects = ActiveClutterList.Concat(ActiveEnvironmentList).ToList();
            return allObjects;
        }

        /// <summary>
        /// Updates the collision list for the enemies.
        /// </summary>
        /// <returns></returns>
        public void CollisionListForEnemy()
        {

        }

        /// <summary>
        /// Makes all active gameobjects inactive.
        /// </summary>
        public void ClearLists()
        {
            for (int i = 0; i < ActiveProjectileList.Count(); i++)
                RemoveActive.Add(ActiveProjectileList[i]);
            for (int i = 0; i < ActiveClutterList.Count(); i++)
                RemoveActive.Add(ActiveClutterList[i]);
            for (int i = 0; i < ActiveEnvironmentList.Count(); i++)
                RemoveActive.Add(ActiveEnvironmentList[i]);
        }
    }
}
