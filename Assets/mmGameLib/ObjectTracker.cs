using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * Tracking all objects in the game by their components, used as an Entity, Component, System framework
 * All GameObjects are the entity.  I find the objects using their components which are saved in "allObjects" dictionary
 * 
 * The "retObjects" can have zero or more GameObjects returning to the called.
 * Every Component that wants to be part of this system will register itself with this db using:
 * 
 *  private void OnEnable()
 *  {
 *      //
 *      // Register component with ObjectTracker
 *      // When attached to a GameObject (entity)
 *      //
 *      ObjectTracker.Register(this.GetType(), this.gameObject);
 *  }
 */
public class ObjectTracker
{

    static Dictionary<GameObject, string> retObjects = new Dictionary<GameObject, string>();
    static ObjectTracker _instance;
    //
    // Database of components and their game objects we are tracking
    //   Type : Component (key)
    //   Dictionary<GameObjects, GameObjectName> : Data
    //
    private Dictionary<Type, Dictionary<GameObject, string>> allObjects = new Dictionary<Type, Dictionary<GameObject, string>>();
    public ObjectTracker()
    {
        if (_instance != null)
        {
            throw new CannotHaveTwoInstancesException();
        }
        _instance = this;
    }
    /// <summary>
    /// Getter for singelton instance.
    /// </summary>
    protected static ObjectTracker instance
    {
        get
        {
            if (_instance == null)
            {
                new ObjectTracker();
            }

            return _instance;
        }
    }
    public static void Clear()
    {
        instance.allObjects.Clear();
    }
    /// <summary>
    /// Register the specified service instance. Usually called in Awake(), like this:
    /// Set<ExampleService>(this);
    /// </summary>
    /// <param name="service">Service instance object.</param>
    /// <typeparam name="T">Type of the instance object.</typeparam>
    public static void Register(Type compType, GameObject go)
    {
        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //
        retObjects = FindComponent(compType);
        if (retObjects.Count > 0)
        {
            string objName;
            if (retObjects.TryGetValue(go, out objName))
            {
                return;                           //Object already exists (should never happen)
            }
        }
        //
        // Add the object to all the other objects (empty list if first time)
        //
        retObjects.Add(go, go.name); 
        //
        // an existing component that has game objects, just got a new set of game objects
        // remove the component, and re-add it with new game objects
        //
        instance.allObjects.Remove(compType);
        instance.allObjects.Add(compType, retObjects);
    }
    /// <summary>
    /// Remove a component and its game object from the collection 
    /// </summary>
    /// <param name="component"></param>
    /// <param name="gameObject"></param>
    public static void Remove(Type compType, GameObject go)
    {
        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //    1. Remove the GameObject associated with this component
        //    2. If the resulting Dictionary of GameObjects is empty
        //          Yes, remove the Component
        //          No , keep the Component and its remaining GameObjects
        //
        retObjects = FindComponent(compType);
        if (retObjects.Count > 0)
        {
            string objName;
            if (retObjects.TryGetValue(go, out objName))
            {
                retObjects.Remove(go);                   //Remove GameObject from the list        
            }
            else
            {
                return;                                 //GameObject not found, don't remove the component
            }
            //
            // Test to see, if anymore GameObjects left
            //
            if (retObjects.Count > 0)
                return;
            //
            // We have no more GameObjects, remove the Component
            //
            instance.allObjects.Remove(compType);

        }
    }
    private static Dictionary<GameObject, string> FindComponent(Type compType)
    {
        //
        // Lookup the Component type, does the key exist?
        //     Yes, return a dictionary of GameObjects that have this component
        //     No , return an empty dictionary
        //
        Dictionary<GameObject, string> goFound = new Dictionary<GameObject, string>();
        if (instance.allObjects.TryGetValue(compType, out goFound ))
        {
            return goFound;
        }
        return new Dictionary<GameObject, string>();
    }
    public static List<GameObject> Find<C1>() where C1 : Component
    {
        Type key;
        key = typeof(C1);
        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //
        retObjects = FindComponent(key);
        return retObjects.Keys.ToList();
    }
    /// <summary>
    /// Return a distinct list of GameObjects that have 2 components in common
    /// </summary>
    /// <typeparam name="C1"></typeparam>
    /// <typeparam name="C2"></typeparam>
    /// <returns></returns>
    public static List<GameObject> Find<C1, C2>() where C1 : Component where C2 : Component
    {
        Type key;
        key = typeof(C1);

        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //
        retObjects = FindComponent(key);
        List<GameObject> temp1 = retObjects.Keys.ToList();

        key = typeof(C2);
        retObjects = FindComponent(key);
        List<GameObject> temp2 = retObjects.Keys.ToList();

        List<GameObject> finalList = temp1.Intersect(temp2).ToList();

        return finalList;
    }
    /// <summary>
    /// Return a distinct list of GameObjects that have 3 components in common
    /// </summary>
    /// <typeparam name="C1"></typeparam>
    /// <typeparam name="C2"></typeparam>
    /// <typeparam name="C3"></typeparam>
    /// <returns></returns>
    public static List<GameObject> Find<C1, C2, C3>() where C1 : Component where C2 : Component where C3 : Component
    {
        Type key;
        key = typeof(C1);

        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //
        retObjects = FindComponent(key);
        List<GameObject> temp1 = retObjects.Keys.ToList();

        key = typeof(C2);
        retObjects = FindComponent(key);
        List<GameObject> temp2 = retObjects.Keys.ToList();

        List<GameObject> tempInbetween1_2 = temp1.Intersect(temp2).ToList();

        key = typeof(C3);
        retObjects = FindComponent(key);
        List<GameObject> temp3 = retObjects.Keys.ToList();

        List<GameObject> finalList = tempInbetween1_2.Intersect(temp3).ToList();

        return finalList;

    }
    public static List<GameObject> Find<C1, C2, C3, C4>() where C1 : Component where C2 : Component where C3 : Component where C4 : Component
    {
        Type key;
        key = typeof(C1);

        // 
        // Lookup the Component in our list and get a dictionary of GameObjects associated with it
        //
        retObjects = FindComponent(key);
        List<GameObject> temp1 = retObjects.Keys.ToList();

        key = typeof(C2);
        retObjects = FindComponent(key);
        List<GameObject> temp2 = retObjects.Keys.ToList();

        List<GameObject> tempInbetween1_2 = temp1.Intersect(temp2).ToList();

        key = typeof(C3);
        retObjects = FindComponent(key);
        List<GameObject> temp3 = retObjects.Keys.ToList();

        List<GameObject> tempInbetween1_2_3 = tempInbetween1_2.Intersect(temp3).ToList();

        key = typeof(C4);
        retObjects = FindComponent(key);
        List<GameObject> temp4 = retObjects.Keys.ToList();

        List<GameObject> finalList = tempInbetween1_2_3.Intersect(temp4).ToList();

        return finalList;
    }
    public class CannotHaveTwoInstancesException : Exception
    {
        public CannotHaveTwoInstancesException() : base("There can be only one instance of a the Services class. It is a singleton...") { }
    }
    public class ServiceAlreadyRegisteredException : Exception
    {
        public ServiceAlreadyRegisteredException(string goName) : base("A service of that name (" + goName + ")  has already been registered!") { }
    }
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(System.Type T) : base("Service (" + T.ToString() + ") not found.\nAlways Register() in Awake(). Never Find() in Awake(). Check Script Execution Order.") { }
    }
}
