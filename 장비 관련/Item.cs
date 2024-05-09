using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public enum RankType
    {
        Normal = 1,
        Epic,
        Legend
    }

    public enum ItemType
    {
        Weapon,
        Helmet,
        Armor,
        Boots,
        None
    }

    public enum Status
    {
        Health,
        AtkDmg,
        Heal,
        FireBall,
        WaterFall,
        Explosion
    }

    public RankType rankType = RankType.Normal; //���� ���
    public ItemType itemType = ItemType.Weapon; //���� Ÿ��
    public string itemName = "";                    //������ �̸�
    public int[] addStatus = { 0, 0, 0, 0, 0, 0 };  //������ �ɷ�ġ
    public Sprite itemImage;                        //������ �̹���
    public int WeaponIdx = -1;
    public int sellPrice = 0;                       //�Ǹ� ����
    public int UpSellPrice = 0;                     //�Ǹ� ���� ������

    public string infoString = "";                  //������ ���� string

    //��ȭ ��� �߰�
    public int upgradeNum = 0;
    public string addedName = "";


    /// <summary>
    /// /�������� ������ �Լ�
    /// </summary>
    /// <param name="isCombine"></param>
    /// <param name="Itype"></param>
    /// <param name="Rtype"></param>
    public Item(bool isCombine = false, Item combineItem = null)
    {
        if (!isCombine)
            SetItem();
        else
            CombineItem(combineItem.itemType, combineItem.rankType);
    }

    public void SetItem()
    {
        int kindrand = Random.Range(0, 4);
        switch(kindrand)
        {
            case (int)ItemType.Weapon:
                SetWeapon();
                itemType = ItemType.Weapon;
                break;

            case (int)ItemType.Helmet:
                SetHelmet();
                itemType = ItemType.Helmet;
                break;

            case (int)ItemType.Armor:
                SetArmor();
                itemType = ItemType.Armor;
                break;

            case (int)ItemType.Boots:
                SetBoots();
                itemType = ItemType.Boots;
                break;
        }

        UpSellPrice = sellPrice;

        for (int i = 0; i < addStatus.Length; i++)
        {
            if (addStatus[i] <= 0)
                continue;

            infoString += GlobalData.itemInfoName[i] + " +" + addStatus[i].ToString() + "\n";
        }
    }

    public void CombineItem(ItemType itemType, RankType RType)
    {
        int combineRand = Random.Range(0, 100);

        int percent = 0;

        if(RType == RankType.Normal)
        {
            percent = 80;
        }
        else if(RType == RankType.Epic)
        {
            percent = 50;
        }
        else
        {
            percent = 20;
        }

        Debug.Log("��ȭȮ���� : " + percent);

        if(combineRand < percent)   //������ ������
        {
            int setrank = ((int)RType * 4) + 4;
            switch (itemType)
            {
                case ItemType.Weapon:
                    SetWeapon(setrank);
                    this.itemType = ItemType.Weapon;
                    break;

                case ItemType.Helmet:
                    SetHelmet(setrank);
                    this.itemType = ItemType.Helmet;
                    break;

                case ItemType.Armor:
                    SetArmor(setrank);
                    this.itemType = ItemType.Armor;
                    break;

                case ItemType.Boots:
                    SetBoots(setrank);
                    this.itemType = ItemType.Boots;
                    break;
            }
            Debug.Log(combineRand + "/ " + percent + "���� ����");
        }
        else
        {
            int setrank = (int)RType * 4;
            switch (itemType)
            {
                case ItemType.Weapon:
                    SetWeapon(setrank);
                    this.itemType = ItemType.Weapon;
                    break;

                case ItemType.Helmet:
                    SetHelmet(setrank);
                    this.itemType = ItemType.Helmet;
                    break;

                case ItemType.Armor:
                    SetArmor(setrank);
                    itemType = ItemType.Armor;
                    break;

                case ItemType.Boots:
                    SetBoots(setrank);
                    this.itemType = ItemType.Boots;
                    break;
            }
            Debug.Log(combineRand + "/ " + percent + "���� ����");
        }

        UpSellPrice = sellPrice;

        for (int i = 0; i < addStatus.Length; i++)
        {
            if (addStatus[i] <= 0)
                continue;

            infoString += GlobalData.itemInfoName[i] + " +" + addStatus[i].ToString() + "\n";
        }
    }

    public void RefreshInfo()
    {
        infoString = "";

        for (int i = 0; i < addStatus.Length; i++)
        {
            if (addStatus[i] <= 0)
                continue;

            infoString += GlobalData.itemInfoName[i] + " +" + addStatus[i].ToString() + "\n";
        }
    }

    public void SetWeapon(int setWeaponRank = - 1)
    {
        int weaponRank = Random.Range(0, 10);
        int Upgrade = Random.Range(0, 10);

        if (setWeaponRank > 0)
            weaponRank = setWeaponRank;
        

        if(Upgrade > 2)
        {
            if (weaponRank < 7)
            {
                itemName = "����� ��";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/NormalSword");
                AddStatusFunc(Status.AtkDmg, 1, 11);
                sellPrice = 50;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "��ö ���̽�";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/EpicSword");
                AddStatusFunc(Status.Health, 1, 21);
                AddStatusFunc(Status.AtkDmg, 1, 21);
                sellPrice = 100;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "������ �ڴ�";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/LegendSword");
                AddStatusFunc(Status.Health, 1, 30);
                AddStatusFunc(Status.AtkDmg, 21, 31);
                AddStatusFunc(Status.FireBall, 20, 30);
                sellPrice = 200;
                rankType = RankType.Legend;
            }
        }
        else
        {
            if (weaponRank < 7)
            {
                itemName = "��ȭ�� ����� ��";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/UpNormalSword");
                int randatk = Random.Range(10, 21);
                addStatus[(int)Status.AtkDmg] = randatk;
                sellPrice = 100;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "��ȭ�� ��ö ���̽�";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/UpEpicSword");
                AddStatusFunc(Status.Health, 5, 20);
                AddStatusFunc(Status.AtkDmg, 5, 20);
                sellPrice = 150;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "��ȭ�� ������ �ڴ�";
                itemImage = Resources.Load<Sprite>("ItemImage/Weapon/UpLegendSword");

                AddStatusFunc(Status.Health, 20, 50);
                AddStatusFunc(Status.AtkDmg, 31, 45);
                AddStatusFunc(Status.FireBall, 10, 20);
                AddStatusFunc(Status.WaterFall, 5, 15);
                AddStatusFunc(Status.Explosion, 20, 30);
                sellPrice = 500;
                rankType = RankType.Legend;
            }
        }
    }

    public void SetHelmet(int setWeaponRank = -1)
    {
        int weaponRank = Random.Range(0, 10);
        int Upgrade = Random.Range(0, 10);
        if (setWeaponRank > 0)
            weaponRank = setWeaponRank;
        if (Upgrade > 2)
        {
            if (weaponRank < 7)
            {
                itemName = "���� ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/NormalHelmet");
                AddStatusFunc(Status.Health, 20, 50);
                sellPrice = 50;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "���� ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/EpicHelmet");

                AddStatusFunc(Status.Health, 30, 60);
                AddStatusFunc(Status.Heal, 10, 30);
                sellPrice = 100;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "Ǯ �÷���Ʈ ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/LegendHelmet");
                AddStatusFunc(Status.Health, 70, 100);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 200;
                rankType = RankType.Legend;
            }
        }
        else
        {
            if (weaponRank < 7)
            {
                itemName = "��ȭ�� ���� ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/UpNormalHelmet");
                AddStatusFunc(Status.Health, 35, 60);
                sellPrice = 100;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "��ȭ�� ���� ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/UpEpicHelmet");
                AddStatusFunc(Status.Health, 45, 70);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 150;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "��ȭ�� Ǯ �÷���Ʈ ���";
                itemImage = Resources.Load<Sprite>("ItemImage/Helmet/UpLegendHelmet");

                AddStatusFunc(Status.Health, 100, 130);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                AddStatusFunc(Status.FireBall, 0, 20);
                AddStatusFunc(Status.WaterFall, 0, 10);
                AddStatusFunc(Status.Explosion, 0, 10);
                sellPrice = 500;
                rankType = RankType.Legend;
            }
        }
    }

    public void SetArmor(int setWeaponRank = -1)
    {
        int weaponRank = Random.Range(0, 10);
        int Upgrade = Random.Range(0, 10);
        if (setWeaponRank > 0)
            weaponRank = setWeaponRank;
        if (Upgrade > 2)
        {
            if (weaponRank < 7)
            {
                itemName = "���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/NormalArmor");
                AddStatusFunc(Status.Health, 20, 50);
                sellPrice = 50;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/EpicArmor");

                AddStatusFunc(Status.Health, 30, 60);
                AddStatusFunc(Status.Heal, 10, 30);
                sellPrice = 100;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "Ǯ �÷���Ʈ ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/LegendArmor");
                AddStatusFunc(Status.Health, 70, 100);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 200;
                rankType = RankType.Legend;
            }
        }
        else
        {
            if (weaponRank < 7)
            {
                itemName = "��ȭ�� ���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/UpNormalArmor");
                int randatk = Random.Range(10, 21);
                addStatus[(int)Status.AtkDmg] = randatk;
                sellPrice = 100;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "��ȭ�� ���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/UpEpicArmor");
                AddStatusFunc(Status.Health, 45, 70);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 150;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "��ȭ�� Ǯ �÷���Ʈ ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Armor/UpLegendArmor");

                AddStatusFunc(Status.Health, 100, 130);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                AddStatusFunc(Status.FireBall, 0, 20);
                AddStatusFunc(Status.WaterFall, 0, 10);
                AddStatusFunc(Status.Explosion, 0, 10);
                sellPrice = 500;
                rankType = RankType.Legend;
            }
        }
    }

    public void SetBoots(int setWeaponRank = -1)
    {
        int weaponRank = Random.Range(0, 10);
        int Upgrade = Random.Range(0, 10);
        if (setWeaponRank > 0)
            weaponRank = setWeaponRank;
        if (Upgrade > 2)
        {
            if (weaponRank < 7)
            {
                itemName = "���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/NormalBoots");
                AddStatusFunc(Status.Health, 20, 50);
                sellPrice = 50;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/EpicBoots");

                AddStatusFunc(Status.Health, 30, 60);
                AddStatusFunc(Status.Heal, 10, 30);
                sellPrice = 100;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "Ǯ �÷���Ʈ ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/LegendBoots");
                AddStatusFunc(Status.Health, 70, 100);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 200;
                rankType = RankType.Legend;
            }
        }
        else
        {
            if (weaponRank < 7)
            {
                itemName = "��ȭ�� ���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/UpNormalBoots");
                int randatk = Random.Range(10, 21);
                addStatus[(int)Status.AtkDmg] = randatk;
                sellPrice = 100;
            }
            else if (7 <= weaponRank && weaponRank < 9)
            {
                itemName = "��ȭ�� ���� ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/UpEpicBoots");
                AddStatusFunc(Status.Health, 45, 70);
                AddStatusFunc(Status.Heal, 20, 40);
                sellPrice = 150;
                rankType = RankType.Epic;
            }
            else
            {
                itemName = "��ȭ�� Ǯ �÷���Ʈ ����";
                itemImage = Resources.Load<Sprite>("ItemImage/Boots/UpLegendBoots");

                AddStatusFunc(Status.Health, 100, 130);
                AddStatusFunc(Status.AtkDmg, 1, 20);
                AddStatusFunc(Status.Heal, 20, 40);
                AddStatusFunc(Status.FireBall, 0, 20);
                AddStatusFunc(Status.WaterFall, 0, 10);
                AddStatusFunc(Status.Explosion, 0, 10);
                sellPrice = 500;
                rankType = RankType.Legend;
            }
        }
    }

    public void AddStatusFunc(Status status, int min, int max)
    {
        int rand = Random.Range(min, max);
        addStatus[(int)status] = rand;
    }
}