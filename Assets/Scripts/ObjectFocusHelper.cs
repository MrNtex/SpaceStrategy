using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFocusHelper : MonoBehaviour
{

    private Orbiting orbiting;
    public ObjectInfo objectInfo;

    [SerializeField]
    private Vector3 offset;

    

    [SerializeField]
    public Transform target;

    public Transform cameraPlacement;

    private readonly Vector2 defaultOffset = new Vector2(11.62f, 7.85f);

    private const float angleDistanceRatio = 0.004f;

    public float minDistance = 3000;

    private float hitboxMultiplier = 0.025f;

    private GameObject cameraMain;
    private SphereCollider myCollider;

    private Material defaultMaterial;
    [SerializeField]
    private Material highlightMaterial;

    
    private void Awake()
    {
        cameraMain = Camera.main.gameObject;

        orbiting = GetComponent<Orbiting>();
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
        myCollider = GetComponent<SphereCollider>();
        hitboxMultiplier /= transform.localScale.x;

        if(cameraPlacement == null)
        {
            cameraPlacement = new GameObject("CameraPlacement").transform;
            cameraPlacement.SetParent(transform);
            cameraPlacement.localPosition = offset;
            if (target == null)
            {
                cameraPlacement.LookAt(transform);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(transform.position - cameraPlacement.position);
                rotation = Quaternion.Euler(rotation.eulerAngles.x - Vector3.Distance(transform.position, target.position) * angleDistanceRatio, rotation.eulerAngles.y, 0);
                cameraPlacement.rotation = rotation;
            }
            
        }

        defaultMaterial = gameObject.GetComponent<MeshRenderer>().material;

        objectInfo = GetComponent<ObjectInfo>();
    }
    private void OnEnable()
    {
        cameraMain.GetComponent<CameraFocus>().onLeftClick += RecalculateColliders;
    }
    public void RecalculateColliders()
    {
        float dist = Vector3.Distance(cameraMain.transform.position, transform.position);
        
        myCollider.radius = Mathf.Clamp(dist * hitboxMultiplier, 0.5f, 6);
    }
    public ObjectFocusHelper Focus(bool force = false)
    {
        if(!force && Vector3.Distance(cameraMain.transform.position, transform.position) > minDistance && transform.parent != null)
        {
            return transform.parent.GetComponent<ObjectFocusHelper>();
        }
        return this;
    }

}
