using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageElementBase : MonoBehaviour
{
    public static Player player => Player.player;

    protected virtual float damage
    {
        get { return 0; }
    }

    public virtual float repeatingTime
    {
        get { return 1; }
    }

    public virtual void DoHit()
    {
        player.TakeHit(damage);
        // TODO: Do sounds or something here
    }

    public virtual void DoExit()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) {
            InvokeRepeating(nameof(DoHit), .1f, repeatingTime);

            collider.gameObject.GetComponent<PlayerPhysics>().damageElementBases.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Reset();
        Player.player.physics.damageElementBases.Remove(this);
    }

    public void Reset()
    {
        CancelInvoke(nameof(DoHit));
        DoExit();

        // this throws an exception but avoids the bug
        Player.player.physics.damageElementBases.Remove(this);
    }
}