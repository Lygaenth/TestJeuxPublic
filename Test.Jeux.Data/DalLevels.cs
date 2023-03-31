using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Test.Jeux.Data.Api;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace Test.Jeux.Data
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
        private const string KeyItems = "Items";
        private const string KeyId = "ID";
        private const string KeyCode = "Code";
        private const string KeyPosition = "Position";
        private const string KeyOrientation = "Orientation";
        private const string KeyState = "State";
        private const string KeyP1 = "P1";
        private const string KeyP2 = "P2";
        private const string KeyAngle = "Angle";
        #endregion

        private string _path = @".\Ressource\LevelDescriptor";

        public DALLevels()
        {

        }

        public List<LevelDto> LoadAllLevels()
        {
            var dtos = new List<LevelDto>();

            var levels = GetLevelFiles();
            foreach(var level in levels)
            {
                var dto = new LevelDto();
                dto.ID = Convert.ToInt32(Path.GetFileNameWithoutExtension(level).Split("Level")[1]);
                dtos.Add(dto);
            }
            return dtos;
        }

        public LevelDto GetDataById(int id)
        {
            var leveldto = new LevelDto();
            leveldto.ID = id;
            var path = _path + "\\Level" + id + ".xml";

            if (!File.Exists(Path.GetFullPath(path)))
                return new LevelDto() { ID = -1 };

            var doc = new XmlDocument();
            doc.Load(path);
            leveldto.Shader = (ShaderType)Convert.ToInt32(GetNodeValue(doc, KeyShader));
            leveldto.LevelMusic = (Musics)Convert.ToInt32(GetNodeValue(doc, KeyMusic));

            // zones
            foreach (XmlNode zone in GetChildNodes(doc, KeyZones))
                leveldto.Zones.Add(GetZoneModel(zone));

            // Tiles
            var groundNode = GetNode(doc, KeyGround);
            if (groundNode != null)
            {
                leveldto.DefaultTile = Convert.ToInt32(groundNode.Attributes["Default"].Value);
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
            var path = _path +"\\Export\\"+ "\\Level" + levelDto.ID + ".xml";
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            var writer = new StreamWriter(path);

            var serializer = new XmlSerializer(typeof(LevelDto));
            serializer.Serialize(writer, levelDto);
            writer.Close();
        }

        private string GetNodeValue(XmlDocument doc, string tag)
        {
            var node = doc.GetElementsByTagName(tag)[0];
            return (node.ChildNodes[0] as XmlText).Value;
        }

        private XmlNode GetNode(XmlDocument doc, string tag)
        {
            return doc.GetElementsByTagName(tag)[0];
        }

        private XmlNodeList GetChildNodes(XmlDocument doc, string tag)
        {
            return (doc.GetElementsByTagName(tag)[0] as XmlNode).ChildNodes;
        }

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

        private TileZoneDto GetTileZone(XmlNode node)
        {
            var tileZone = new TileZoneDto();
            foreach(XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case KeyId:
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

        private DecorationDto GetDecoration(XmlNode node)
        {
            var decoration = new DecorationDto();
            foreach (XmlAttribute att in node.Attributes)
            {
                switch (att.Name)
                {
                    case KeyId:
                        decoration.Decoration = (Decorations)Convert.ToInt32(att.Value);
                        break;
                    case KeyP1:
                        decoration.TopLeft = ConvertToPoint(att.Value);
                        break;
                }
            }
            return decoration;
        }

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

        private Point ConvertToPoint(string value)
        {
            var coord = value.Split(';');
            return new Point(Convert.ToInt32(coord[0]), Convert.ToInt32(coord[1]));
        }
    
        private string[] GetLevelFiles()
        {
            if (Directory.Exists(Path.GetFullPath(_path)))
    			return Directory.GetFiles(Path.GetFullPath(_path), "Level*.xml");
            else
                return new string[0];
		}
    }
}
