using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlaneEnemyRocket : AirplaneRocket
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        Destroy(gameObject);
    }
}
