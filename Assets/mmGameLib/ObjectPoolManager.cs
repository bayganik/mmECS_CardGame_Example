/*

 * @Author: David Crook
 * 
 * Use this singleton Object Pooling Manager Class to manage a series of object pools.
 * Typical uses are for particle effects, bullets, enemies etc. 
 * 
 * usage: attache the following to any GameObject 
 *  
    private void CreateObjectPools()
    {
    // Initial pool size = 1
    // max pool size = 3
    // can grow = true
    //
        ObjectPoolingManager.Instance.CreatePool(GameObject, PoolName, 1, 3, ture);
        ObjectPoolingManager.Instance.ActivateObject(GameObject.name or PoolName);
    }
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectPoolingManager
{
    //
    //  the variable is declared to be volatile to ensure that 
    //  assignment to the instance variable completes before the 
    //  instance variable can be accessed.
    //
    private static volatile ObjectPoolingManager instance;
    private Dictionary<String, ObjectPool> objectPools;         //the actual pool of objects by name
    private static object syncRoot = new System.Object();       //object for locking

    /// <summary>
    /// Constructor for the class.
    /// </summary>
    private ObjectPoolingManager()
    {
        //Ensure object pools exists.

        this.objectPools = new Dictionary<String, ObjectPool>();
    }
    /// <summary>
    /// Property for retreiving the singleton.  See msdn documentation.
    /// </summary>
    public static ObjectPoolingManager Instance
    {
        get
        {
            //check to see if it doesnt exist
            if (instance == null)
            {
                lock (syncRoot)             //lock access, if it is already locked, wait.
                {
                    //the instance could have been made between 
                    //checking and waiting for a lock to release.

                    if (instance == null)
                    {
                        instance = new ObjectPoolingManager();                  //create a new instancev
                    }
                }
            }
            return instance;        //return either the new instance or the already built one.
        }
    }

    /// <summary>
    /// Create a new object pool of the objects you wish to pool
    /// </summary>
    /// <param name="objToPool">The object you wish to pool.  The name property of the object MUST be unique.</param>
    /// <param name="initialPoolSize">Number of objects you wish to instantiate initially for the pool.</param>
    /// <param name="maxPoolSize">Maximum number of objects allowed to exist in this pool.</param>
    /// <param name="canGrow">this list can grow.</param>
    /// <returns></returns>
    public bool CreatePool(GameObject objToPool, string poolName, int initialPoolSize, int maxPoolSize, bool canGrow=true)
    {
        //
        // if a name is not assigned then use object's name
        //
        if (poolName.Length == 0)
            poolName = objToPool.name;

        if (ObjectPoolingManager.Instance.objectPools.ContainsKey(objToPool.name))
        {
            return false;       //let the caller know it already exists, just use the pool out there.
        }
        else
        {
            //znznznznznznznznznznznznznznznznznznznznznznznznznzn
            //     create a new pool using the properties
            //znznznznznznznznznznznznznznznznznznznznznznznznznzn
            ObjectPool nPool = new ObjectPool(objToPool, initialPoolSize, maxPoolSize, canGrow);

            //Add the pool to the dictionary of pools to manage 
            //using the object name as the key and the pool as the value.

            ObjectPoolingManager.Instance.objectPools.Add(poolName, nPool);
            //
            //  We created a new pool!
            //
            return true;
        }
    }
    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    /// <param name="poolName">name of the object pool </param>
    /// <returns>A GameObject if one is available, else returns null if all are currently active and max size is reached.</returns>
    public GameObject ActivateObject(string poolName)
    {
        //
        // Find the right pool, then look thru the list of objects
        // for the active one.  It can be null
        //

        return ObjectPoolingManager.Instance.objectPools[poolName].ActivateObject();
    }
    public void DeActivateObject(string poolName, GameObject inObj)
    {
        ObjectPoolingManager.Instance.objectPools[poolName].DeActivateObject(inObj);
    }

}