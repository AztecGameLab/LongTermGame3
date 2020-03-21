using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardImposter : MonoBehaviour
{
    Image image;

    public bool mirror;
    public bool sideViewFacingLeft;

    public Transform player;
    public Sprite[] sprites;

    int directionSegments;
    
    void Start()
    {
        try
        {
            player = GameObject.Find("Player").transform;
            Debug.LogWarning("Need a better way to find player");
        }
        catch
        {
            Debug.LogError("Could not find player");
        }


        directionSegments = sprites.Length * 2;
        image = transform.GetComponentInChildren<Image>();
    }

    void Update()
    {
        Vector3 topDownDirection = (player.position - transform.position).normalized;
        topDownDirection.y = 0;
        image.transform.forward = topDownDirection;

        float direction = image.transform.localEulerAngles.y / 360;
        int intDirection = (int)((((direction * directionSegments) + 1)%directionSegments)/2f);


        for (int i = 0; i < sprites.Length; i++)
        {
            if(intDirection == i)
            {
                if (mirror && i > sprites.Length / 2)
                    if (sideViewFacingLeft)
                        image.transform.localScale = new Vector3(-1, 1, 1);
                    else
                        image.transform.localScale = new Vector3(1, 1, 1);
                else
                    if(sideViewFacingLeft)
                        image.transform.localScale = new Vector3(1, 1, 1);
                    else
                        image.transform.localScale = new Vector3(-1, 1, 1);


                image.sprite = sprites[i];
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 2f, 0), (transform.position + new Vector3(0, 2f, 0)) + (transform.forward * 2));
    }

}
