using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using EclipsePhase.Level.Internal;
using System.Globalization;

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
                        Vector2 scale = new Vector2(float.Parse(temp[(int)PlatformEnum.ScaleX]), float.Parse(temp[(int)PlatformEnum.ScaleY]));

                        //Color Preparation
                        Color overlayColor;
                        string[] tempColor = temp[(int)PlatformEnum.OverlayColor].Split('|');

                        //Converts the numbers in text, to a color
                        overlayColor = new Color(int.Parse(tempColor[0], NumberStyles.HexNumber), int.Parse(tempColor[1], NumberStyles.HexNumber), 
                            int.Parse(tempColor[2], NumberStyles.HexNumber), int.Parse(tempColor[3], NumberStyles.HexNumber));

                        //Getting the images SourceRectangle
                        string[] tempSourceRectangle = temp[(int)PlatformEnum.ImageSourceRectangle].Split('|');

                        //Spawns the object on the coordinates matching it, Converts the coordinates from strings at the needed indexes
                        GameWorld.Instance.gameObjectPool.CreateEnvironment(new Vector2(int.Parse(temp[(int)PlatformEnum.PositionX]), int.Parse(temp[(int)PlatformEnum.PositionY])), 1,
                            temp[(int)PlatformEnum.ImagePath], overlayColor,
                            new Rectangle(int.Parse(tempSourceRectangle[0]), int.Parse(tempSourceRectangle[1]), int.Parse(tempSourceRectangle[2]), int.Parse(tempSourceRectangle[3])), scale);
                        break;
                }
            }
        }
    }
}
