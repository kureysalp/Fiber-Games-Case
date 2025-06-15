using System.Collections.Generic;
using UnityEngine;
using System;

namespace FiberCase.Systems
{
    public class Pool
    {
        public GameObject ReferencePrefab { get; }
        public Queue<Component> Queue { get; }
        public int ExpandCount { get; }

        public Pool(GameObject gameObject, Queue<Component> queue, int expandCount)
        {
            ReferencePrefab = gameObject;
            Queue = queue;
            ExpandCount = expandCount;
        }

    }

    public abstract class Poolable : MonoBehaviour
    {
        private static Dictionary<Type, Pool> objPool
            = new();

        /// <summary>
        /// Get an object from the pool; If fails, use the alternative method to create one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : Poolable
        {
            if (objPool.TryGetValue(typeof(T), out var pool))
            {
                if (pool.Queue.Count == 0)
                {
                    var pooledObj = AddObjectsToPool<T>(pool);
                    pooledObj.Reactivate();
                    return pooledObj;
                }

                var ret = pool.Queue.Dequeue() as T;
                ret.Reactivate();
                return ret;
            }

            Debug.LogError($"You have not created a pool for {typeof(T)}.");
            return null;
        }

        /// <summary>
        /// Create a pool for the object.
        /// </summary>
        /// <param name="objectToPool"></param>
        /// <param name="poolCount"></param>
        /// <param name="expandCount"></param>
        /// <typeparam name="T"></typeparam>
        public static void CreatePool<T>(GameObject objectToPool, int poolCount, int expandCount)
        {
            var type = typeof(T);
            var queue = new Queue<Component>();

            for (var i = 0; i < poolCount; i++)
            {
                var instantiatedObject = Instantiate(objectToPool);
                instantiatedObject.SetActive(false);
                var component = instantiatedObject.GetComponent<T>() as Component;
                queue.Enqueue(component);

            }

            var pool = new Pool(objectToPool, queue, expandCount);

            objPool.Add(type, pool);
        }

        private static T AddObjectsToPool<T>(Pool pool) where T : Poolable
        {
            for (var i = 0; i < pool.ExpandCount; i++)
            {
                var instantiatedObject = Instantiate(pool.ReferencePrefab);
                instantiatedObject.SetActive(false);
                var component = instantiatedObject.GetComponent<T>() as Component;
                pool.Queue.Enqueue(component);
            }

            return pool.Queue.Dequeue() as T;
        }

        /// <summary>
        /// Return the object to the pool
        /// </summary>
        public void ReturnToPool()
        {
            Reset();
            var type = GetType();
            if (objPool.TryGetValue(type, out var pool))
            {
                if (pool.Queue.Contains(this)) return;
                pool.Queue.Enqueue(this);
            }
            else
                Debug.LogError($"You have not created a pool for {nameof(type)}.");
        }


        /// <summary>
        /// Reset the object so it is ready to go into the object pool
        /// </summary>
        /// <returns>whether the reset is successful.</returns>
        protected virtual void Reset()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Reactive the object as it goes out of the object pool
        /// </summary>
        protected virtual void Reactivate()
        {
            gameObject.SetActive(true);
        }
    }
}