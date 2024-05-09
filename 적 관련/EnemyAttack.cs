using UnityEngine;
using Pathfinding;

public class EnemyAttack : MonoBehaviour, IAttackPlayer
{
    GameObject playerObj;
    Player _player;

    Vector3 playerVec;
    Enemy enemy;

    float playerDist;
    public float CheckDist;

    private void Start() => StartFunc();

    private void StartFunc()
    {
        InitMonster();
    }

    private void Update() => UpdateFunc();

    private void UpdateFunc()
    {
        CheckDistFromPlayer();
    }

    void CheckDistFromPlayer()
    {
        //�÷��̾ �����ϰ� �ڽ��� ü���� 0 �̻��� ��
        if(playerObj != null && enemy.curHp > 0)
        {
            //���� ���� �� �Ÿ� ���ϱ�
            playerVec = playerObj.transform.position - this.transform.position;
            playerDist = playerVec.magnitude;
            enemy._animator.SetFloat("DistanceFromPlayer", playerDist);

            //�÷��̾� �߰� �� �÷��̾� ����
            if (enemy.findPlayer)
                enemy._aipath.destination = playerObj.transform.position;

            if (playerDist <= CheckDist)
            {
                if (Vector3.Dot(transform.forward, playerVec.normalized) < 0.6f && !enemy.findPlayer)
                {
                    return;
                }

                //���� �� �������� �ʵ���
                if (playerDist <= 1.5f)
                {
                    enemy._aipath.destination = this.transform.position;
                    return;
                }

                EnemyFind();
            }
        }
    }

    public void EnemyFind()
    {
        if (enemy.findPlayer)
            return;

        enemy.findPlayer = true;
        enemy._animator.SetBool("FindPlayer", enemy.findPlayer);
        enemy._animator.SetTrigger("Hitted");
    }

    public void InitMonster()
    {
        playerObj = GameObject.Find("Player");
        _player = playerObj.GetComponent<Player>();

        enemy = GetComponent<Enemy>();
    }

    public void AttackPlayer()
    {
        if (GlobalData.gameStop)
            return;

        if (Vector3.Dot(transform.forward, playerVec.normalized) >= 0.5f && playerDist <= 2.5f)
        {
            playerObj.GetComponent<Player>().TakeDamage(enemy.attackDamage);
        }
    }

    public void LookPlayer()
    {
        Vector3 rot = _player.transform.position - this.transform.position;
        rot.y = 0;

        this.transform.rotation = Quaternion.LookRotation(rot);
    }

    public void SetAttack()
    {
        if (enemy.findPlayer)
            return;

        enemy.findPlayer = true;
        enemy._animator.SetBool("FindPlayer", enemy.findPlayer);
        enemy._animator.SetTrigger("Hitted");
    }
}