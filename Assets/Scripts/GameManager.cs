using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static bool _isVR;
    public static bool IsVR { get { return _isVR; } }
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    void Awake()
    {
        if(_instance != null)
        {
            Debug.LogError("Duplicate GameManager created.  Deleting duplicate.");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _isVR = UnityEngine.VR.VRDevice.isPresent;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
