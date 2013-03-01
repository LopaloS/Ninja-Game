using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
    MenuNGUI menuNgui;
    TPScamera camera;
    Transform viewPoint;
    public  float hitPoints = 100;
    public GameObject deadNinja;
    GameObject ninja;

	// Use this for initialization
	void Start () 
    {
        ninja = GameObject.Find("Player");
        camera = GetComponentInChildren<TPScamera>();
        menuNgui = GameObject.Find("MenuNgui")
               .GetComponentInChildren<MenuNGUI>();
        viewPoint = HelpMethods.GetTransform(ninja.transform, "View point").transform;
	}

    void CauseDamage(float damage)
    {
        if (hitPoints <= 0.0f)
        {
            return;
        }
        hitPoints -= damage;
        if (hitPoints <= 0.0f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y + 180, transform.eulerAngles.z);
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        camera.transform.parent = null;
        viewPoint.parent = null;
        Destroy(ninja);
        Instantiate(deadNinja, ninja.transform.position, ninja.transform.rotation);
        yield return new WaitForSeconds(2.0f);
        Screenfade.screenFade = ScreenState.fade;
        yield return new WaitForSeconds(2.0f);
        Destroy(menuNgui);
        Application.LoadLevel(0);
    }
}
