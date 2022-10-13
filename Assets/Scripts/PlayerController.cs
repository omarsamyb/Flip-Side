using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveValue;
    public float forwardSpeed = 5;
    public float speedIncrease = 5;
    private float horizontalRawInput;
    private bool isAxisInUse;
    private float jumpRawInput;
    private bool isJumpInUse;
    private float switchRawInput;
    private bool isSwitchInUse;
    ObjectPooler objectPooler;
    private float accelerometerInput;
    private bool accAxisInUse;
    private bool isJumping;

    private float xspeed;
    private float yspeed;
    private float moveSpeed;
    private Vector3 rotationTargetFlipped = new Vector3(-180, 180, 0);
    private Vector3 currentRotation = Vector3.zero;
    private float xValueBeforeJump;

    private void Start()
    {
        isAxisInUse = false;
        isJumpInUse = false;
        isSwitchInUse = false;
        moveValue = 3f;
        forwardSpeed = 5;
        speedIncrease = 5;
        transform.GetChild(41).gameObject.GetComponent<MeshRenderer>().material = GameManager.game.scoreCollectableMatArr[Random.Range(0, 3)];
        InvokeRepeating("ChangeColor", 15f, 15f);
        objectPooler = ObjectPooler.SharedInstance;
        accAxisInUse = false;
        isJumping = false;
        xspeed = 0.6f;
        yspeed = 0.15f;
        moveSpeed = 0.2f;
        transform.eulerAngles = currentRotation;
        xValueBeforeJump = 0f;
    }

    private void FixedUpdate()
    {
        if (GameManager.game.isAlive && !isJumping)
        {
            Vector3 forwardMoveVector = Vector3.forward * Time.fixedDeltaTime * forwardSpeed;
            float currentX = transform.position.x;

            if (horizontalRawInput != 0)
                if (isAxisInUse == false)
                {
                    if (currentX + (horizontalRawInput * moveValue) <= moveValue && currentX + (horizontalRawInput * moveValue) >= -moveValue)
                    {
                        AudioManager.instance.Play("SwitchLane");
                        transform.position += new Vector3(horizontalRawInput * moveValue, 0, 0);
                    }
                    isAxisInUse = true;
                }
            if (horizontalRawInput == 0) isAxisInUse = false;

            if (jumpRawInput != 0)
                if (isJumpInUse == false)
                {
                    Jump();
                    isJumpInUse = true;
                }
            if (jumpRawInput == 0) isJumpInUse = false;

            if (switchRawInput != 0)
                if (isSwitchInUse == false)
                {
                    SwitchView();
                    isSwitchInUse = true;
                }
            if (switchRawInput == 0) isSwitchInUse = false;

            if (accelerometerInput > 0.3 || accelerometerInput < -0.3)
                if (accAxisInUse == false)
                {
                    if (accelerometerInput > 0) accelerometerInput = 1;
                    else accelerometerInput = -1;
                    if (currentX + (accelerometerInput * moveValue) <= moveValue && currentX + (accelerometerInput * moveValue) >= -moveValue)
                        transform.position += new Vector3(accelerometerInput * moveValue, 0, 0);
                    accAxisInUse = true;
                    Invoke("FreeAccAxis", 0.5f);
                }

            transform.Translate(forwardMoveVector, Space.World);
        }
    }

    void Update()
    {
        horizontalRawInput = Input.GetAxisRaw("Horizontal");
        jumpRawInput = Input.GetAxisRaw("Jump");
        switchRawInput = Input.GetAxisRaw("Switch");
        accelerometerInput = Input.acceleration.x;
        if (Input.GetKeyDown(KeyCode.R)) ChangeColor();
        if (Input.GetKeyDown(KeyCode.E)) GameManager.game.ModifyHealth(1, true);
        if (Input.GetKeyDown(KeyCode.Q)) GameManager.game.ModifyScore(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioManager.instance.Play("CollisionSFX");
            GameManager.game.ModifyHealth(-1, false);
            collision.gameObject.SetActive(false);
            collision.gameObject.transform.SetParent(objectPooler.gameObject.transform, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ScoreCollectable"))
        {
            if (GameManager.game.isFlipped)
            {
                if (transform.GetChild(41).gameObject.GetComponent<MeshRenderer>().sharedMaterial.Equals(other.gameObject.GetComponent<MeshRenderer>().sharedMaterial))
                {
                    AudioManager.instance.Play("CollisionSFX");
                    GameManager.game.ModifyScore(-5);
                }
                else
                {
                    AudioManager.instance.Play("CorrectCoinSFX");
                    GameManager.game.ModifyScore(10);
                }
            }
            else
            {
                if (transform.GetChild(41).gameObject.GetComponent<MeshRenderer>().sharedMaterial.Equals(other.gameObject.GetComponent<MeshRenderer>().sharedMaterial))
                {
                    AudioManager.instance.Play("CorrectCoinSFX");
                    GameManager.game.ModifyScore(10);
                }
                else
                {
                    AudioManager.instance.Play("CollisionSFX");
                    GameManager.game.ModifyScore(-5);
                }
            }
            other.gameObject.SetActive(false);
            other.gameObject.transform.SetParent(objectPooler.gameObject.transform, false);
        }
        else if (other.gameObject.CompareTag("HealthCollectable"))
        {
            AudioManager.instance.Play("HealthOrbSFX");
            GameManager.game.ModifyHealth(1, false);
            other.gameObject.SetActive(false);
            other.gameObject.transform.SetParent(objectPooler.gameObject.transform, false);
        }
    }

    void ChangeColor()
    {
        AudioManager.instance.Play("SwitchColorSFX");
        Material newMat = GameManager.game.scoreCollectableMatArr[Random.Range(0, 3)];
        while (transform.GetChild(41).gameObject.GetComponent<MeshRenderer>().sharedMaterial.Equals(newMat))
        {
            newMat = GameManager.game.scoreCollectableMatArr[Random.Range(0, 3)];
        }
        transform.GetChild(41).gameObject.GetComponent<MeshRenderer>().material = newMat;
    }
    public void SwitchView()
    {
        if (GameManager.game.mainCamera.enabled)
        {
            GameManager.game.sideCamera.enabled = true;
            GameManager.game.mainCamera.enabled = false;
        }
        else
        {
            GameManager.game.mainCamera.enabled = true;
            GameManager.game.sideCamera.enabled = false;
        }
    }
    public void Jump()
    {
        AudioManager.instance.Play("JumpSFX");
        AudioManager.instance.setPitch("GameplayMusic", 0.8f);
        moveSpeed = 1f;
        xspeed = 4.32f;
        yspeed = 1.08f;
        Time.timeScale = 0.1f;
        xValueBeforeJump = transform.position.x;
        Vector3 fromPos = transform.position;
        Vector3 toPos = Vector3.zero;
        if (GameManager.game.isFlipped)
        {
            GameManager.game.isFlipped = false;
            //transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z);
            toPos.y = 0.8f;
            StartCoroutine(RotateTo(currentRotation, Vector3.zero));
            StartCoroutine(TranslateTo(fromPos, toPos));
        }
        else
        {
            GameManager.game.isFlipped = true;
            //transform.position = new Vector3(transform.position.x, 3.2f, transform.position.z);
            toPos.y = 3.2f;
            StartCoroutine(RotateTo(currentRotation, rotationTargetFlipped));
            StartCoroutine(TranslateTo(fromPos, toPos));
        }
        isJumping = true;
    }

    IEnumerator RotateTo(Vector3 from, Vector3 to)
    {
        float t0;
        float t1 = 0;
        for (t0 = 0f; t0 < 1f; t0 += yspeed * Time.deltaTime)
        {
            currentRotation = new Vector3(Mathf.Lerp(from.x, to.x, t1), Mathf.Lerp(from.y, to.y, t0), 0);
            transform.eulerAngles = currentRotation;
            t1 += xspeed * Time.deltaTime;
            if (currentRotation.x == -180 && GameManager.game.isFlipped || (currentRotation.x == 0 && !GameManager.game.isFlipped))
            {
                yspeed = 10f;
                Time.timeScale = 1f;
            }
            yield return null;
        }
        transform.eulerAngles = to;
        currentRotation = to;
        Vector3 position = transform.position;
        position.x = xValueBeforeJump;
        transform.position = position;
    }
    IEnumerator TranslateTo(Vector3 from, Vector3 to)
    {
        Vector3 currentPos;
        for (float t = 0f; t < 1f; t += moveSpeed * Time.deltaTime)
        {
            currentPos = Vector3.Lerp(from, to, t);
            currentPos.x = transform.position.x;
            currentPos.z = transform.position.z;
            transform.position = currentPos;
            if ((currentRotation.x == -180 && GameManager.game.isFlipped) || (currentRotation.x == 0 && !GameManager.game.isFlipped))
                moveSpeed = 4f;
            yield return null;
        }
        currentPos.x = xValueBeforeJump;
        currentPos.y = to.y;
        currentPos.z = transform.position.z;
        transform.position = currentPos;
        isJumping = false;
        AudioManager.instance.setPitch("GameplayMusic", 1f);
    }

    void FreeAccAxis()
    {
        accAxisInUse = false;
    }
}
