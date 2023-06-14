using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKeyGet : MonoBehaviour
{
    public KeyCard keycard;
    public int accessLevel; //Űī�� ����
    public bool CanAccess;  //ī�忡 ���ٰ�������
    private bool isPlayer;  //�÷��̾ �����ߴ���
    // Start is called before the first frame update
    void Awake()
    {
        CanAccess = false;
        isPlayer = false;
        accessLevel = keycard.AccessLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsPause)
            return;

        if (isPlayer && Input.GetKeyDown(KeyCode.F))
        {
            GetTutorialCard();
        }
    }

    public void KeyCardCheck()
    {
        //if (keycard.isHave)
        //{
        //    gameManager.GetCard(keycard.AccessLevel, keycard.isHave);//Űī�带 ������������ ���ӸŴ������� ����
        //    Debug.Log(keycard.KeyColor + "key card�� ȹ���߽��ϴ�");
        //}
        //else
        //{
        //    return;//����
        //}
    }

    private void GetTutorialCard()
    {
        keycard.AccessToKey();//Űī�� ȹ��
        KeyCardCheck();//Űī�带 ȹ�������� ���ӸŴ����� ����
        gameObject.SetActive(false);//Űī�� ����(������Ʈ����)
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayer = true;//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isPlayer = false;//������ Ȯ��(ù ���ٿ��� �̺�Ʈ ����, �̺�Ʈ �߿��� ���� �Ұ���, �̺�Ʈ ���� �Ŀ� Űī�� ȹ�� ����)
        }
    }
}
