using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyCardData", menuName = "Scriptable Object/KeyCard")]
public class KeyCard : ScriptableObject
{
    public int AccessLevel;//엑세스 레벨(단계별)
    public string KeyColor;//색깔
    public Sprite sprite;//스프라이트
    public bool isHave = false;//키카드를 가지고있는지

    public void AccessToKey()
    {
        isHave = true;
    }//이벤트가 끝나면 Key에 엑세스해서 획득
}
