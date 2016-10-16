using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCastSphere : MonoBehaviour {
    public Transform castPrefab;
    List<Transform> castObjects;

	// Use this for initialization
	void Start () {
        castObjects = new List<Transform>();

    }
    IEnumerator GenerateCasts()
    {
        Vector3 pos = transform.position;
        for(int i=1;i<=5;++i)
        {
            Transform tf = (Instantiate(castPrefab, pos, Quaternion.identity) as Transform);
            //tf.GetComponent<SphereBehaviour>().
            yield return new WaitForSeconds(0.2f);
        }
    }
    // Update is called once per frame
    void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GenerateCasts());
            //Transform tf = (Instantiate(castPrefab, transform.position,Quaternion.identity) as Transform);
            //castObjects.Add(tf);
        }
	}
}
