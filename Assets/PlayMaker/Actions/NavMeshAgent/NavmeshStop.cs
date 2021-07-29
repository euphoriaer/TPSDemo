using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.NavMeshAgent)]
    [Tooltip("停止Navmesh的行动")]
    public class NavmeshStop : FsmStateAction
    {
        public FsmOwnerDefault navMeshObject;
        private NavMeshAgent navMeshAgent;

        private GameObject go;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            go = Fsm.GetOwnerDefaultTarget(navMeshObject);
            navMeshAgent = go.GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped=true;
        }

        public override void OnExit()
        {
            navMeshAgent.isStopped = false;
            base.OnExit();
        }
    }
}