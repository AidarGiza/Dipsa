
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float normalDamage;
    public float normalSpeed;
    public float damage;
    Animator animator;

    GameObject side;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject == core)
    //    {
    //        other.gameObject.GetComponent<Core>().MakeDamage(damage);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Core")
    //    {
    //        collision.gameObject.GetComponent<Core>().MakeDamage(damage, side.name);
    //        Destroy(this.gameObject);
    //    }
    //}

    public void Kill()
    {
        animator.SetBool("isDying", true);
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 0.3f);
        GetComponent<MovingObject>().IsDying = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        try
        {
            if (collision.tag == "CoreSide")
            {
                side = collision.gameObject;
            }
            if (collision.gameObject.tag == "Core")
            {
                collision.gameObject.GetComponent<Core>().MakeDamage(damage, side.name);
                Destroy(this.gameObject);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    side = other.gameObject;
    //}
}
