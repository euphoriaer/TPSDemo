using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MosterBase : MonoBehaviour
{
    public PlayMakerFSM fsm;

    public Player player;
    private AnimatorStateInfo bornInfo;
    private NavMeshAgent navMeshAgent;
    private GameObject playerObj;

    [Header("��ʬ����")]
    public float blood = 100;

    public float hurt;
    public float pursueSpeed;
    public float FollowRange;
    public float AttackRange;
    // Start is called before the first frame update

    public ParticleSystem hurtBloodGun;
    public ParticleSystem hurtBloodLaser;
        

    public void Start()
    {
        fsm = this.GetComponent<PlayMakerFSM>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.Find("MyModification").GetComponent<Player>();
        playerObj = GameObject.Find("Player_Prefab");
    }

    private void OnColliderEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("��ʬ���ұ��ӵ�������" + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider theCollider)
    {
        if (theCollider.gameObject.tag == "Bullet")
        {
            Debug.Log("��ʬ���ұ��ӵ�������" + theCollider.gameObject.name);
          
            //myTodo ��͸�������ټ�����͸ ����
            //myTodo  ������ʬ��������Ч
            Destroy(theCollider.gameObject);
            GetHurtGun(player.weapon.hurt);
        }

        if (theCollider.gameObject.tag == "Grenade")
        {
            Debug.Log("��ʬ���ұ����ִ����� " + theCollider.gameObject.name);
            GetHurtGun(player.weapon.hurt);
        }
    }

    public void Die()
    {
        fsm.SendEvent("Die");
        this.gameObject.GetComponent<Collider>().enabled = false;
        hurtBloodLaser.Stop(true);
        hurtBloodGun.Stop(true);
    }

    // Update is called once per frame
    public void Update()
    {
        float CrrentFloat = Vector3.Distance(this.transform.position, playerObj.transform.position);

        if (CrrentFloat <= FollowRange)
        {
            fsm.SendEvent("FindPlayer");
        }

        if (CrrentFloat <= AttackRange)
        {
            fsm.SendEvent("InAttackRange");
        }
        else
        {
            fsm.SendEvent("LeaveAttackRange");
        }
    }

    public void GetLaser()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
        }
    }

    #region AnimationEvent

    public void HurtPlayer()
    {
        Debug.Log("��ʬ�����˹�����player��Ѫ");
        player.blood -= hurt;
    }

    public void BornFinish()
    {
        fsm.SendEvent("Born");
    }

    public void DieEnd()
    {
        Timer time = Timer.Register(20f, () =>
        {
            Destroy(this);
        }, autoDestroyOwner: this);
    }

    #endregion AnimationEvent

    public void GetHurtGun(float hurt)
    {
        hurtBloodGun.Play(true);
        blood -= hurt;
        if (blood <= 0)
        {
            Die();
        }
    }

    public void GetHurtLaser(float hurt)
    {
        
        blood -= hurt;
        if (blood <= 0)
        {
            Die();
        }

        if (!hurtBloodLaser.isPlaying)
        {
            hurtBloodLaser.Play(true);
        }
    }
  
}