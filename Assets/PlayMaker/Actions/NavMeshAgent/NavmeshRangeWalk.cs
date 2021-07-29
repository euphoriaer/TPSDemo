using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.NavMeshAgent)]
    [Tooltip("基于Navmesh的随机游荡")]
    public class NavmeshRangeWalk : FsmStateAction
    {
        public FsmOwnerDefault navMeshObject;

        private NavMeshAgent navMeshAgent;

        private GameObject go;

        public override void Awake()
        {
        }

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            go = Fsm.GetOwnerDefaultTarget(navMeshObject);
            navMeshAgent = go.GetComponent<NavMeshAgent>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (navMeshAgent.velocity == Vector3.zero)//当前处于停止则 随机游荡
            {
                ;
                Vector3 rangeVector3 = GetNavMeshRandomPos();

                navMeshAgent.SetDestination(rangeVector3);
            }
        }

        //Navmesh 随机取点
        public Vector3 GetNavMeshRandomPos()
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            int t = Random.Range(0, navMeshData.indices.Length - 3);

            Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
            point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

            return point;
        }

        
    }
}