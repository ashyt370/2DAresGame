using System.Collections.Generic;
using UnityEngine;

public class EvasionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EvasionManager instance;

    [SerializeField]
    private List<GameObject> evasionPointList;

    public Color disableColor;
    public Color activatedColor;

    public float waitTime;
    

    private void Awake()
    {
        if(instance ==  null)
        {
            instance = this;
        }
    }

    public void SetRandomEvasionPoint()
    {
        int rand = Random.Range(0, evasionPointList.Count-1);

        if(evasionPointList[rand])
        {
            evasionPointList[rand].GetComponent<EvasionPoint>().ActivateEvasionPoint();
        }
        
    }
}
