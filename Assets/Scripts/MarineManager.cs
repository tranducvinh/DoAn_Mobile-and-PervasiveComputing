using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MarineManager : MonoBehaviour
{
    private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
    Vector2 startPos;
    Vector2 endPos;
    float t;
    public float moveSpeed;
    public float timeWait;
    public float gridSize;
    private bool isAlive = true;
    private bool isFinish = false;
    private bool isTreasure = false;

    GameObject currentTreasure;

    void Start()
    {
        moveSpeed = 1f;
        timeWait = 0.5f;
        gridSize = 120f;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveUp();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDown();
            }
        }
    }

    public bool IsFinish()
    {
        return isFinish;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public bool IsTreasure()
    {
        return isTreasure;
    }

    public void SetFinish(bool value)
    {
        isFinish = value;
    }

    public void SetAlive(bool value)
    {
        isAlive = value;
    }

    public void SetTreasure(bool value)
    {
        isTreasure = value;
    }

    public void MoveUp()
    {
        if (!isMoving)
        {
            AudioManager.Instance.PlayMovement();
            StartCoroutine(MoveUp_());
        }

    }
    public void MoveDown()
    {
        if (!isMoving)
        {
            AudioManager.Instance.PlayMovement();
            StartCoroutine(MoveDown_());
        }
    }
    public void MoveLeft()
    {
        if (!isMoving)
        {
            AudioManager.Instance.PlayMovement();
            StartCoroutine(MoveLeft_());
        }
    }
    public void MoveRight()
    {
        if (!isMoving)
        {
            AudioManager.Instance.PlayMovement();
            StartCoroutine(MoveRight_());
        }
    }

    //public IEnumerator TakeTreasure_()
    //{
    //    if (isTreasure == true)
    //    {
    //        isTreasure = false;
    //        AudioManager.Instance.PlayTakeTreasure();
    //        GameManager.score += 50;
    //        Debug.Log("Nhat Kho Bau");
    //        currentTreasure.GetComponent<Animator>().SetBool("isAlive", false);
    //        currentTreasure.GetComponent<Collider2D>().enabled = false;
    //        yield return new WaitForSeconds(timeWait + 0.5f);
    //    }
    //    yield return null;
    //}

    public IEnumerator MoveUp_()
    {
        t = 0;
        isMoving = true;

        startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(startPos.x, startPos.y + gridSize);

        while (t < 1f)
        {
            t += moveSpeed * Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return new WaitForSeconds(timeWait);
    }

    public IEnumerator MoveDown_()
    {
        t = 0;
        isMoving = true;

        startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(startPos.x, startPos.y - gridSize);

        while (t < 1f)
        {
            t += moveSpeed * Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return new WaitForSeconds(timeWait);
    }

    public IEnumerator MoveLeft_()
    {
        t = 0;
        isMoving = true;

        this.transform.eulerAngles = new Vector2(0, 180);

        startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(startPos.x - gridSize, startPos.y);

        while (t < 1f)
        {
            t += moveSpeed * Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return new WaitForSeconds(timeWait);
    }

    public IEnumerator MoveRight_()
    {
        t = 0;
        isMoving = true;

        this.transform.eulerAngles = new Vector2(0, 0);

        startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(startPos.x + gridSize, startPos.y);

        while (t < 1f)
        {
            t += moveSpeed * Time.deltaTime;
            this.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return new WaitForSeconds(timeWait);
    }

    /// <summary>
    /// Hàm xử lý va chạm
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            AudioManager.Instance.PlayCoinCollected();
            GameManager.score += 10;
            isTreasure = false;
            other.GetComponent<Animator>().SetBool("isAlive", false);
            other.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<CanvasGroup>().alpha = 0;
        }
        else if (other.tag == "Finish")
        {
            AudioManager.Instance.StopMusicBackground();
            AudioManager.Instance.PlayLevelComplete();
            isFinish = true;
            isTreasure = false;
            other.GetComponent<Collider2D>().enabled = false;
        }
        else if (other.tag == "Mine")
        {
            AudioManager.Instance.PlayExplosion();
            AudioManager.Instance.PlayLevelFailed();
            isTreasure = false;
            other.GetComponent<Animator>().SetBool("isAlive", false);
            other.GetComponent<Collider2D>().enabled = false;
            isFinish = true;
            isAlive = false;
        }
        else if (other.tag == "Wall")
        {
            AudioManager.Instance.PlayExplosion();
            //AudioManager.Instance.PlayLevelFailed();
            isTreasure = false;
            isFinish = true;
            isAlive = false;
        }
        else if (other.tag == "Treasure")
        {
            isTreasure = true;
            currentTreasure = other.gameObject;
        }
    }

    public void SpeedUp()
    {
        if (moveSpeed < 3f && moveSpeed >= 1f)
        {
            moveSpeed += 1f;
        }
        else if (moveSpeed < 1f)
        {
            moveSpeed += 0.5f;
        }

        GameObject.Find("txtSpeed").GetComponent<Text>().text = moveSpeed.ToString() + "x";
    }

    public void SpeedDown()
    {
        if (moveSpeed <= 3f && moveSpeed > 1f)
        {
            moveSpeed -= 1f;
        }
        else if (moveSpeed == 1)
        {
            moveSpeed -= 0.5f;
        }

        GameObject.Find("txtSpeed").GetComponent<Text>().text = moveSpeed.ToString() + "x";
    }
}
