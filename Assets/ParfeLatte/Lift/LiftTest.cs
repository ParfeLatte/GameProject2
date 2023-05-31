using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTest : MonoBehaviour
{
    public GameObject Lift;
    public InteractObj UI;
    public bool Reverse;//true�϶� �Ʒ���, false�϶� ����
    public bool isMove;//�����̴��� �ȿ����̴���
    public List<GameObject> Objects = new List<GameObject>();
    Vector3 nextpos = new Vector3(0, 4f, 0);


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if(Reverse && isMove)
        {
            Down();
        }
        else if(isMove)
        {
            Up();
        }
    }

    public void Move()
    {
        isMove = true;
        UI.HideInteractUI();
    }

    public void Stop()
    {
        isMove = false;
        UI.ShowInteractUI();
    }

    public void Up()
    {
        transform.position = transform.position + nextpos * Time.deltaTime;
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].transform.position = Objects[i].transform.position + nextpos * Time.deltaTime;
        }
    }
    public void Down()
    {
        transform.position = transform.position - nextpos * Time.deltaTime;
        for (int i = 0; i < Objects.Count; i++)
        {
           Objects[i].transform.position = Objects[i].transform.position - nextpos * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag=="Player" || col.gameObject.tag=="enemy") {
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Stop" && isMove)
        {
            if (Reverse)
            {
               Reverse = false;
            }
            else if (!Reverse)
            {
               Reverse = true;
            }
            Stop();
        }
    }
}
