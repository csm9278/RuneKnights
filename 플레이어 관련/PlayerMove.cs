using UnityEngine;
using Pathfinding;

public class PlayerMove : MonoBehaviour
{

    RaycastHit groundHit;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public Camera heroCamera;
    public AIPath _aipath;
    public GameObject pickingEff;
    Skill skill;

    public bool canMove = true;

    //���� �Ÿ� üũ
    Player player;
    int distanceFromWayPointHash = Animator.StringToHash("DistanceFromWayPoint");

    Vector3 targetPoint = Vector3.zero;
    Vector3 targetDir = Vector3.zero;
    float targetDist = 0.0f;

    //��ŷ ����
    //GameObject target;
    Vector3 targetVec = Vector3.zero;
    bool isAttack = false;

    private void Start() => StartFunc();

    private void StartFunc()
    {
        _aipath = GetComponent<AIPath>();
        targetMask = 1 << 7 | 1 << 8;

        player = GetComponent<Player>();
        targetPoint = this.transform.position;

        heroCamera = Camera.main;
        skill = GetComponent<Skill>();
    }

    private void Update() => UpdateFunc();

    private void UpdateFunc()
    {
        if (GlobalData.gameStop)
            return;

        if (!canMove)
            return;

        PickMoveUpdate();
        CheckDistanceFromWayPoint();
        AttackUpdate();

    }

    void PickMoveUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !skill.casting)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = heroCamera.farClipPlane;

            Vector3 dir = heroCamera.ScreenToWorldPoint(mousePos);

            Debug.DrawRay(heroCamera.transform.position, dir, Color.red, 5.0f);
            Ray ray = new Ray(heroCamera.transform.position, dir);
            if (Physics.Raycast(ray, out groundHit, mousePos.z, targetMask))
            {
                //���� Ŭ��
                if (groundHit.collider.gameObject.layer == 7)
                {

                    //������ �ʹ� �������� Ŭ�� �� ����
                    Collider[] coll = Physics.OverlapSphere(groundHit.point, 0.75f, obstacleMask);

                    if (coll.Length > 0)
                    {
                        return;
                    }

                    player.target = null;   //���� �������� Ÿ�� null ó��
                    pickingEff.SetActive(true); //�̵� ��ũ Ȱ��ȭ

                    player.ChangeAnim(Player.AnimType.Move);    // �ִϸ��̼� ����

                    isAttack = false;
                    pickingEff.transform.position = groundHit.point;
                    _aipath.destination = groundHit.point;
                    targetPoint = groundHit.point;
                }
                else if(groundHit.collider.gameObject.layer == 8)   //�� Ŭ��
                {

                    //���� �������� Ÿ���� �ƴ϶��
                    if (player.target != groundHit.collider.gameObject)
                    {
                        isAttack = false;
                    }
                    else //���� �������� Ÿ���̶�� ����
                        return;

                    player.ChangeAnim(Player.AnimType.Move);
                    player.target = groundHit.collider.gameObject;  //Ÿ�� ����
                    Player.SetBackUpTarget(player.target);  //�� ü�� ǥ�� UI�� ���ӿ�����Ʈ ���

                    pickingEff.SetActive(true);
                    pickingEff.transform.position = player.target.transform.position;
                    targetPoint = player.target.transform.position;
                    _aipath.destination = player.target.transform.position;

                }

            }
        }
    }

    void AttackUpdate()
    {
        if (player.target == null)
            return;

        targetVec = player.target.transform.position - this.transform.position;
        if(targetVec.magnitude <= 1.5f)
        {
            if (!isAttack)
            {
                _aipath.destination = this.transform.position;
                pickingEff.SetActive(false);
                player.ChangeAnim(Player.AnimType.Attack);

                isAttack = true;
            }
        }
        else
        {

        }

    }

    void CheckDistanceFromWayPoint()
    {
        targetDir = targetPoint - this.transform.position;
        targetDist = targetDir.magnitude;

        if (targetDist <= 0.2f && pickingEff.activeSelf)
        {
            pickingEff.SetActive(false);
            player.ChangeAnim(Player.AnimType.Idle);

        }
        player._animator.SetFloat(distanceFromWayPointHash, targetDist);
    }
}