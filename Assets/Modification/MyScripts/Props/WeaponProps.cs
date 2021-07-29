using UnityEngine;

public class WeaponProps : PropsBase
{
    public Weapon Weapon;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        base.Update();
    }

    public void OnTriggerEnter(Collider otherCollider)
    {
        base.OnTriggerEnter(otherCollider);
        if (otherCollider.tag!="Player")
        {
            return;
        }
        Player player = GameObject.Find("MyModification").GetComponent<Player>();
        if (player != null)
        {
            player.GetProps(this);
        }
        Destroy(this.gameObject);
    }
}