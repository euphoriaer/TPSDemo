using UnityEngine;
using UnityEngine.Rendering.UI;

public class LaserWeapon : Weapon
{
    public LineRenderer Instance;
    public EGA_Laser LaserScript;
    private GameObject laserObj;

    public override void Fire(params object[] message)
    {
        Transform spawnPoint = message[0] as Transform;
        if (laserObj == null)
        {
            laserObj = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.rotation);
            laserObj.transform.parent = spawnPoint;
            Instance = laserObj.GetComponent<LineRenderer>();
            LaserScript = Instance.GetComponent<EGA_Laser>();
        }

        laserObj.SetActive(true);

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.forward, out hit)) //CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
        {
            if (hit.transform.tag == "Enemy")
            {
                MosterBase enemy = hit.transform.gameObject.GetComponent<MosterBase>();
                if (enemy == null)
                {
                    return;
                }
                enemy.GetHurtLaser(hurt);
               
            }
        }
        //myTodo 激光变粗
    }

    public override void Stop(params object[] message)
    {
        Transform spawnPoint = message[0] as Transform;
        if (spawnPoint==null)
        {
            return;
        }
        if (laserObj == null)
        {
            laserObj = Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.rotation);
            laserObj.transform.parent = spawnPoint;
            Instance = laserObj.GetComponent<LineRenderer>();
            LaserScript = Instance.GetComponent<EGA_Laser>();
        }
        laserObj.SetActive(false);
        //myTodo 激光还原
        return;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}