using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public States state;
    public float distanceFollow;
    public float distanceAtack;


    private void CheckState()
    {
        switch (state)
        {   
             case States.idle:
                StateIdle();
                break;
             case States.walk:
                StateWalk();
                break;
             case States.running:
                StateRunning();
                break;
             case States.atack:
                StateAtack();
                break;  
             case States.follow:
                StateFollow();
                break;

        }
    }
    public virtual void StateIdle()
    {

    }
    public virtual void StateWalk()
    {

    }
    public virtual void StateRunning()
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
        Handles.color= Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.up, distanceAtack);
        Handles.color= Color.black;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanceFollow);
       
        
    }

}

public enum States
{
    idle = 0,
    atack= 1,
    walk = 2,
    running= 3,
    follow= 4,
}
