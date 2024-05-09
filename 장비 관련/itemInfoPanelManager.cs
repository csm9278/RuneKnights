using UnityEngine;
using UnityEngine.UI;

public class itemInfoPanelManager : MonoBehaviour
{
    public Text itemNameText;
    public Text infoText;
    public Button equipBtn;
    public Button unEquipBtn;
    public Button sellBtn;
    public Text sellCostText;
    public Image ItemImage;
    public int listIdx = -1;
    bool isEquipItem = false;
    InventoryManager invenMgr;
    EquipPanelManager equipMgr;

    //��ȭ�ϱ� ����
    [Header("---Upgrade Item ---")]
    public Button UpgradeBtn;
    public GameObject upgradePanel;
    public ItemUpgradeManager itemUpgradeMgr;



    private void Start() => StartFunc();

    private void StartFunc()
    {
        invenMgr = FindObjectOfType<InventoryManager>();
        equipMgr = FindObjectOfType<EquipPanelManager>();
        itemUpgradeMgr = upgradePanel.GetComponent<ItemUpgradeManager>();

        if (equipBtn != null)
            equipBtn.onClick.AddListener(equipItemFunc);

        if (unEquipBtn != null)
            unEquipBtn.onClick.AddListener(UnEquipItemFunc);

        if (sellBtn != null)
            sellBtn.onClick.AddListener(SellItemFunc);

        if (UpgradeBtn != null)
            UpgradeBtn.onClick.AddListener(UpgradeItemFunc);
    }

    private void Update() => UpdateFunc();

    private void UpdateFunc()
    {
        
    }

    /// <summary>
    /// ��� ���� ��ư �Լ�
    /// </summary>
    public void equipItemFunc()
    {
        if (listIdx < 0)
            return;

        if (GlobalData.equipItems[(int)GlobalData.items[listIdx].itemType] != null) //�Ѿ�� ������ Ÿ���� �������� �̹� ������̶��..
            invenMgr.AddItenFunc(GlobalData.equipItems[(int)GlobalData.items[listIdx].itemType]);

        GlobalData.equipItems[(int)GlobalData.items[listIdx].itemType] = GlobalData.items[listIdx]; //�������� ����
        equipMgr.EquipItems(GlobalData.items[listIdx].itemType);

        GlobalData.items.RemoveAt(listIdx);
        invenMgr.ClearBtn(listIdx);
        invenMgr.RefreshItems();

        ClearInfo();
    }

    /// <summary>
    /// ������� ��ư �Լ�
    /// </summary>
    public void UnEquipItemFunc()
    {
        if (listIdx < 0)
            return;

        invenMgr.AddItenFunc(GlobalData.equipItems[listIdx]);
        equipMgr.UnEquipItem(GlobalData.equipItems[listIdx].itemType);

        ClearInfo();
    }
    
    /// <summary>
    /// ������ �Ǹ� ��ư �Լ�
    /// </summary>
    public void SellItemFunc()
    {
        if(isEquipItem)
        {
            StartCoroutine(equipMgr.AddGoldFunc(GlobalData.equipItems[listIdx].sellPrice));   //���ݸ�ŭ �� �߰�
            equipMgr.UnEquipItem(GlobalData.equipItems[listIdx].itemType);      //������ ����

            ClearInfo();
        }
        else
        {
            //GlobalData.playerGold += GlobalData.items[listIdx].sellPrice;
            StartCoroutine(equipMgr.AddGoldFunc(GlobalData.items[listIdx].sellPrice));
            GlobalData.items.RemoveAt(listIdx);
            invenMgr.ClearBtn(listIdx);
            invenMgr.RefreshItems();
            //equipMgr.goldText.text = "Gold : " + GlobalData.playerGold.ToString();

            ClearInfo();
        }
    }

    /// <summary>
    /// ������ ���׷��̵� ��ư �Լ�
    /// </summary>
    void UpgradeItemFunc()
    {
        upgradePanel.gameObject.SetActive(true);

        if (isEquipItem)
            itemUpgradeMgr.UpgradeInit(GlobalData.equipItems[listIdx], listIdx, isEquipItem);
        else
            itemUpgradeMgr.UpgradeInit(GlobalData.items[listIdx], listIdx, isEquipItem);

        ClearInfo();
    }

    /// <summary>
    /// �����Ŵ����� ������ �ʱ�ȭ
    /// </summary>
    public void ClearInfo()
    {
        itemNameText.text = "";
        infoText.text = "";
        ItemImage.sprite = Resources.Load<Sprite>("ItemImage/NoneItem");
        listIdx = -1;

        sellCostText.text = "";
        equipBtn.gameObject.SetActive(false);
        unEquipBtn.gameObject.SetActive(false);
        sellBtn.gameObject.SetActive(false);
        UpgradeBtn.gameObject.SetActive(false);
    }

    public void SetInfo(Item item, int idx, bool isEquipItem = false)
    {
        itemNameText.text = item.itemName;
        infoText.text = item.infoString;
        ItemImage.sprite = item.itemImage;
        sellCostText.text = "�ǸŰ��� : " + item.sellPrice;
        listIdx = idx;
        this.isEquipItem = isEquipItem;

        changeEquipBtns(isEquipItem);
    }

    /// <summary>
    /// �����ϱ� �Ǵ� �����ϱ� ����
    /// </summary>
    /// <param name="isEquipItem"></param>
    void changeEquipBtns(bool isEquipItem)
    {
        if (isEquipItem)
        {
            equipBtn.gameObject.SetActive(false);
            unEquipBtn.gameObject.SetActive(true);
            UpgradeBtn.gameObject.SetActive(true);
        }
        else
        {
            equipBtn.gameObject.SetActive(true);
            unEquipBtn.gameObject.SetActive(false);
            UpgradeBtn.gameObject.SetActive(true);
        }

        sellBtn.gameObject.SetActive(true);
    }
}