using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public State estados;
    public float distanceFollow;
    public float distanceDead;
    public float distanceAtack;


    public void checkState()
    {
        switch (estados)
        {
            case State.idle:
                StateIdle();
                break;
            case State.seguir:
                StateFollow();
                break;
            case State.muerto:
               // StateMuerto();
                break;
            case State.atacar:
                State()
                break;  
            
        }
    }
    public virtual void StateIdle()
    {

    }
    public virtual void StateWalk()
    {

    }
    public virtual void StateAtack()
    {

    }
    public virtual void StateFollow()
    {

    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, distanceAtack);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanceFollow);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanceDead);
        Handles.color = Color.green;
    }

}

public enum State
{
   idle=0,
   seguir=1,
   atacar=3,
   muerto=4
 
}    
