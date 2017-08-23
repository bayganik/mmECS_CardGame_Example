/// <summary>
/// Based on:
/// Simple service manager. Allows global access to a single instance of any class.
/// Copyright (c) 2014-2015 Eliot Lash
/// </summary>
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class Services
{
    private static Services _instance;
    private Dictionary<Type, object> services = new Dictionary<Type, object>();


    public Services()
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
    protected static Services instance
    {
        get
        {
            if (_instance == null)
            {
                new Services();
            }

            return _instance;
        }
    }


    /// <summary>
    /// Register the specified service instance. Usually called in Awake(), like this:
    /// Set<ExampleService>(this);
    /// </summary>
    /// <param name="service">Service instance object.</param>
    /// <typeparam name="T">Type of the instance object.</typeparam>
    public static void Register<T>(T service) where T : class
    {
        if (instance.services.ContainsKey(typeof(T)))
        {
            throw new ServiceAlreadyRegisteredException(typeof(T));
        }
        instance.services.Add(typeof(T), service);
    }


    /// <summary>
    /// Find the instance of the specified service. Usually called in Start(), like this:
    /// ExampleService es = Get<ExampleService>();
    /// Throws new UnityException if service not found.
    /// </summary>
    /// <typeparam name="T">Type of the service.</typeparam>
    /// <returns>Service instance, or null if not initialized</returns>
    public static T Find<T>() where T : class
    {
        T ret = null;
        try
        {
            ret = instance.services[typeof(T)] as T;
        }
        catch (KeyNotFoundException)
        {
            throw new ServiceNotFoundException(typeof(T));
        }
        return ret;
    }


    /// <summary>
    /// Clears internal dictionary of service instances.
    /// This will not clear out any global state that they contain,
    /// unless there are no other references to the object.
    /// </summary>
    public static void Clear()
    {
        instance.services.Clear();
    }


    /// <summary>
    /// Prints the list of services in the debug console
    /// </summary>
    public static void Debug()
    {
        string output = "Debug Services List:\n";

        foreach (var s in instance.services)
        {
            output += "* " + s.Key + " = " + s.Value.ToString() + "\n";
        }
        output += "Total: " + instance.services.Count.ToString() + " services registered.";

        UnityEngine.Debug.Log(output);
    }

    public class CannotHaveTwoInstancesException : Exception
    {
        public CannotHaveTwoInstancesException() : base("There can be only one instance of a the Services class. It is a singleton...") { }
    }

    public class ServiceAlreadyRegisteredException : Exception
    {
        public ServiceAlreadyRegisteredException(System.Type T) : base("A service of that name (" + T.ToString() + ")  has already been registered!") { }
    }

    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(System.Type T) : base("Service (" + T.ToString() + ") not found.\nAlways Register() in Awake(). Never Find() in Awake(). Check Script Execution Order.") { }
    }
}
