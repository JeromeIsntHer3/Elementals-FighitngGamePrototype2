using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPredictionBox : GameBox
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent(out IHitbox hitbox))
        {
            if (hitbox.Data().Source.gameObject == owner.gameObject) return;
            Vector2 dir = owner.transform.position - hitbox.Data().Source.transform.position;
            hitbox.Data().Direction = dir;
            owner.GetComponent<CharacterInput>().OnDefend?.Invoke(this, System.EventArgs.Empty);
        }
    }
}