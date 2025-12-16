using UnityEngine;

public class HintCollider : MonoBehaviour
{
    private bool hintShown = false;

    public string hint;

    private CanvasManager canvasManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasManager=GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>().CanvasManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player") {
            if (!hintShown)
            {
                hintShown = true;
                Debug.Log(canvasManager);
                canvasManager.showText(hint);
            }
        }
    }
}
