using UnityEngine;
using System.Collections;

public class SwipeSword : MonoBehaviour
{
	void OnTriggerEnter(Collider hit)
    {
        SendMessageUpwards("AttackSword", hit, SendMessageOptions.DontRequireReceiver);
    }
}
