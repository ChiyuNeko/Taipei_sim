using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;


public abstract class CharacterBase
{
   float hp;
   float MoveSpeed;
   UnityEvent IsLiveEvent;
   UnityEvent IsHurtEvent;
   UnityEvent IsPanicEvent;
   UnityEvent IsDeadEvent;
   
   public abstract void IsLive();
   public abstract void IsHurt();
   public abstract void IsPanic();
   public abstract void IsDead();
}

[Serializable]

public class Character : CharacterBase
{
    public float hp;
    public float MoveSpeed;
    public int DeathTimes;
    public UnityEvent IsLiveEvent;
    public UnityEvent IsHurtEvent;
    public UnityEvent IsPanicEvent;
    public UnityEvent IsDeadEvent;
    public UnityEvent InteractEvent;
    public override void IsLive()
    {
        IsLiveEvent?.Invoke();
    }
    public override void IsHurt()
    {
        IsHurtEvent?.Invoke();
    }
    public override void IsDead()
    {
        IsPanicEvent?.Invoke();
    }
    public override void IsPanic()
    {
        IsDeadEvent?.Invoke();
    }
    public void Interact()
    {
        InteractEvent?.Invoke();
    }
}


public class CharacterManerger : MonoBehaviour
{
    public enum STATUS
    {
        IsLive,
        IsHurt,
        IsDead,
        IsPanic

    }
    public STATUS Status;
    
    [SerializeField]
    private Character Character;

    public void Update()
    {

    }

        
}

