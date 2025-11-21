using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpeedTest : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude;
        float value=Mathf.Round(speed);
        text.text = "Speed: " + value;

        
    }
}
