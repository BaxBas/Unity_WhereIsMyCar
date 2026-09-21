using System.Collections.Generic;
using UnityEngine;

public class GameObjectsScript : MonoBehaviour
{
    [Header("Vehicle spawnpoints")]
    public Transform[] spawnpoints;

    [HideInInspector]
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
        PlaceVehiclesRandomly();

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

    void PlaceVehiclesRandomly()
    {
        vehicleObjects = new GameObject[]
        {
            garbageTruck, medicine, schoolBus, b2, cementTruck, e46,
            e61, excavator, police, yellowTractor, greenTractor, firefighters
        };

        if (spawnpoints == null || spawnpoints.Length == 0)
        {
            Debug.LogWarning("No spawnpoints assigned - vehicles stay where they are.");
            return;
        }

        List<Transform> spawnpointsCopy = new List<Transform>();
        foreach (Transform spawnpoint in spawnpoints)
        {
            if (spawnpoint != null)
                spawnpointsCopy.Add(spawnpoint);
        }

        if (spawnpointsCopy.Count < vehicleObjects.Length)
        {
            Debug.LogWarning("There are less spawnpoints than vehicles - some vehicles won't be moved.");
        }

        foreach (GameObject vehicle in vehicleObjects)
        {
            if (vehicle == null) continue;
            if (spawnpointsCopy.Count == 0) break;

            int index = Random.Range(0, spawnpointsCopy.Count);

            vehicle.transform.position = spawnpointsCopy[index].position;
            spawnpointsCopy.RemoveAt(index);
        }
    }
}