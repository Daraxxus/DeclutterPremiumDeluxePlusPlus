using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopUpVirus : MonoBehaviour {
    [SerializeField] GameObject[] popUps = new GameObject[10];
    float xMinBounds, xMaxBounds, yMinBounds, yMaxBounds;
    float chanceToActivate = 5.0f;

    bool isActive = false;
    bool activatable = false;
    bool cooldown = false;
    bool isRunning = false;
    bool shaking = false;

    [SerializeField] GameObject PopUpBlocker;
    [SerializeField] GameObject popUpLoading;

	// Update is called once per frame
	void Update () {
        if (!RecycleBin.GameOver)
        {
            if (!activatable)
            {
                if (RecycleBin.score > 20)
                {
                    activatable = true;
                }
            }
            else
            {
                if (RecycleBin.score % 5 == 0 && RecycleBin.score <= 50)
                {
                    chanceToActivate = 5 + (((RecycleBin.score / 5) - 1) * 9);
                }

                if (!isActive)
                    TryActivate();
                else
                {
                    if (!isRunning)
                    {
                        foreach (GameObject popup in popUps)
                        {
                            if (!popup.activeSelf)
                            {
                                StartCoroutine(SpawnPopUps());
                                return;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            StopAllCoroutines();
            foreach (GameObject popup in popUps)
            {
                popup.SetActive(false);
            }
            PopUpBlocker.SetActive(false);
            popUpLoading.SetActive(false);
        }
	}

    void TryActivate()
    {
        if (Random.Range(0.0f, 1.0f) > (float)1 - (chanceToActivate / 100.0f) && !cooldown)
        {
            isActive = true;
            Active();
        }
    }
    
    void Active()
    {
        StartCoroutine(SpawnPopUps());
        PopUpBlocker.SetActive(true);
        shakeGameObject(PopUpBlocker, 0.3f, 0.1f);
    }

    IEnumerator SpawnPopUps ()
    {
        isRunning = true;
        List<GameObject> popUpSequence = new List<GameObject>();
        foreach (GameObject popup in popUps)
        {
            if (!popup.activeSelf) popUpSequence.Add(popup);
        }

        if (popUpSequence.Count > 0)
        {
            popUpSequence = popUps.ToList().RandomizeList();
            int counter = 0;
            while (counter < popUpSequence.Count || isActive)
            {
                if (!popUpSequence[counter].activeSelf)
                {
                    popUpSequence[counter].SetActive(true);
                    shakeGameObject(popUpSequence[counter], 0.3f, 0.1f);
                    popUpSequence[counter].GetComponent<AudioSource>().Play();
                }

                foreach (GameObject popUp in popUps)
                {
                    if (!popUp.activeSelf)
                    {
                        popUpSequence.Add(popUp);
                    }
                }

                counter++;
                yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
            }
        }

        isRunning = false;
    }

    public void BlockPopUp()
    {
        isActive = false;
        StopAllCoroutines();
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown ()
    {
        cooldown = true;
        yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
        cooldown = false;
    }

    IEnumerator shakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break; //Exit!
        }

        //Get Original Pos and rot
        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;

        //Shake Speed
        const float speed = 0.1f;

        //Angle Rotation(Optional)
        const float angleRot = 2;

        //Do the actual shaking
        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed = speed;
            float decreaseAngle = angleRot;

            //Don't Translate the Z Axis if 2D Object
            Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
            tempPos.z = defaultPos.z;
            objTransform.position = tempPos;

            //Only Rotate the Z axis if 2D
            objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            yield return null;


            //Check if we have reached the decreasePoint then start decreasing  decreaseSpeed value
            if (counter >= decreasePoint)
            {

                //Reset counter to 0 
                counter = 0f;
                while (counter <= decreasePoint)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                    decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);


                    //Don't Translate the Z Axis if 2D Object
                    Vector3 temp = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                    temp.z = defaultPos.z;
                    objTransform.position = temp;

                    //Only Rotate the Z axis if 2D
                    objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                    yield return null;
                }

                //Break from the outer loop
                break;
            }
        }
        objTransform.position = defaultPos; //Reset to original postion
        objTransform.rotation = defaultRot;//Reset to original rotation

        shaking = false; //So that we can call this function next time
    }


    void shakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint)
    {
        if (shaking)
        {
            return;
        }
        shaking = true;
        StartCoroutine(shakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint));
    }
}
