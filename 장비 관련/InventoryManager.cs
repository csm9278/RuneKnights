using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    itemInfoPanelManager infomanager;
    [HideInInspector] public CombinePanelManager combineManager;

    public GameObject itemBtn;
    public Button[] sortBtns;

    List<GameObject> BtnList;
    public Transform scrollContent;
    
    private void Start() => StartFunc();

    private void StartFunc()
    {
        infomanager = FindObjectOfType<itemInfoPanelManager>();
        for(int i = 0; i < sortBtns.Length; i++)
        {
            int idx = i;
            sortBtns[idx].onClick.AddListener(() =>
            {
                SortList((Item.ItemType)idx);
            });
        }

        BtnList = new List<GameObject>();
        InitItem();
    }

    private void Update() => UpdateFunc();

    private void UpdateFunc()
    {
    }

    public void ClearBtn(int idx)
    {

        Destroy(BtnList[idx]);
        BtnList.RemoveAt(idx);

        RefreshItems();
    }

    public void AddItenFunc(Item item)
    {
        item.WeaponIdx = BtnList.Count;
        GlobalData.items.Add(item);
        GameObject btnObj = Instantiate(itemBtn, scrollContent);    //������ ��ư�� ��ũ�Ѻ信 �߰��ϱ�

        BtnList.Add(btnObj);    // ��ư����Ʈ�� �߰�

        RefreshItems();         //����Ʈ ����Ʈ ��������
    }

    public void InitItem()
    {
        for(int i = 0; i < GlobalData.items.Count; i++)
        {
            GameObject btnObj = Instantiate(itemBtn, scrollContent);
            BtnList.Add(btnObj);    // ��ư����Ʈ�� �߰�
        }

        RefreshItems();         //����Ʈ ����Ʈ ��������
    }

    public void SortList(Item.ItemType type, int rankType = -1)
    {
        //�ϴ� ���� �ѹ� ���ְ�
        for (int i = 0; i < BtnList.Count; i++)
        {
            BtnList[i].gameObject.SetActive(true);
        }

        if (type == Item.ItemType.None) //��ü�� ���ΰ�
        {
            return;
        }
        else    //�װԾƴϸ� Ÿ�Ժ��� ���ֱ�
        {
            for (int i = 0; i < BtnList.Count; i++)
            {
                if (GlobalData.items[i].itemType != type)
                    BtnList[i].gameObject.SetActive(false);

                if(GlobalData.items[i].rankType != (Item.RankType)rankType && rankType >= 0) //���� ��ũŸ�Ե� ���ҰŸ�?
                    BtnList[i].gameObject.SetActive(false);
            }
        }
    }

    public void SortBtnActive(bool isActive)
    {
        for(int i = 0; i < sortBtns.Length; i++)
        {
            sortBtns[i].gameObject.SetActive(isActive);
        }
    }

    public void DisableList()
    {
        //�ϴ� ���� ���ֱ�
        for (int i = 0; i < BtnList.Count; i++)
        {
            BtnList[i].gameObject.SetActive(false);
        }
    }

    public void RefreshItemBtn(int idx, bool isEquip)
    {
        if(!isEquip)
            GlobalData.items[idx].RefreshInfo();
        else
            GlobalData.equipItems[idx].RefreshInfo();
    }

    public void RefreshItems()
    {
        Button btn;

        for(int i = 0; i < BtnList.Count; i++)
        {
            int idx = i;
            btn = BtnList[i].GetComponent<Button>();
            btn.GetComponentsInChildren<Image>()[1].sprite = GlobalData.items[idx].itemImage;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (!combineManager.isCombining)
                    infomanager.SetInfo(GlobalData.items[idx], idx);
                else
                {
                    if(combineManager.isReady)
                        combineManager.SetCombineInfo(GlobalData.items[idx], idx);
                }

            });
        }
    }
}