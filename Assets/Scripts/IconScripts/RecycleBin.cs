using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class RecycleBin : MonoBehaviour
{
    public static int score { get; private set; } = 0;
    public static bool GameOver { get; private set; } = true;
    InteractableIcon inRecycleBin = null;
    SaveData saveData;

    [SerializeField] private ResetButton resetButton;
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private ObjectPool iconPool;

    private IconSnapToGrid iconSnap;

    [SerializeField] private AudioSource recycleSound;
    [SerializeField] private AudioSource errorPopUp;

    bool errorSoundPlayed = false;
    bool shaking = false;

    private void Awake()
    {
        if (iconPool == null) throw new System.ArgumentNullException("Missing icon pool, please assign one in the inspector.");
        iconSnap = GetComponent<IconSnapToGrid>();
        iconSnap.Init();
        iconSnap.TrySnapIcon();
        saveData = FindObjectOfType<SaveData>();
    }

    void Update()
    {
        if (!GameOver)
        {
            if (inRecycleBin != null)
            {
                if (!inRecycleBin.IsClickedOn)
                {
                    inRecycleBin.HoverOverBin = false;
                    iconPool.ReturnGameObjectToPool(inRecycleBin.gameObject);
                    inRecycleBin = null;
                    recycleSound.Play();
                    shakeGameObject(gameObject, 0.3f, 0.1f);
                    score++;
                }
            }

            if ((float)iconPool.NumberOfActiveObjects / (float)iconPool.MaxNumberOfObjects > 0.85)
            {
                GameOver = true;
                resetPanel.SetActive(true);
                resetButton.Init();

                if (!errorSoundPlayed)
                {
                    errorPopUp.Play();
                    errorSoundPlayed = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameOver)
        {
            inRecycleBin = other.gameObject.GetComponent<InteractableIcon>();
            if (inRecycleBin != null) inRecycleBin.HoverOverBin = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!GameOver)
        {
            if (inRecycleBin != null) inRecycleBin.HoverOverBin = false;
            inRecycleBin = null;
        }
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

    public void Save(string name)
    {
        saveData.Save(name, score);
        score = 0;
        SceneManager.LoadScene(0);
    }

    public void Reset()
    {
        score = 0;
        if (inRecycleBin != null) inRecycleBin.HoverOverBin = false;
        inRecycleBin = null;
        GameOver = false;
    }
}
