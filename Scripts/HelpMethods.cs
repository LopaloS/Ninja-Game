using UnityEngine;
using System.Collections;

public class HelpMethods : MonoBehaviour
{
     static public float AngleClamp(float angle) 
    {
		if(angle > 360) angle -= 360;
		if(angle < - 360) angle += 360;
		return angle;
	}

    static public Transform GetTransform(Transform gO, string name)
    {
        Transform[] objectTransform = gO.GetComponentsInChildren<Transform>();
        foreach (Transform leg in objectTransform)
        {
            if (leg.name == name)
                return leg;
        }
        return null;
    }
}

