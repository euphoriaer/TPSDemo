using DigitalRubyShared;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerGameobject; 
    private PlayerMovementAndLook playerMovementAndLook;

    [Header("touch")]
    public FingersJoystickScript JoyMoveScript;//控制移动
    public FingersJoystickScript JoyShootScript;//控制方向和射击
    private Vector3 rotateDirection;

    private bool isOn = false;

    // myTodo 优化朝向问题
    // ：当移动时候，移动控制方向，另一个失灵；当射击时候，射击控制方向，另一个失灵
    public Collider2D MaskMove;

    public Collider2D MaskShoot;

    public Weapon weapon;
    private float timer;

    [Header("人物属性")]
    public float blood;
    public float speed;

    public GameObject light;
    public GameObject lightPoint;
    public Transform gunPoint;

    private void Awake()
    {
        playerGameobject = GameObject.Find("Player_Prefab").gameObject;
        playerMovementAndLook = playerGameobject.GetComponent<PlayerMovementAndLook>();
        rotateDirection = playerGameobject.transform.forward;
        JoyMoveScript.JoystickExecuted = JoystickMove;
        JoyShootScript.JoystickExecuted = JoystickShoot;

        JoyShootScript.JoystickCallback.AddListener(Fire);

        playerMovementAndLook.CollAction += OnCollisionPlayer;//玩家被碰撞  受击
    }

    private void OnCollisionPlayer(Collision obj)
    {
    }

    public void Fire(Vector2 arg0)
    {
        bool turn = JoyShootScript.Executing;
        if (!turn)
        {
            isOn = !isOn;
        }

        if (weapon.GetType() == typeof(GunWeapon))
        {
            var gun = weapon as GunWeapon;
            if (timer > gun.speed)
            {
                weapon.Fire(gunPoint);
                timer = 0f;
            }
            
        }

        if (weapon.GetType() == typeof(LaserWeapon))
        {
            weapon.Fire( gunPoint);
        }

        if (weapon.GetType() == typeof(GrenadeWeapon))
        {
            var grenade = weapon as GrenadeWeapon;
            if (timer > grenade.coolTime)
            {
                weapon.Fire(gunPoint,playerGameobject);
                timer = 0f;
            }
        }
    }

    private void JoystickShoot(FingersJoystickScript arg1, Vector2 arg2)
    {
        if (arg2 == Vector2.zero)
        {
            return;
        }

        rotateDirection = new Vector3(-arg2.x, 0, -arg2.y);
        playerGameobject.transform.forward = rotateDirection;
    }

    private void JoystickMove(FingersJoystickScript arg1, Vector2 arg2)
    {
        playerMovementAndLook.h = arg2.x;
        playerMovementAndLook.v = arg2.y;
    }

    private void OnEnable()
    {
        FingersScript.Instance.AddMask(MaskMove, JoyMoveScript.PanGesture);
        FingersScript.Instance.AddMask(MaskShoot, JoyShootScript.PanGesture);
    }

    private void LateUpdate()
    {
        //Vector3 lookAtDir = (playerGameobject.transform.position - playerGameobject.transform.position).normalized;
        //Quaternion lookAtRotation = Quaternion.LookRotation(lookAtDir, playerGameobject.transform.up);
        //Quaternion currentRotation = playerGameobject.transform.rotation;
        //playerGameobject.transform.rotation = Quaternion.Lerp(currentRotation, lookAtRotation, ZoomLookAtSpeed * Time.deltaTime);
    }

    // Start is called before the first frame update
    private void Start()
    {
        light.transform.position = lightPoint.transform.position;
        light.transform.forward = lightPoint.transform.forward;
        light.transform.parent = lightPoint.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if (!isOn)
        {
            weapon.Stop( gunPoint);
        }
    }

    public void GetProps(PropsBase prop)
    {
        //Weapons
        if (prop.GetType() == typeof(WeaponProps))
        {
            var weaponProp = prop as WeaponProps;
            Debug.Log("我捡走了" + weaponProp.Weapon);
            weapon.Stop(gunPoint);//停止上一个武器
            weapon = weaponProp.Weapon;
        }

        //Light
        if (prop.GetType() == typeof(LightProps))
        {
            var lightProps = prop as LightProps;
            Debug.Log("我捡走了" + prop.GetType());
            light = lightProps.gameObject;
            light.transform.parent = lightPoint.transform;
            light.transform.forward = lightPoint.transform.forward;
        }
    }
}