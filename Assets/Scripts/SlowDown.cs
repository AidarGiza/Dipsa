using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    public int duration;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Core")
        {
            collision.gameObject.GetComponent<Core>().SendBonusActionToManager(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
