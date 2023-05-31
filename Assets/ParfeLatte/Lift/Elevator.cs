    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject elevator;
    public GameObject elevatorDoor;
    public Animator elevAnim;
    public InteractObj UI;
    public int floor;//몇 층인지
    public bool Reverse;//true일때 아래로, false일때 위로
    public bool isMove;//움직이는지 안움직이는지
    public List<GameObject> Objects = new List<GameObject>();
    public List<Transform> Floors = new List<Transform>();//층 위치
    public Transform StopPos;
    Vector3 nextpos = new Vector3(0, 8f, 0);
    // Start is called before the first frame update
    void Awake()
    {
        elevatorDoor.SetActive(false);
        elevAnim.SetBool("isOpen", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(isMove)
        {
            ElevMove();
            StopCheck();
        }
    }

    public void Move()
    {
        isMove = true;
        UI.HideInteractUI();
        elevatorDoor.SetActive(true);
        elevAnim.SetBool("isOpen", false);
    }

    public void StopCheck()
    {
        if (!Reverse)
        {
            if (transform.position.y >= StopPos.position.y)
            {
                isMove = false;
                UI.ShowInteractUI();
                elevatorDoor.SetActive(false);
                elevAnim.SetBool("isOpen", true);
            }
        }
        else if(Reverse)
        {
            if(transform.position.y <= StopPos.position.y)
            {
                isMove = false;
                UI.ShowInteractUI();
                elevatorDoor.SetActive(false);
                elevAnim.SetBool("isOpen", true);
            }
        }
    }
    public void Up()
    {
        if(floor == 3)
        {
            UI.ShowInteractUI();
            return;
        }
        else if (floor == 1)
        {
            floor = 2;
            StopPos = Floors[1];
            Reverse = false;
            Move();
        }
        else if(floor == 2)
        {
            floor = 3;
            StopPos= Floors[2];
            Reverse = false;
            Move();
        }
    }
    public void Down()
    {
        if(floor == 1)
        {
            UI.ShowInteractUI();
            return;
        }
        if(floor == 3)
        {
            floor = 2;
            StopPos = Floors[1];
            Reverse = true;
            Move();
        }
        else if(floor == 2)
        {
            floor = 1;
            StopPos = Floors[0];
            Reverse = true;
            Move();
        }
    }

    public void Call(int i)
    {
        if (floor == i)
        {
            return;
        }
        else if(floor > i)
        {
            StopPos = Floors[i-1];
            Down();
            Move();
        }
        else if(floor < i)
        {
            StopPos = Floors[i-1];
            Up();
            Move();
        }
    }

    public void ElevMove()
    {
        if (!Reverse && isMove)
        {
            transform.position = transform.position + nextpos * Time.deltaTime;
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].transform.position = Objects[i].transform.position + nextpos * Time.deltaTime;
            }
        }
        else if (Reverse && isMove)
        {
            transform.position = transform.position - nextpos * Time.deltaTime;
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].transform.position = Objects[i].transform.position - nextpos * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "enemy")
        {
            Objects.Add(col.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (Objects.Contains(col.gameObject))
        {
            Objects.Remove(col.gameObject);
        }
    }
}
