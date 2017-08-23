using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StacksLoader : MonoBehaviour
{

    public const string path = "StacksDB";

    // Use this for initialization
    void Start()
    {
        StackContainer ic = StackContainer.Load(path);

        foreach (XmlStacks stack in ic.stacks)
        {
            print(stack.Name);

        }
    }
}
