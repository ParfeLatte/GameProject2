using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyCardData", menuName = "Scriptable Object/KeyCard")]
public class KeyCard : ScriptableObject
{
    public int AccessLevel;//������ ����(�ܰ躰)
    public string KeyColor;//����
    public Sprite sprite;//��������Ʈ
    public bool isHave = false;//Űī�带 �������ִ���

    public void AccessToKey()
    {
        isHave = true;
    }//�̺�Ʈ�� ������ Key�� �������ؼ� ȹ��
}
