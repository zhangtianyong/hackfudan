using UnityEngine;
using System.Collections;

public class SphereBehaviour : MonoBehaviour
{
    public Color InitColor;
    public float ExpandSpeed;// Default Value
    float radius;

    // Use this for initialization
	void Start () {
        radius = 0;
	}
	
	// Update is called once per frame
	void Update () {
        radius = radius + ExpandSpeed * Time.deltaTime;
        transform.localScale = new Vector3(radius, radius, radius);
    }
}
