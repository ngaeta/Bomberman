using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using OpenTK;
using System.IO;

namespace Bomberman
{
    static class GfxManager
    {
        private static Dictionary<string, Texture> textures;
        private static Dictionary<string, Tuple<Texture, List<Animation>>> spritesheets;

        public struct ColorRGB
        {
            public byte Red;
            public byte Green;
            public byte Blue;

            public ColorRGB(byte r, byte g, byte b)
            {
                Red = r;
                Green = g;
                Blue = b;
            }
        }

        static GfxManager()
        {
            textures = new Dictionary<string, Texture>();
            spritesheets = new Dictionary<string, Tuple<Texture, List<Animation>>>();
        }

        public static bool AddTexture(string key, string textureName)
        {
            if (!textures.ContainsKey(key))
            {
                textures.Add(key, new Texture(textureName));
                return true;
            }
            return false;
        }

        public static void RemoveAll()
        {
            textures.Clear();
            spritesheets.Clear();
        }

        public static Texture GetTexture(string textureName)
        {
            if (textures.ContainsKey(textureName))
            {
                return textures[textureName];
            }

            return null;
        }

        public static void AddSpritesheet(string name, Texture t, List<Animation> a)
        {
            if (!spritesheets.ContainsKey(name))
                spritesheets.Add(name, new Tuple<Texture, List<Animation>>(t, a));
        }

        public static Tuple<Texture, List<Animation>> GetSpritesheet(string key)
        {
            if (spritesheets.ContainsKey(key))
            {
                Tuple<Texture, List<Animation>> tupleToCopy = spritesheets[key];
                List<Animation> cloneList = new List<Animation>();

                for (int i = 0; i < tupleToCopy.Item2.Count; i++)
                {
                    cloneList.Add((Animation)tupleToCopy.Item2[i].Clone());
                }

                return new Tuple<Texture, List<Animation>>(tupleToCopy.Item1, cloneList);
            }

            return null;
        }

        private static Animation LoadAnimation(
            XmlNode animationNode, int width, int height)
        {
            XmlNode currNode = animationNode.FirstChild;
            bool loop = bool.Parse(currNode.InnerText);

            currNode = currNode.NextSibling;
            float fps = float.Parse(currNode.InnerText);

            currNode = currNode.NextSibling;
            int rows = int.Parse(currNode.InnerText);

            currNode = currNode.NextSibling;
            int cols = int.Parse(currNode.InnerText);

            currNode = currNode.NextSibling;
            int startX = int.Parse(currNode.InnerText);

            currNode = currNode.NextSibling;
            int startY = int.Parse(currNode.InnerText);

            return new Animation(width, height, cols, rows, fps, loop, startX, startY);
        }

        public static void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Assets/SpriteSheetConfig.xml");

            XmlNode root = doc.DocumentElement;

            foreach (XmlNode spritesheetNode in root.ChildNodes)
            {
                if (spritesheetNode.NodeType != XmlNodeType.Comment)
                    LoadSpritesheet(spritesheetNode);
            }
        }

        private static void LoadSpritesheet(XmlNode spritesheetNode)
        {
            XmlNode nameNode = spritesheetNode.FirstChild;

            string name = nameNode.InnerText;
            XmlNode filenameNode = nameNode.NextSibling;
            Texture texteure = new Texture(filenameNode.InnerText);
            XmlNode frameNode = filenameNode.NextSibling;

            List<Animation> animations = new List<Animation>();

            if (frameNode.HasChildNodes)
            {
                int width = int.Parse(frameNode.FirstChild.InnerText);
                int height = int.Parse(frameNode.LastChild.InnerText);
                XmlNode animationsNode = frameNode.NextSibling;

                foreach (XmlNode animation in animationsNode)
                {
                    if (animation.NodeType != XmlNodeType.Comment)
                    {
                        animations.Add(LoadAnimation(
                                animation, width, height));
                    }
                }
            }
            else
            {
                animations.Add(new Animation(texteure.Width, texteure.Height));
            }

            AddSpritesheet(name, texteure, animations);
        }

        // prendere tutti i nodi image
        // estrarre l'attributo source
        // creare la texture a partire da source
        // creare la spritesheet utilizzando source (come nome ) e la texture creata
        // aggiungere source alla lista da ritornare
        public static List<string> LoadTileSet(string filename, out Dictionary<int, XmlNode> dictProperties)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            List<string> tileNames = new List<string>();

            // Get all nodes with name == "image"
            XmlNodeList imageNodes = doc.SelectNodes("//image");

            for (int i = 0; i < imageNodes.Count; i++)
            {
                string source = imageNodes[i].Attributes["source"].Value;
                tileNames.Add(Path.GetFileNameWithoutExtension(source));
            }

            XmlNodeList tileNodes = doc.SelectNodes("//tile");
            dictProperties = new Dictionary<int, XmlNode>();

            foreach (XmlNode tile in tileNodes)
            {
                if (tile.FirstChild.Name.Equals("properties"))
                {
                    dictProperties.Add(int.Parse(tile.Attributes["id"].Value), tile.FirstChild);
                }
            }

            return tileNames;
        }   

        private static Vector2 GetVectorFromXMlNode(XmlNode nodeX, XmlNode nodeY)
        {
            Vector2 vector = Vector2.Zero;

            if (nodeX != null)
                vector.X = int.Parse(nodeX.Attributes["value"].Value);

            if (nodeY != null)
                vector.Y = int.Parse(nodeY.Attributes["value"].Value);

            return vector;
        }


        private static XmlNode SearchNodeWithAttribute(ref XmlNodeList list, string attributesName, string valueName = "")
        {
            foreach (XmlNode node in list)
            {
                if (valueName == "")
                {
                    if (node.Attributes[attributesName] != null)
                    {
                        return node;
                    }
                }

                else if (node.Attributes[attributesName] != null && node.Attributes[attributesName].Value.Equals(valueName))
                {
                    return node;
                }
            }

            return null;
        }
    }
}

