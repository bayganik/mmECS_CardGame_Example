/*
 * @Author: David Crook
 * Use the object pools to help reduce object instantiation time and performance 
 * with objects that are frequently created and used.
 *
 * You can creat an object pool for multiple "types" of objects (projectiles, enemies, swarms, etc)
 * Then use the singlton for object pool manager
 *  
 * The object pool class is the management structure for individual object pools.  This includes creation of the pool, retrieving objects, 
 * increasing the pool size and also shrinking the pool.  If you have multiple object pools, they should be accessed through the object pool 
 * manager and not the individual pools.
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
/// <summary>
/// The object pool is a list of already instantiated with the "same" game objects
/// </summary>
public class ObjectPool
{
    private List<GameObject> pooledObjects;             //list of objects
    private bool listCanGrow ;                          //can list of object grow
    private int maxPoolSize;                            //max number of objects in the list
    private int initialPoolSize;                        //initial and default number of objects to have in the list.
    
    //sample of the actual object to store.
    //used if we need to grow the list.
    private GameObject pooledObj;


    /// <summary>
    /// Constructor for creating a new Object Pool.
    /// </summary>
    /// <param name="inobj">Game Object for this pool</param>
    /// <param name="initialPoolSize">Initial and default size of the pool.</param>
    /// <param name="maxPoolSize">Maximum number of objects this pool can contain.</param>
    /// <param name="cangrow">can this pool grow when you need more objects?</param>
    public ObjectPool(GameObject inobj, int _initialPoolSize, int maxPoolSize, bool cangrow)
    {
        //
        // Get a new List for this particular GameObject
        //
        listCanGrow = cangrow;
        pooledObjects = new List<GameObject>();
        //
        // instantiate all the GameObjects with default values
        //
        for (int i = 0; i < _initialPoolSize; i++)
        {
            GameObject nObj = GameObject.Instantiate(inobj, Vector3.zero, Quaternion.identity) as GameObject;

            nObj.SetActive(false);              //make sure the object isn't active.
            pooledObjects.Add(nObj);            //add the object too our list.
            GameObject.DontDestroyOnLoad(nObj); //Don't destroy on load, so we can manage when scene changes.
        }

        // Pools data

        this.maxPoolSize = maxPoolSize;
        this.pooledObj = inobj;
        this.initialPoolSize = _initialPoolSize;
        this.listCanGrow = cangrow;
    }
    public void DeActivateObject(GameObject inObj)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == inObj)
            {
                pooledObjects[i].SetActive(false);           //set the object to active.
            }
        }
    }
    /// <summary>
    /// Returns an active object from the object pool without resetting any of its values.
    /// You will need to set its values and set it inactive again when you are done with it.
    /// </summary>
    /// <returns>Game Object of requested type if it is available, otherwise null.</returns>
    public GameObject ActivateObject()
    {
        //look for the first one that is inactive.

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeSelf == false)
            {
                pooledObjects[i].SetActive(true);           //set the object to active.
                return pooledObjects[i];                    //return the object we found.
            }
        }
        //
        // if we make it this far, we obviously didn't find an inactive object.
        // so we need to see if we can grow beyond our current count.
        //
        if (this.maxPoolSize > this.pooledObjects.Count)
        {
            //
            //  Instantiate a new object.
            //
            GameObject nObj = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;

            //set it to active since we are about to use it.

            nObj.SetActive(true);
            pooledObjects.Add(nObj);                //add it to the pool of objects

            return nObj;                           //return the object to the requestor.
        }
        else if (listCanGrow)
        {
            //Instantiate a new object.

            GameObject nObj = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;

            //set it to active since we are about to use it.

            nObj.SetActive(true);
            pooledObjects.Add(nObj);                //add it to the pool of objects
 
            return nObj;                           //return the object to the requestor.
        }

        //
        // if we made it this far obviously we didn't have any inactive objects
        // we also were unable to grow, so return null as we can't return an object.
        //
        return null;
    }
}