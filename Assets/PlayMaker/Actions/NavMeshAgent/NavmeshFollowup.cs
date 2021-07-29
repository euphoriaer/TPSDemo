using System;
using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.NavMeshAgent)]
    [Tooltip("追击")]
    public class NavmeshFollowup : FsmStateAction
    {
        public FsmOwnerDefault navMeshObject;
        public GameObject target;

        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private GameObject go;
        private float speed;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            go = Fsm.GetOwnerDefaultTarget(navMeshObject);
            navMeshAgent = go.GetComponent<NavMeshAgent>();
            speed = go.GetComponent<MosterBase>().pursueSpeed;
            animator= go.GetComponent<Animator>();
            target = GameObject.Find("Player_Prefab");
            animator.speed = speed * animator.speed;
            navMeshAgent.speed = speed * navMeshAgent.speed;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            navMeshAgent.SetDestination(target.transform.position);
        }

        public override void OnExit()
        {
            animator.speed =  animator.speed/speed;
            navMeshAgent.speed =  navMeshAgent.speed/ speed;
            base.OnExit();
        }
    }
}