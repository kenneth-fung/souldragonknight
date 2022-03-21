using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Combat combat;

    public abstract Movement Movement { get; }
    public Combat Combat { get => combat; }

    public int NetworkViewId { get => photonView.ViewID; }
    public bool IsNetworkOwner { get => photonView.IsMine; }

    public static ActorController GetActorFromCollider(Collider2D collider)
    {
        return collider.GetComponentInParent<ActorController>();
    }

    protected virtual void OnEnable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.AddListener(HandleHurtEvent);
            Combat.DeathEvent.AddListener(HandleDeathEvent);
        }
    }

    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.RemoveListener(HandleHurtEvent);
            Combat.DeathEvent.RemoveListener(HandleDeathEvent);
        }
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            Combat.CombatStateMachine.Update();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Movement.MovementStateMachine.Update();
        }
    }

    public void Interact(Interactable interactable)
    {
        interactable.Interact(this);
    }

    protected virtual void HandleHurtEvent() { }

    protected virtual void HandleDeathEvent() { }
}
