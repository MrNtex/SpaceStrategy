using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFocusHelper : MonoBehaviour
{

    private Orbiting orbiting;


    [SerializeField]
    private Vector3 offset;

    

    [SerializeField]
    public Transform target;

    private GameObject cameraPlacement;

    private readonly Vector2 defaultOffset = new Vector2(11.62f, 7.85f);

    private const float angleDistanceRatio = 0.004f;

    private Billboard billboard;

    public float minDistance = 3000;

    public const float hitboxMultiplier = 1.5f;
    private void Awake()
    {
        orbiting = GetComponent<Orbiting>();
        //billboard.minDistance = minDistance;
        if (orbiting != null)
        {
            target = orbiting.target;

        }

        if(offset == Vector3.zero && cameraPlacement == null)
        {
            if(target == null)
            {
                // For bodies like the sun
                offset = defaultOffset;
            }
            else
            {
                offset = (transform.position - target.position).normalized;
                offset = new Vector3(offset.x * defaultOffset.x, (offset.y + 1) * defaultOffset.y, offset.z * defaultOffset.x);
            }
            
        }
    }
    private void Start()
    {
        if(cameraPlacement == null)
        {
            cameraPlacement = new GameObject("CameraPlacement");
            cameraPlacement.transform.SetParent(transform);
            cameraPlacement.transform.localPosition = offset;
            if (target == null)
            {
                cameraPlacement.transform.LookAt(transform);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(transform.position - cameraPlacement.transform.position);
                rotation = Quaternion.Euler(rotation.eulerAngles.x - Vector3.Distance(transform.position, target.position) * angleDistanceRatio, rotation.eulerAngles.y, 0);
                cameraPlacement.transform.rotation = rotation;
            }
            
        }
    }
    private void OnEnable()
    {
        Camera.main.GetComponent<CameraFocus>().onLeftClick += RecalculateColliders;
    }
    public void RecalculateColliders()
    {
        Debug.Log("Recalculating colliders for: " + gameObject.name);
    }
    public Transform Focus()
    {
        return cameraPlacement.transform;
    }

}
