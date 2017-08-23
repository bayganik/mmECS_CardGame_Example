using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("StacksItems")]
public class StackContainer
{

    [XmlArray("DBRecords")]
    [XmlArrayItem("Stack")]

    public List<XmlStacks> stacks = new List<XmlStacks>();

    public static StackContainer Load(string assetFileName)
    {
        StackContainer stacksall = new StackContainer();
        //
        // Get the file name
        // 
        TextAsset _xml = Resources.Load<TextAsset>(assetFileName);

        XmlSerializer serializer = new XmlSerializer(typeof(StackContainer));
        //
        // Read strings from asset file name
        //
        StringReader reader = new StringReader(_xml.text);
        //
        // XML reader will read the entire data file and load it into ItemContainer
        //

        try
        {
            stacksall = serializer.Deserialize(reader) as StackContainer;

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        reader.Close();

        //return null;
        return stacksall;
    }
}
