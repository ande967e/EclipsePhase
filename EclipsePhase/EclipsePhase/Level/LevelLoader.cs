using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using EclipsePhase.Level.Internal;

namespace EclipsePhase.Level
{
    public static class LevelLoader
    {
        private static XmlDocument xmd;

        static LevelLoader()
        {
            //Loads the XmlDocument
            xmd = new XmlDocument();
            xmd.Load("Content\\Levels.xml");
        }

        public static void LoadLevel(string Level)
        {
            //Selects the specified level in the document
            XmlNode xn = xmd.SelectSingleNode("head").SelectSingleNode(Level);

            //Loops through every single piece of information regarding the level, and loads it.
            foreach(XmlNode child in xn.ChildNodes)
            {
                string[] temp;

                switch (child.Name)
                {
                    case "PlayerSpawn":
                        //Converts the text to a Vector2, and places a Player on those coordinates
                        temp = child.InnerText.Split(':');
                        GameWorld.Instance.gameObjectPool.CreatePlayer(new Vector2(int.Parse(temp[(int)PlayerIndexes.PositionX]), int.Parse(temp[(int)PlayerIndexes.PositionY])));
                        break;
                    case "Platform":
                        //Splits all the info about the platform into an array
                        temp = child.InnerText.Split(':');
                        //Uses the ScaleX to calculate the scale of the object.
                        float scale = float.Parse(temp[(int)PlatformEnum.ScaleX]) / 400;

                        //Spawns the object on the coordinates matching it, Converts the coordinates from strings at the needed indexes
                        GameWorld.Instance.gameObjectPool.CreateEnvironment(new Vector2(int.Parse(temp[(int)PlatformEnum.PositionX]), int.Parse(temp[(int)PlatformEnum.PositionY])), scale);
                        break;
                }
            }
        }
    }
}
