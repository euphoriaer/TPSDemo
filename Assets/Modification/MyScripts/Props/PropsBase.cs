using UnityEngine;

public class PropsBase : MonoBehaviour
{
    public float PropRotatoSpeed;
    // Start is called before the first frame update
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        this.transform.Rotate(0f, PropRotatoSpeed, 0);
    }

    public void OnTriggerEnter(Collider otherCollider)
    {
        //if (otherCollider.tag != "Player")
        //{
        //    return;
        //}
        //Debug.Log(this.GetType().ToString() + "  ±»¼ñ×ßÁË");
    }
}