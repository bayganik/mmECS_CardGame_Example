using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Item
{
    [XmlElement("Name")]
    public string item;

    //[XmlAttribute("name")]
    //public string name;

    [XmlElement("Damage")]
    public float damage;

    [XmlElement("Durability")]
    public float durability;

}
