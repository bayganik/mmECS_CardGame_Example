using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("ItemCollection")]
public class ItemContainer
{

    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<Item> items = new List<Item>();

    public static ItemContainer Load(string assetFileName)
    {
        ItemContainer itemsall = new ItemContainer();
        //
        // Get the file name
        // 
        TextAsset _xml = Resources.Load<TextAsset>(assetFileName);

        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));
        //
        // Read strings from asset file name
        //
        StringReader reader = new StringReader(_xml.text);
        //
        // XML reader will read the entire data file and load it into ItemContainer
        //
        try
        { 
            itemsall = serializer.Deserialize(reader) as ItemContainer;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

         reader.Close();

        return itemsall;
    }
}
