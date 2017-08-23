using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System;
using System.IO;
using UnityEngine;

public class CardStacks : StacksItems
{
     CardStacks _instance;
    public string FilePath { get; set; }   //file path to xml database file
    public StringReader StringRead { get; set; }
    private List<StacksItems> Items { get; set; }   //Row retrieved into a list
    private int RowCount { get; set; }   //Count rows in the table

    string tableName;

    public CardStacks()
    {

        Items = new List<StacksItems>();
        RowCount = 0;


    }
    public CardStacks(StringReader _sr)
    {
        StringRead = _sr;
        XmlParser xml = new XmlParser(StringRead);

        Items = new List<StacksItems>();
        RowCount = 0;
        Init();
    }
    public  void Init()
    {
        string temp;
        StacksItems rec = new StacksItems();

        XmlTextReader xRead = new XmlTextReader(StringRead);
        try
        {
            while (xRead.Read())
            {
                if (xRead.IsStartElement())
                {
                    switch (xRead.Name)
                    {
                        case "Stack":               //Start of a new row of data
                            if (string.IsNullOrEmpty(rec.StackID))
                                rec = new StacksItems();
                            else
                            {
                                Items.Add(rec);     //add previous record
                                rec = new StacksItems();
                            }
                            break;
                        case "StackID":           //element
                            if (xRead.Read())
                                rec.StackID = xRead.Value.Trim();      //element value
                            break;
                        case "Name":           //element
                            if (xRead.Read())
                                rec.Name = xRead.Value.Trim();      //element value
                            break;
                        case "FaceUp":           //element
                            if (xRead.Read())
                                rec.FaceUp = xRead.Value.Trim();      //element value
                            break;
                        case "Hidden1":           //element
                            if (xRead.Read())
                                rec.Hidden1 = xRead.Value.Trim();      //element value
                            break;
                        case "Hidden2":           //element
                            if (xRead.Read())
                                rec.Hidden2 = xRead.Value.Trim();      //element value
                            break;
                        case "Xcorr":           //element
                            if (xRead.Read())
                                rec.Xcorr = xRead.Value.Trim();      //element value
                            break;
                        case "Ycorr":           //element
                            if (xRead.Read())
                                rec.Ycorr = xRead.Value.Trim();      //element value
                            break;
                        case "TotalCards":           //element
                            if (xRead.Read())
                                rec.TotalCards = xRead.Value.Trim();      //element value
                            break;
                        case "LastCardFaceUp":           //element
                            if (xRead.Read())
                                rec.LastCardfaceUp = xRead.Value.Trim();      //element value
                            break;
                        case "Xoffset":           //element
                            if (xRead.Read())
                                rec.Xoffset = xRead.Value.Trim();      //element value
                            break;
                        case "Yoffset":           //element
                            if (xRead.Read())
                                rec.Yoffset = xRead.Value.Trim();      //element value
                            break;
                    }
                }
                else
                    Debug.Log("here");

            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        //
        // Do we have a last record to add?
        //
        if (string.IsNullOrEmpty(rec.StackID))
            Debug.Log("empty stacks.");
        else
            Items.Add(rec);

        RowCount = Items.Count;

    }


    public List<StacksItems> Search(string _stackID)
    {
        List<StacksItems> ret = new List<StacksItems>();

        foreach (StacksItems _dr in Items)
        {
            if (_dr.StackID == _stackID)
                ret.Add(_dr);
        }

        return ret;
    }

}
public class StacksItems
{
    public System.String StackID { get; set; }          //stack id
    public System.String Name { get; set; }             //name of stack of cards
    public System.String FaceUp { get; set; }           //Are all cards in stack face up?
    public System.String Hidden1 { get; set; }
    public System.String Hidden2 { get; set; }
    public System.String Xcorr { get; set; }            //X location
    public System.String Ycorr { get; set; }            //Y location
    public System.String TotalCards { get; set; }       //total cards in this stack during initial deal
    public System.String LastCardfaceUp { get; set; }   //is last card face up after initial deal
    public System.String Xoffset { get; set; }            //X offset to pan cards (left or right)
    public System.String Yoffset { get; set; }            //Y offset to pan cards (up or down)

    public System.Int32 ColCount { get; set; }
    public List<string> ColName = new List<string>();
    public List<Type> ColType = new List<Type>();

    public StacksItems()
    {
        //ColName = new List<string>();
        //ColType = new List<string>();
        StackID = "";
        ColName.Add("StackID");
        ColType.Add(typeof(string));
        Name = "";
        ColName.Add("Name");
        ColType.Add(typeof(string));
        FaceUp = "";
        ColName.Add("FaceUp");
        ColType.Add(typeof(string));
        Hidden1 = "";
        ColName.Add("Hidden1");
        ColType.Add(typeof(string));
        Hidden2 = "";
        ColName.Add("Hidden2");
        ColType.Add(typeof(string));
        Xcorr = "";
        ColName.Add("Xcorr");
        ColType.Add(typeof(string));
        Ycorr = "";
        ColName.Add("Ycorr");
        ColType.Add(typeof(string));
        TotalCards = "";
        ColName.Add("TotalCards");
        ColType.Add(typeof(string));
        LastCardfaceUp = "";
        ColName.Add("LastCardfaceUp");
        ColType.Add(typeof(string));

        ColCount = 9;
    }
}
