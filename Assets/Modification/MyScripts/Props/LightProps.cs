using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightProps : PropsBase
{
    // Start is called before the first frame update
    public LightProps()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider otherCollider)
    {
        base.OnTriggerEnter(otherCollider);
        Player player = GameObject.Find("MyModification").GetComponent<Player>();
        if (player != null)
        {
            player.GetProps(this);
        }
    }
}
