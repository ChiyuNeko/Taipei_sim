using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using UI_Control;
using ExitGames.Client.Photon.StructWrapping;


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
    public UnityEvent RespawnEvent;
    public UnityEvent FinishEvent;
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
    public void Respawn()
    {
        RespawnEvent?.Invoke();
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
    public DeathUI deathUI;
    public Character Character;
    public Vector3 DeathPos;

    public void Update()
    {
        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     if(Status != STATUS.IsDead)
        //     {
        //         Character.IsDeadEvent?.Invoke();
        //         deathUI.startTime = Time.time;
        //         Status = STATUS.IsDead;
        //         Character.DeathTimes++;
        //         Debug.Log("dead");
        //         DeathPos = this.transform.position;
        //     }
        // }
        if(deathUI.delta <= 0)
        {
            if( Status != STATUS.IsLive)
            {
                Character.RespawnEvent?.Invoke();
                Status = STATUS.IsLive;
                Debug.Log("live");
            }
        }
        if(Status == STATUS.IsDead)
        {
            this.transform.position = DeathPos;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        string Tag = other.transform.tag;
        if(Tag == "Cars" || Tag == "brokenBuild" || Tag == "hug-IceBall")
        {
            if(Status != STATUS.IsDead)
            {
                deathUI.delta = 5;
                Debug.Log(deathUI.delta);
                deathUI.startTime = Time.time;
                Character.IsDeadEvent?.Invoke();
                Status = STATUS.IsDead;
                Character.DeathTimes++;
                DeathPos = this.transform.position;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string Tag = other.transform.tag;
        if(Tag == "SavePoint")
        {
            Character.FinishEvent?.Invoke();
        }
    }

        
}

