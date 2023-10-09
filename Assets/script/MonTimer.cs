using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonTimer : MonoBehaviour
{

    Material leMateriel;
    public string laPropriete;

    public float tempsProgression;
    public float incrementation;

    public float minimum;
    public float maximum;

    void Start()
    {
        leMateriel = GetComponent<Renderer>().material;
        laPropriete = "_" + laPropriete;
    }

    void Commencer()
    {
          StartCoroutine(transition());
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Commencer();
        }
    }



    public IEnumerator transition()
    {
        float tempsPasse = 0;

        while(tempsPasse < tempsProgression)
        {
            tempsPasse += Time.deltaTime;

            incrementation = Mathf.Lerp(minimum, maximum, tempsPasse / tempsProgression);

            leMateriel.SetFloat(laPropriete, incrementation);

            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }

    }

}
