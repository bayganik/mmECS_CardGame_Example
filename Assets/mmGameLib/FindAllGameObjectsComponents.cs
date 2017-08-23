using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAllGameObjectsComponents : MonoBehaviour
{
    List<Component> resultComponents = new List<Component>();
	// Use this for initialization
	void Start ()
    {
        GameObject[] GetRootGameObjects;
        GetRootGameObjects = System.Array.FindAll(GameObject.FindObjectsOfType<GameObject>(), (item) => item.transform.parent == null);


    }

    // Update is called once per frame
    void Update () {
		
	}
    GameObject[] GetRootGameObjects()
    {
        return System.Array.FindAll(GameObject.FindObjectsOfType<GameObject>(), (item) => item.transform.parent == null);
    }
    void FindComponent(string keyword)
    {
        foreach (var go in GetRootGameObjects())
        {
            FindComponent(go, keyword);
        }
    }

    void FindComponent(GameObject go, string keyword)
    {
        // Find component
        foreach (var component in go.GetComponents(typeof(Component)))
        {
            var componentName = component.GetType().ToString().ToUpper();
            if (0 < componentName.ToUpper().IndexOf(keyword))
            {
                resultComponents.Add(component);
            }
        }
        // Find child component
        for (int i = 0, count = go.transform.childCount; i < count; ++i)
        {
            FindComponent(go.transform.GetChild(i).gameObject, keyword);
        }
    }
}
