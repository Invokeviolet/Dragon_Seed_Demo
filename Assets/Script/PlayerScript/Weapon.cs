using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dragon"))
        {
            Time.timeScale = 0.35f;
            Invoke("TimeBack",0.1f);
            other.SendMessage("DragonTransferDamage", damage,SendMessageOptions.DontRequireReceiver);
        }
    }

    private void TimeBack()
    {
        Time.timeScale = 1f;
    }
}
