using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemUpgradeManager : MonoBehaviour
{
    int idx = -1;
    bool isEquipItem = false;

    public Text UpgradeInfoText;
    public Text UpgradeResultText;

    public Button UpgradeBtn;
    public Button CancleBtn;
    public Button OkBtn;

    public ParticleSystem upgradingEff;
    public ParticleSystem upgradeFailEff;
    public ParticleSystem upgradeSuccessEff;

    EquipPanelManager equipPanelMgr;

    Item upgradeItem;
    public Image itemImage;

    int upgradePercent = 50;
    int upgradeCost = 0;

    private void OnEnable()
    {
        UpgradeResultText.text = "";
        UpgradeBtn.gameObject.SetActive(true);
        CancleBtn.gameObject.SetActive(true);
        OkBtn.gameObject.SetActive(false);
        itemImage.color = Color.white;
    }

    private void Start() => StartFunc();

    private void StartFunc()
    {
        if (UpgradeBtn != null)
            UpgradeBtn.onClick.AddListener(() =>
            {
                if (GlobalData.playerGold < upgradeCost)
                    return;

                equipPanelMgr.StartCoroutine(equipPanelMgr.DecreaseGoldFunc(upgradeCost));

                UpgradeBtn.gameObject.SetActive(false);
                CancleBtn.gameObject.SetActive(false);
                StartCoroutine(UpgradeStartCo());
            });

        if (CancleBtn != null)
            CancleBtn.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

        if (OkBtn != null)
            OkBtn.onClick.AddListener(() =>
            {
                equipPanelMgr.TotalStatusFunc();
                this.gameObject.SetActive(false);
            });

        equipPanelMgr = FindObjectOfType<EquipPanelManager>();
    }

    public void UpgradeInit(Item item, int itemIdx, bool isEquip)
    {
        idx = itemIdx;
        Debug.Log(idx);
        isEquipItem = isEquip;

        upgradeItem = item;

        itemImage.sprite = item.itemImage;

        upgradePercent = (100 - (upgradeItem.upgradeNum * 10));
        upgradeCost = (200 + (upgradeItem.upgradeNum * 100)) * (int)upgradeItem.rankType;


        UpgradeInfoText.text = item.itemName + "\n" +
                               "��ȭ Ȯ�� : " + upgradePercent + "%\n" +
                               "��ȭ ��� : " + upgradeCost + "Gold\n" +
                               "��ȭ �Ͻðڽ��ϱ�?";
    }

    IEnumerator UpgradeStartCo()
    {
        for(int i = 0; i < 3; i++)
        {
            upgradingEff.Play();
            yield return new WaitForSeconds(0.5f);
        }

        int rand = Random.Range(0, 100);
        if(rand <= upgradePercent)  //��ȭ ����
        {
            upgradeSuccessEff.Play();
            upgradeItem.upgradeNum++;   //��ȭ ��ġ ����

            if(upgradeItem.addedName == "")
                upgradeItem.addedName = upgradeItem.itemName;   //���� ���

            upgradeItem.itemName = upgradeItem.addedName + " + " + upgradeItem.upgradeNum; //�̸� ����

            UpgradeInfoText.text = " ��ȭ ����!!";
            UpgradeResultText.text = upgradeItem.itemName;
            UpgradeResultText.gameObject.SetActive(true);
            OkBtn.gameObject.SetActive(true);
            StartCoroutine(UpgradeItemStatus());    //�ɷ�ġ ���׷��̵� ����
        }
        else
        {
            upgradeFailEff.Play();
            UpgradeResultText.text = "<color=red>��ȭ ����!!</color>";
            UpgradeInfoText.text = " ��� ���簡 �Ǿ����ϴ�...";
            itemImage.color = new Color32(118, 118, 118, 255);

            if(isEquipItem) //������ ��������
            {
                equipPanelMgr.UnEquipItem(GlobalData.equipItems[idx].itemType);      //������ ����

                equipPanelMgr.infoMgr.ClearInfo();
            }
            else
            {
                GlobalData.items.RemoveAt(idx);
                equipPanelMgr.invenMgr.ClearBtn(idx);
            }

            UpgradeResultText.gameObject.SetActive(true);
            OkBtn.gameObject.SetActive(true);
        }

    }

    IEnumerator UpgradeItemStatus()
    {
        string beforetext = "";
        int[] beforeStauts = new int[upgradeItem.addStatus.Length]; // ����� ������
        int[] upgradeStatus = new int[upgradeItem.addStatus.Length]; // ����� ������
        int plusValue = 0;

        for(int i = 0; i < upgradeItem.addStatus.Length; i++)   //�̸� ������ ������Ʈ
        {
            if (upgradeItem.addStatus[i] <= 0)
                continue;

            if(!isEquipItem)    //����ϰ� �ִ� �������� �ƴҶ�
            {
                beforeStauts[i] = GlobalData.items[idx].addStatus[i];       //���׷��̵� �� ġ���� ���
                GlobalData.items[idx].addStatus[i] += (int)(upgradeItem.addStatus[i] * 0.2f);   //���� ġ�� ���׷��̵�

                upgradeStatus[i] = GlobalData.items[idx].addStatus[i];      //���׷��̵� �� ġ���� ����
                if (upgradeStatus[i] == GlobalData.items[idx].addStatus[i]) //���� 20%��ȭ���� �ɷ�ġ ��ȭ�� ���� ���� �ɷ�ġ�Ͻ� +1��ġ �߰�
                {
                    upgradeStatus[i]++;
                    GlobalData.items[idx].addStatus[i]++;
                }
            }
            else
            {
                beforeStauts[i] = GlobalData.equipItems[idx].addStatus[i];       //���׷��̵� �� ġ���� ���
                GlobalData.equipItems[idx].addStatus[i] += (int)(upgradeItem.addStatus[i] * 0.2f);   //���� ġ�� ���׷��̵�

                upgradeStatus[i] = GlobalData.equipItems[idx].addStatus[i];      //���׷��̵� �� ġ���� ����
                if (upgradeStatus[i] == GlobalData.equipItems[idx].addStatus[i]) //���� 20%��ȭ���� �ɷ�ġ ��ȭ�� ���� ���� �ɷ�ġ�Ͻ� +1��ġ �߰�
                {
                    upgradeStatus[i]++;
                    GlobalData.equipItems[idx].addStatus[i]++;
                }
            }
        }

        if (!isEquipItem)
            GlobalData.items[idx].sellPrice += GlobalData.items[idx].UpSellPrice;   //�ǸŰ��� ����
        else
            GlobalData.equipItems[idx].sellPrice += GlobalData.equipItems[idx].UpSellPrice;   //�ǸŰ��� ����


        equipPanelMgr.invenMgr.RefreshItemBtn(idx, isEquipItem); //��ư�� �������� �ٲ��ش�

        for (int i = 0; i < upgradeItem.addStatus.Length; i++)
        {
            if (upgradeItem.addStatus[i] <= 0)
                continue;

            yield return new WaitForSeconds(0.5f);

            UpgradeInfoText.text += "\n" + GlobalData.itemInfoName[i];     // ��ȭ���� /n ü��
            beforetext = UpgradeInfoText.text;  //��ȭ���� /n ü��

            for(int j = beforeStauts[i]; j < upgradeStatus[i]; j++) //ġ���� ���ݸ�ŭ ��ġ ���׷��̵�
            {
                plusValue++;
                beforeStauts[i]++;
                UpgradeInfoText.text = beforetext + " +" + beforeStauts[i].ToString();
                yield return new WaitForSeconds(0.05f);
            }

            UpgradeInfoText.text += "<color=green> (+ " + plusValue.ToString() + ")</color>";
            plusValue = 0;
        }
    }
}