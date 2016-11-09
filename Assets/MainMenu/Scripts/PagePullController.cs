using UnityEngine;
using System.Collections;

public class PagePullController : MonoBehaviour {
    
    public UnityEngine.UI.Button button;
    public int State = 2;
	// Use this for initialization
	void Start () {
        UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //find parent
        GameObject page = button.gameObject.transform.parent.gameObject;
        page = page.transform.parent.gameObject;
        //return all other pages
        GameObject panel = page.transform.parent.gameObject;
        Animator anim;

        print(page);
        print(panel);

        for (int i = 0; i < panel.transform.childCount; i++)
        {
            GameObject scan = panel.transform.GetChild(i).gameObject;
            if (scan != page)
            {
                if (scan.name.Contains("Page"))
                {
                    anim = scan.GetComponent<Animator>();
                    anim.SetInteger("State", 2); //return page
                }
            }
        }
        anim = page.GetComponent<Animator>();

        //1 pullout, 2 putback, 3 normal
        State = anim.GetInteger("State");

        if (State == 1)
            State = 2;
        else if (State == 2)
            State = 1;
        else //invalid state go to normal
            State = 2;

        anim.SetInteger("State", State);
    }
}
