using UnityEngine;

public class GrenadeWeapon : Weapon
{
    public float InitHighSpeed;
    public float forwardSpeed;
    public float coolTime;

    private Rigidbody Grenade;

 

    public override void Fire(params object[] message)
    {
        Transform spawnPoint = message[0] as Transform;
        GameObject player= message[1] as GameObject;

        GameObject obj = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        obj.transform.forward = player.transform.forward;//设置方向

        Grenade = obj.GetComponent<Rigidbody>();//设置速度

        Vector3 initSpeed = new Vector3(obj.transform.forward.x* forwardSpeed, InitHighSpeed, obj.transform.forward.z* forwardSpeed);
        Grenade.velocity = initSpeed;
       


       
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    private bool tempOnce = true;

    // Update is called once per frame
    private void Update()
    {
    
    }
}