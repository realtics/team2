using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
    public Text logText = null;
    public ScrollRect scroll_rect = null;

    // Start is called before the first frame update
    void Start()
    {
        logText = GameObject.Find("log_Text").GetComponent <Text> ();
        scroll_rect = GameObject.Find("Scroll_View").GetComponent <ScrollRect> ();

        if (logText != null)
            logText.text += "Hello Log Window" + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            logText.text += "asdf " + "\n";
        }
    }
}
