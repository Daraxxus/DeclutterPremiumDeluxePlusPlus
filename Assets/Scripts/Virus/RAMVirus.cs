using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAMVirus : MonoBehaviour {
    private float slowDownRate = 0.15f;
    private bool activatable = false;
    private bool cooldown = false;
    private bool isActive = false;
    bool shaking = false;
    private float chanceToActivate = 5.0f;
    MouseManager mouse;
    [SerializeField] private GameObject windowDefender;
    [SerializeField] private GameObject windowDefenderLoading;
    [SerializeField] private AudioSource ramDefenderishere;

    void Start()
    {
        mouse = FindObjectOfType<MouseManager>();
    }

	// Update is called once per frame
	void Update () {
        if (!RecycleBin.GameOver)
        {
            if (!activatable)
            {
                if (RecycleBin.score > 5)
                {
                    activatable = true;
                    chanceToActivate = 5.0f;
                }
            }
            else
            {
                if (RecycleBin.score % 5 == 0 && RecycleBin.score <= 50)
                {
                    chanceToActivate = 15 + (((RecycleBin.score / 5) - 1) * 9);
                }

                if (!isActive)
                    TryActivate();
            }
        }
        else
        {
            activatable = false;
            mouse.SlowDown(0f);
            windowDefender.SetActive(false);
            windowDefenderLoading.SetActive(false);
        }
	}

    void TryActivate ()
    {
        if (Random.Range(0.0f, 1.0f) > (float ) 1 - (chanceToActivate/100.0f) && !cooldown)
        {
            isActive = true;
            Active();
        }
    }

    void Active()
    {
        mouse.SlowDown(slowDownRate);
        windowDefender.SetActive(true);
        shakeGameObject(windowDefender, 0.3f, 0.1f);
        ramDefenderishere.Play();
    }

    public void Block()
    {
        if (isActive)
        {
            mouse.SlowDown(0f);
            isActive = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(Random.Range (5.0f, 12.0f));
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
