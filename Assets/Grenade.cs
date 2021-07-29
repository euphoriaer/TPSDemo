using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody GrenadeRig;
    public ParticleSystem Boom;

    // Start is called before the first frame update
    private void Start()
    {
        GrenadeRig = this.GetComponent<Rigidbody>();
        Boom.Stop();
    }

    private bool tempOnce = true;

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("手雷碰到了：" + other.tag);

        Debug.Log("手雷爆炸");
        Boom.Play(true);
        this.gameObject.GetComponent<ParticleSystem>().Stop(false);
    }
}