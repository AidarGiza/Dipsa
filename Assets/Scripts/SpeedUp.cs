using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public int duration;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Core")
        {
            collision.gameObject.GetComponent<Core>().SendBonusActionToManager(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
