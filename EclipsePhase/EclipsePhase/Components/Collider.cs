using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EclipsePhase
{
    //This is to be added to an object after all other objects that can affect the objects location, bu before the component Translation.

    class Collider : Component, IUpdateable
    {
        Vector2 push;
        Vector2 combinedPush;
        Vector2 transVec;

        public Collider(GameObject obj) : base(obj)
        {

        }

        public void Update(GameTime gameTime)
        {
            push = Vector2.Zero;
            combinedPush = Vector2.Zero;
            transVec = Vector2.Zero;

            transVec = obj.GetComponent<Translation>().translationVec;
            Collision();
            obj.GetComponent<Translation>().translationVec = transVec;
        }

        /// <summary>
        /// Calculates the pushvector.
        /// </summary>
        private void Collision()
        {
            //Checks for collision and adds to the final pushvector if nessecary
            for (int i = 0; i < GameWorld.Instance.gameObjectPool.ActiveEnvironmentList.Count; i++)
            {
                if (GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].GetComponent<Environment>() != null)
                {
                    //Places the object ontop the environment
                    push = CollisionCheck.CheckV2(obj.GetComponent<CollisionRectangle>().Edges, obj.position + transVec,
                        GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].GetComponent<CollisionRectangle>().Edges, 
                        GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].position);
                    transVec += push;
                    //Used for checks
                    combinedPush += push;
                }
            }
            //Checks if the final push vector is larger than 0
            if (obj.GetComponent<Gravity>() != null && combinedPush.Length() > 0 && combinedPush.Y < 0)
            {
                obj.GetComponent<Gravity>().OnGround = true;
            }
            else if (obj.GetComponent<Gravity>() != null)
            {
                obj.GetComponent<Gravity>().OnGround = false;
            }
        }
    }
}
