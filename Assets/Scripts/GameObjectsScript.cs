using NUnit.Framework;
using UnityEngine;

public class GameObjectsScript : MonoBehaviour
{
    [Header("Vehicle spawnpoints")]
    public Transform[] spawnpoints;

    [Header("Vehicle objects")]
    public GameObject[] vehicleObjects;

    public GameObject garbageTruck;
    public GameObject medicine;
    public GameObject schoolBus;
    public GameObject b2;
    public GameObject cementTruck;
    public GameObject e46;
    public GameObject e61;
    public GameObject excavator;
    public GameObject police;
    public GameObject yellowTractor;
    public GameObject greenTractor;
    public GameObject firefighters;

    // Vēlāk jāpievieno pārējās mašīnas un to sākuma koordinātas...

    [HideInInspector] 
    public Vector2 garbageTruckCoord;
    [HideInInspector]
    public Vector2 medicineCoord;
    [HideInInspector]
    public Vector2 schoolBusCoord;
    [HideInInspector]
    public Vector2 b2Coord;
    [HideInInspector]
    public Vector2 cementTruckCoord;
    [HideInInspector]
    public Vector2 e46Coord;
    [HideInInspector]
    public Vector2 e61Coord;
    [HideInInspector]
    public Vector2 excavatorCoord;
    [HideInInspector]
    public Vector2 policeCoord;
    [HideInInspector]
    public Vector2 yellowTractorCoord;
    [HideInInspector]
    public Vector2 greenTractorCoord;
    [HideInInspector]
    public Vector2 firefightersCoord;

    public Canvas canvas;
    public AudioSource carSoundSource;
    public AudioClip[] sounds;

    [HideInInspector]
    public bool inRightPlace = false;
    public static GameObject lastDragged = null;
    public static bool isDragging = false;


    
    void Awake()
    {

        //firefighters.transform.position = spawnpoint1.transform.position;
        for(int i=0; i<13; i++)
        {

        }

        garbageTruckCoord = garbageTruck.GetComponent<RectTransform>().localPosition;
        medicineCoord = medicine.GetComponent<RectTransform>().localPosition;
        schoolBusCoord = schoolBus.GetComponent<RectTransform>().localPosition;
        b2Coord = b2.GetComponent<RectTransform>().localPosition;
        cementTruckCoord = cementTruck.GetComponent<RectTransform>().localPosition;
        e46Coord = e46.GetComponent<RectTransform>().localPosition;
        e61Coord = e61.GetComponent<RectTransform>().localPosition;
        excavatorCoord = excavator.GetComponent<RectTransform>().localPosition;
        policeCoord = police.GetComponent<RectTransform>().localPosition;
        yellowTractorCoord = yellowTractor.GetComponent<RectTransform>().localPosition;
        greenTractorCoord = greenTractor.GetComponent<RectTransform>().localPosition;
        firefightersCoord = firefighters.GetComponent<RectTransform>().localPosition;
    }
}