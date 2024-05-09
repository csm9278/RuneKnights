using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum AnimType
    {
        Idle,
        Move,
        Attack
    }


    public AnimType animType = AnimType.Idle;
    public Animator _animator;
    public int attackDamage;
    public GameObject target;
    Vector3 targetVec;
    Vector3 rotateVec;
    PlayerMove playerMove;

    public LayerMask enemyLayer;

    int IdleHash = Animator.StringToHash("Idle");
    int WalkHash = Animator.StringToHash("Walk");
    int AttackHash = Animator.StringToHash("Attack");

    //ü��
    int curHp;
    int maxHp;
    public Image hpBar;
    public Text hpText;

    //�� ����
    public static GameObject backupTarget;
    public GameObject targetStatus;
    public static Image targetHpBar;
    public static Text targetHp;

    //���
    public GameObject DiePanel;
    public ParticleSystem hitEff;

    //���
    public Text goldText;

    private void Start() => StartFunc();

    private void StartFunc()
    {
        maxHp = GlobalData.beforeStatus[(int)GlobalData.statues.Hp] + GlobalData.itemStatus[(int)GlobalData.statues.Hp];
        attackDamage = GlobalData.beforeStatus[(int)GlobalData.statues.NormalAtk] + GlobalData.itemStatus[(int)GlobalData.statues.NormalAtk];
        playerMove = GetComponent<PlayerMove>();

        _animator = GetComponent<Animator>();
        curHp = maxHp;
        hpText.text = curHp.ToString() + " / " + maxHp;
        if (targetStatus == null)
            return;
        targetHp = targetStatus.GetComponentInChildren<Text>();
        targetHpBar = targetStatus.GetComponentsInChildren<Image>()[1];

        goldText.text = "Gold : " + GlobalData.playerGold.ToString();
    }

    private void Update() => UpdateFunc();

    private void UpdateFunc()
    {
        if (GlobalData.gameStop)
            return;

        TargetInfo();

        if(animType == AnimType.Attack)
            RotationFunc();
    }

    public void TargetInfo()
    {
        if (targetStatus == null)
            return;

        if (backupTarget == null)
        {
            targetStatus.SetActive(false);
            return;
        }
        else
            targetStatus.SetActive(true);

        if (!backupTarget.gameObject.activeSelf)
        {
            backupTarget = null;
        }
    }

    public static void SetBackUpTarget(GameObject target)
    {
        backupTarget = target;
        

        RefreshTargetInfo();
    }

    public static void RefreshTargetInfo()
    {
        if (backupTarget == null)
            return;

        int targetcurhp = backupTarget.GetComponent<GetHp>().GetCurHp();
        int targetmaxhp = backupTarget.GetComponent<GetHp>().GetMaxHp();
        targetHpBar.fillAmount = (float)targetcurhp / targetmaxhp;
        targetHp.text = targetcurhp.ToString()  + "/" +  targetmaxhp.ToString();
    }


    public void RotationFunc()
    {
        if (target == null)
        {
            return;
        }

        rotateVec = target.transform.position - this.transform.position;

        Quaternion a_TargetRot = Quaternion.LookRotation(rotateVec);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                a_TargetRot,
                                Time.deltaTime * 10);
    }
    
    public void ChangeAnim(AnimType type)
    {
        if (animType == type)
            return;

        animType = type;
        
        switch(animType)
        {
            case AnimType.Idle:
                _animator.SetTrigger(IdleHash);
                break;

            case AnimType.Move:
                _animator.SetTrigger(WalkHash);
                break;

            case AnimType.Attack:
                _animator.SetTrigger(AttackHash);
                break;
        }
    }

    public void AttackEnemy()
    {
        Collider[] colls = Physics.OverlapSphere(this.transform.position, 1.5f, enemyLayer);
        if (colls.Length > 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].TryGetComponent(out IDamageable damage))   //������ �������̽� �Լ��� ������ ������
                {
                    targetVec = colls[i].transform.position - this.transform.position;  //���� ���� ���ϱ�

                    if (Vector3.Dot(transform.forward, targetVec.normalized) < 0.45f)   // 45�� ���� ���� ���� ���� ������
                        continue;

                    //������ ó�� �� �ǰ� ����Ʈ Ȱ��ȭ
                    damage.TakeDamage(attackDamage);
                    damage.PlayEff();

                    // ������ ����� �� �� Ÿ���̰�, ���� ���
                    if (colls[i].gameObject == target && colls[i].gameObject.layer == 9) // 9 == Corpse Layer
                    {
                        target = null;          //�� Ÿ�� Null
                        backupTarget = null;    //ü�� ǥ�ø� ���� backUp������Ʈ Null
                        ChangeAnim(AnimType.Idle); //Idle �ִϸ��̼� ����
                        return;
                    }
                }


            }
            RefreshTargetInfo();    // ���� ����� Hp���� Refresh
        }
    }

    public void GetGold(int val = 20)
    {
        GlobalData.playerGold += val;
        this.goldText.text = "Gold : " + GlobalData.playerGold.ToString();

    }

    public void Heal(int val)
    {
        curHp += val;
        if (curHp >= maxHp)
            curHp = maxHp;

        hpBar.fillAmount = (float)curHp / maxHp;
        hpText.text = curHp.ToString() + " / " + maxHp;
    }

    public void TakeDamage(int val)
    {
        if (curHp <= 0)
            return;

        curHp -= val;
        hitEff.Play();

        if (curHp <= 0)
        {
            curHp = 0;
            _animator.SetTrigger("Die");
            GlobalData.gameStop = true;

            playerMove._aipath.destination = this.transform.position;
            playerMove.canMove = false;
            DiePanel.gameObject.SetActive(true);
            
            //this.gameObject.SetActive(false);
        }

        hpBar.fillAmount = (float)curHp / maxHp;
        hpText.text = curHp.ToString() + " / " + maxHp;
    }
}