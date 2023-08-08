using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiag : Enemy
{
    protected override void Start()
    {
        this.transform.rotation =  Quaternion.Euler(
           new Vector3(0, 0, Random.Range(-45, 46)));
        base.Start();
        if (Random.Range(0f, 1f) > .9f)
        {
            SetShield();
        }
    }

    protected override void FireWeapon()
    {
    }
}