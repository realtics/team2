using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    
    Rigidbody rb;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal"); //좌우 방향키 체크
        float vertical = Input.GetAxis("Vertical");     //위아래 방향키 체크
        bool playerStop = Input.GetKey(KeyCode.Keypad0);

        if (playerStop == true)
        {
            Vector3 movement = new Vector3(0f, 0f, 0f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Vector3 movement = new Vector3(horizontal, 0f, vertical);//입력받은 방향대로 벡터값을 저장
            rb.AddForce(movement * speed);  //rigidbody에 힘을 가함
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            other.gameObject.SetActive(false);

            count += 1;
            SetCountText();
        }
        
        if(count >= 8)
        {
            winText.text = "Game End";
        }
    }

    void SetCountText()
    {
        countText.text = "Count : " + count.ToString();
    }
}
