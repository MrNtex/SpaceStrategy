using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ClickableLinkHandler : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TMP_Text textMeshPro;

    private Focus cameraFocus;

    public static GameObject adress; // Object that the link addresses

    void Start()
    {
        cameraFocus = CameraControler.mainCamera.GetComponent<Focus>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);

        if (linkIndex != -1) // Means a link was clicked
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();

            HandleLinkClick(linkId);
        }
    }

    private void HandleLinkClick(string linkId)
    {
        if (adress == null)
        {
            Debug.LogWarning("Adress is not set!");
            return;
        }
        switch (linkId)
        {
            case "Fleet":
            case "CelestialBody":
                ObjectFocusHelper objectFocusHelper = adress.GetComponent<ObjectFocusHelper>();
                if(objectFocusHelper == null)
                {
                    Debug.LogWarning("Adress does not have ObjectFocusHelper component!");
                    return;
                }

                cameraFocus.FocusOn(objectFocusHelper, true);
                break;
            
            default:
                Debug.Log("Unknown link clicked!");
                break;
        }
    }
}
