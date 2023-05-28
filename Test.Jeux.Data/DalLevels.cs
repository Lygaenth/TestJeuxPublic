using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TestJeux.Data.Api;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace Test.Jeux.Data.Xml
{
	public class DALLevels : IDALLevels
    { 
        #region Consts
        private const string KeyShader = "Shader";
        private const string KeyMusic = "Music";
        private const string KeyMap = "Map";
        private const string KeyZones = "Zones";
        private const string KeyGround = "Ground";
        private const string KeyDecorations = "Decorations";
		private const string KeyDecoration = "Decoration";
		private const string KeyItems = "Items";
        private const string KeyId = "ID";
        private const string KeyCode = "Code";
        private const string KeyPosition = "Position";
        private const string KeyOrientation = "Orientation";
        private const string KeyState = "State";
        private const string KeyP1 = "P1";
        private const string KeyP2 = "P2";
        private const string KeyAngle = "Angle";
        private const string KeyGroundType = "GroundType";
        #endregion

        private string _path = @".\..\..\..\Ressource\LevelDescriptor";

        public DALLevels()
        {

        }

        /// <summary>
        /// Load all levels
        /// </summary>
        /// <returns></returns>
        public List<LevelDto> LoadAllLevels()
        {
            var dtos = new List<LevelDto>();
			var levels = Directory.GetFiles(GetResourcePath(), "Level*.xml");
            foreach(var level in levels)
            {
                var dto = new LevelDto();
                dto.ID = Convert.ToInt32(Path.GetFileNameWithoutExtension(level).Split("Level")[1]);
                dtos.Add(dto);
            }
            return dtos;
        }

        private string GetResourcePath()
        {
			var path = Path.GetFullPath(@".\");
			var rootIndex = path.IndexOf("Game1Demo");
			return path.Substring(0, rootIndex) + @"Game1Demo\Ressource\LevelDescriptor";
		}

		/// <summary>
		/// Get data by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public LevelDto GetDataById(int id)
        {
            var leveldto = new LevelDto();
            leveldto.ID = id;
            var resourcePath = GetResourcePath();
			var path = Path.GetFullPath(resourcePath + "\\Level" + id + ".xml");

            if (!File.Exists(path))
                return new LevelDto() { ID = -1 };

            var doc = new XmlDocument();
            doc.Load(path);
            leveldto.Shader = (ShaderType)Convert.ToInt32(GetNodeValue(doc, KeyShader));
            var musicNode = GetNodeValue(doc, KeyMusic);
			leveldto.LevelMusic = musicNode == null ? Musics.None : (Musics)Convert.ToInt32(musicNode);

            // zones
            foreach (XmlNode zone in GetChildNodes(doc, KeyZones))
                leveldto.Zones.Add(GetZoneModel(zone));

            // Tiles
            var groundNode = GetNode(doc, KeyGround);
            if (groundNode != null)
            {
                var defaultTileNode = groundNode.Attributes["Default"];
				leveldto.DefaultTile = defaultTileNode == null ? 0 : Convert.ToInt32(defaultTileNode.Value);
                foreach (XmlNode item in groundNode.ChildNodes)
                    leveldto.TilesZones.Add(GetTileZone(item));
            }

            // Decorations
            foreach (XmlNode deco in GetChildNodes(doc, KeyDecorations))
                leveldto.Decorations.Add(GetDecoration(deco));

            // Items
            foreach (XmlNode item in GetChildNodes(doc, KeyItems))
                leveldto.Items.Add(GetItemModel(item));

            return leveldto;
        }

        /// <summary>
        /// Save current level settings into xml files
        /// </summary>
        /// <param name="levelDto"></param>
        public void SaveLevel(LevelDto levelDto)
        {
            if (levelDto.ID <= 0)
                levelDto.ID = GetFirstAvailableId();

            var path = GetCompletePath(levelDto.ID);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
    			Directory.CreateDirectory(Path.GetDirectoryName(path));

            var writer = new StreamWriter(path);

            var serializer = new XmlSerializer(typeof(LevelDto));
            serializer.Serialize(writer, levelDto);
            writer.Close();
        }

        /// <summary>
        /// Get complete file path for given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetCompletePath(int id)
        {
            return _path + "\\Level" + id + ".xml";
		}

        /// <summary>
        /// Get first available Id for new level
        /// </summary>
        /// <returns></returns>
        private int GetFirstAvailableId()
        {
			int i = 1;
			while (File.Exists(GetCompletePath(i)))
				i++;
            return i;
		}

        /// <summary>
        /// Get node value
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
		private string GetNodeValue(XmlDocument doc, string tag)
        {
            var node = doc.GetElementsByTagName(tag)[0];
            if (node != null)
                return (node.ChildNodes[0] as XmlText).Value;
            else
                return null;
        }

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private XmlNode GetNode(XmlDocument doc, string tag)
        {
            return doc.GetElementsByTagName(tag)[0];
        }

        /// <summary>
        /// Get child nodes
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private XmlNodeList GetChildNodes(XmlDocument doc, string tag)
        {
            return (doc.GetElementsByTagName(tag)[0] as XmlNode).ChildNodes;
        }

        /// <summary>
        /// Get zones
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ZoneDto GetZoneModel(XmlNode node)
        {
            var zone = new ZoneDto();
            foreach (XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case "GroundType":
                        zone.GroundType = (GroundType)Convert.ToInt32(att.Value);
                        break;
                    case "P1":
                        zone.TopLeft = ConvertToPoint(att.Value);
                        break;
                    case "P2":
                        zone.BottomRight = ConvertToPoint(att.Value);
                        break;
                    case "ID":
                        zone.ID = Convert.ToInt32(att.Value);
                        break;
                }
            }
            return zone;
        }

        /// <summary>
        /// Get tile zones
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private TileZoneDto GetTileZone(XmlNode node)
        {
            var tileZone = new TileZoneDto();
            foreach(XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case KeyId:
                        tileZone.ID = Convert.ToInt32(att.Value);
                        break;
                    case KeyGroundType:
                        tileZone.Tile = (GroundSprite)Convert.ToInt32(att.Value);
                        break;
                    case KeyP1:
                        tileZone.TopLeft = ConvertToPoint(att.Value);
                        break;
                    case KeyP2:
                        tileZone.BottomRight = ConvertToPoint(att.Value);
                        break;
                    case KeyAngle:
                        tileZone.Angle = 90 * Convert.ToInt32(att.Value);
                        break;
                }
            }
            return tileZone;
        }

        /// <summary>
        /// get decorations
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private DecorationDto GetDecoration(XmlNode node)
        {
            var decoration = new DecorationDto();
            foreach (XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case KeyId:
                        decoration.ID = Convert.ToInt32(att.Value);
                        break;
                    case KeyDecoration:
                        decoration.Decoration = (Decorations)Convert.ToInt32(att.Value);
                        break;
                    case KeyP1:
                        decoration.TopLeft = ConvertToPoint(att.Value);
                        break;
                    case KeyAngle:
                        decoration.Angle = 90 * Convert.ToInt32(att.Value);
                        break;
				}
            }
            return decoration;
        }

        /// <summary>
        /// Get items
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private ItemDto GetItemModel(XmlNode node)
        {
            var item = new ItemDto();
            foreach (XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case KeyId:
                        item.ID = Convert.ToInt32(att.Value);
                        break;
                    case KeyCode:
                        item.Code = (ItemCode)Convert.ToInt32(att.Value);
                        break;
                    case KeyPosition:
                        item.StartPosition = ConvertToPoint(att.Value);
                        break;
                    case KeyOrientation:
                        item.Orientation = (DirectionEnum)Convert.ToInt32(att.Value);
                        break;
                    case KeyState:
                        item.DefaultState = Convert.ToInt32(att.Value);
                        break;
                }
            }
            return item;
        }

        /// <summary>
        /// Convert string to point
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Point ConvertToPoint(string value)
        {
            var coord = value.Split(';');
            return new Point(Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1]));
        }
    }
}
