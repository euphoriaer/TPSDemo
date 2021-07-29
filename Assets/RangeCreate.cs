using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeCreate : MonoBehaviour
{

    public GameObject Enemy;

    public int number;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 position= GetNavMeshRandomPos();
           var obj= GameObject.Instantiate(Enemy, position, Quaternion.identity,this.transform);
           NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
           agent.Warp(position);
           if (obj.transform.position.y>=3.78f)
           {
               Destroy(obj);
           }
        }   
    }
    //Navmesh Ëæ»úÈ¡µã
    public Vector3 GetNavMeshRandomPos()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);

        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
