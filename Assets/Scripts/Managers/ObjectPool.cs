using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int numberOfObjectsToPool;
    [SerializeField] private bool fixedPoolSize = true;

    IDictionary<GameObject, bool> objectPool = new Dictionary<GameObject, bool>();
    IDictionary<GameObject, Coroutine> collectionOfDisablingCoroutines = new Dictionary<GameObject, Coroutine>();
    Queue<GameObject> activeGameObjects = new Queue<GameObject>();

    public int NumberOfActiveObjects { get; private set; } = 0;
    public int MaxNumberOfObjects { get { return numberOfObjectsToPool; } }

    private void Awake()
    {
        GameObject[] tempObjectPool = GrowPool(numberOfObjectsToPool);

        foreach (GameObject obj in tempObjectPool)
        {
            objectPool.Add(obj, obj.activeSelf);
        }
    }

    /*----------------- Pool Bases ----------------------------*/
    GameObject[] GrowPool(int numberToGrow)
    {
        GameObject[] temp = new GameObject[numberToGrow];

        for (int i = 0; i < numberToGrow; i++)
        {
            GameObject spawnedObject = Instantiate(objectToPool, transform);
            spawnedObject.SetActive(false);
            temp[i] = spawnedObject;
        }

        return temp;
    }

    /*----------------- Instantiate/Destroy ----------------------------*/
    public GameObject GetAvailableObject()
    {
        GameObject gameObjectToReturn = null;

        //Check if objectpool dictionary has any inactive gameobjects
        //otherwise grow pool/recycle an active gameobject
        if (objectPool.Values.Contains(false))
        {
            for (int i = 0; i < objectPool.Count; i++)
            {
                if (!objectPool[objectPool.Keys.ToList()[i]])
                {
                    gameObjectToReturn = objectPool.Keys.ToList()[i];
                }
            }
        }
        else
        {
            if (fixedPoolSize)
            {
                //if all are used
                //Dequeue one of the gameobjects and return the recycled one
                gameObjectToReturn = null;
            }
            else
            {
                //if pool is not fixed size
                //Grow the pool and return one of the elements 
                GameObject[] temp = GrowPool(5);
                foreach (GameObject GO in temp)
                {
                    objectPool.Add(GO, GO.activeSelf);
                }
                gameObjectToReturn = temp[0];
            }
        }

        if (gameObjectToReturn != null)
        {
            gameObjectToReturn.SetActive(true);
            objectPool[gameObjectToReturn] = gameObjectToReturn.activeSelf;
            activeGameObjects.Enqueue(gameObjectToReturn);
            NumberOfActiveObjects++;
        }
      
        return gameObjectToReturn;
    }

    public void ReturnGameObjectToPool(GameObject go)
    {
        if (objectPool[go])
        {
            go.SetActive(false);
            NumberOfActiveObjects--;
            objectPool[go] = go.activeSelf;
            go.transform.parent = transform;

            List<GameObject> tempList = activeGameObjects.ToList();
            tempList.Remove(go);
            activeGameObjects.Clear();
            foreach (GameObject obj in tempList)
            {
                activeGameObjects.Enqueue(obj);
            }
        }
        else
        {
            Debug.LogError("GameObject cant be found, have you passed in the wrong gameobject?");
        }
    }

    public void ResetPool ()
    {
        for (int i = objectPool.Count - 1; i >= 0; i--)
        {
            if (objectPool[objectPool.Keys.ToList()[i]])
            {
                ReturnGameObjectToPool(objectPool.Keys.ToList()[i]);
            }
        }
    }

    public bool PoolAvailable ()
    {
        foreach (bool go in objectPool.Values)
        {
            if (!go) return true;
        }

        return false;
    }

    /*-------------------- Recyles the oldest active gameobject -----------------------*/
    private GameObject RecycleGameObject()
    {
        Coroutine result;
        GameObject recycledGameObject = activeGameObjects.Dequeue();

        if (collectionOfDisablingCoroutines.TryGetValue(recycledGameObject.gameObject, out result))
        {
            collectionOfDisablingCoroutines.Remove(recycledGameObject);
            StopCoroutine(result);
        }

        ReturnGameObjectToPool(recycledGameObject);
        return recycledGameObject;
    }

    /*-------------------- For Objects with Lifetime -----------------------*/
    public void RemoveObjectAfterTime(GameObject go, float time)
    {
        Coroutine removeObject = StartCoroutine(RemoveAfterTime(go, time));

        Coroutine result;
        if (collectionOfDisablingCoroutines.TryGetValue(go, out result))
        {
            collectionOfDisablingCoroutines[go] = removeObject;
            StopCoroutine(result);
        }
        else
        {
            collectionOfDisablingCoroutines.Add(go, removeObject);
        }
    }

    IEnumerator RemoveAfterTime(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        ReturnGameObjectToPool(go);
    }
}
