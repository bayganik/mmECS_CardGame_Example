using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class XmlStacks
{
    [XmlElement("StackID")]
    public string StackID { get; set; }          //stack id
    [XmlElement("Name")]
    public string Name { get; set; }             //name of stack of cards
    [XmlElement("FaceUp")]
    public int FaceUp { get; set; }           //Are all cards in stack face up?
    [XmlElement("Hidden1")]
    public int Hidden1 { get; set; }
    [XmlElement("Hidden2")]
    public int Hidden2 { get; set; }
    [XmlElement("Xcorr")]
    public float Xcorr { get; set; }            //X location
    [XmlElement("Ycorr")]
    public float Ycorr { get; set; }            //Y location
    [XmlElement("TotalCards")]
    public int TotalCards { get; set; }       //total cards in this stack during initial deal
    [XmlElement("LastCardfaceUp")]
    public int LastCardfaceUp { get; set; }   //is last card face up after initial deal
    [XmlElement("Xoffset")]
    public float Xoffset { get; set; }            //X offset to pan cards (left or right)
    [XmlElement("Yoffset")]
    public float Yoffset { get; set; }            //Y offset to pan cards (up or down)
}

