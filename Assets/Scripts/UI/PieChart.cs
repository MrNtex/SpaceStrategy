using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieChart : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerClickHandler
{
    enum DataSource
    {
        NationalUnity,
        Planet,
        Colony,
        Decision
    }
    enum PieChartType
    {
        HoverToSelect,
        ClickToSelect
    }
    [SerializeField]
    private PieChartType pieChartType;

    [SerializeField] private DataSource dataSource;

    [SerializeField]
    [Range(0, 1)]
    private float donutSize = 0;

    public int SelectedSliceIndex = -1;
    public int hoveredSelectedSliceIndex = -1;

    public List<float> values = new List<float>();
    public List<Color> colors = new List<Color>();

    [SerializeField]
    private GameObject slice;

    [SerializeField]
    private Transform background;


    List<float> angles = new List<float>();
    List<Transform> slices = new List<Transform>();

    private Dictionary<Transform, Coroutine> activeCoroutines = new Dictionary<Transform, Coroutine>();

    private RectTransform rectTransform;
    [SerializeField]
    private Camera UICamera;

    [SerializeField]
    private Tooltip tooltip;
    Transform selectedSlice
    {
        get
        {
            return _selectedSlice;
        }
        set
        {
            if(value == null)
            {
                if (_selectedSlice == null) return;
                AnimateTransform(_selectedSlice, Vector3.one);
                _selectedSlice = null;
                return;
            }
            if (_selectedSlice != value)
            {
                if(_selectedSlice != null) AnimateTransform(_selectedSlice, Vector3.one);
                
                _selectedSlice = value;
                AnimateTransform(_selectedSlice, new Vector3(1.3f, 1.3f));

                tooltip.ShowTooltip(GetTooltipData(), TooltipTarget.Piechart);
            }
        }
    }

    Transform _selectedSlice;

    TooltipData GetTooltipData()
    {
        switch(dataSource)
        {
            case DataSource.NationalUnity:
                return NationalUnity.instance.GetTooltipData(hoveredSelectedSliceIndex);
            default:
                Debug.LogError("No tooltip data for this data source");
                return new TooltipData();
        }
    }


    void Start()
    {
        rectTransform = background.GetComponent<RectTransform>();
        SetValues();

        if(tooltip == null)
        {
            Debug.LogWarning("Tooltip is null, please assign it in the inspector. Assigned default");
            tooltip = MenusManager.Instance.mainCanvas.GetComponent<Tooltip>();
        }
    }
    void OnEnable()
    {
        foreach (Transform slice in slices)
        {
            slice.localScale = Vector3.one;
        }

        selectedSlice = null;
        hoveredSelectedSliceIndex = -1;

        StopAllCoroutines();
        activeCoroutines.Clear();
    }
    public void SetValues()
    {
        float[] percents = FindPercentage(values.ToArray());
        float rotation = 0;

        StopAllCoroutines();
        activeCoroutines.Clear();

        foreach (Transform slice in slices)
        {
            Destroy(slice.gameObject);
        }
        slices.Clear();
        angles.Clear();


        for (int i = 0; i < percents.Length; i++)
        {
            // I save the rotation of each slice to be able to know which slice the pointer is hovering over
            angles.Add(rotation);

            GameObject newSlice = Instantiate(slice, background);
            newSlice.GetComponent<Image>().color = colors[i];
            newSlice.GetComponent<Image>().fillAmount = percents[i];
            newSlice.transform.rotation = Quaternion.Euler(0, 0, rotation);
            rotation -= 360 * percents[i];
            
            slices.Add(newSlice.transform);
        }
        angles.Add(rotation);

        if (donutSize > 0)
        {
            GameObject hole = Instantiate(slice, background);
            hole.GetComponent<Image>().color = background.GetComponent<Image>().color;
            hole.transform.localScale = new Vector3(donutSize, donutSize, 1);
            hole.transform.SetAsLastSibling();
            hole.GetComponent<Image>().fillAmount = 1;
        }
    }

    private float[] FindPercentage(float[] values)
    {
        float total = 0;
        foreach (float value in values)
        {
            total += value;
        }

        float[] percentages = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            percentages[i] = values[i] / total;
        }
        return percentages;
    }
    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        // Get the angle of the pointer relative to the center of the pie chart
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, UICamera, out localPoint); // eventData.pressEventCamera doesn't unless you first click on something
        
        tooltip.MoveTooltip(eventData.position);

        float angle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;

        angle -= 90;

        //Normalize angle
        angle = 360 - (angle + 360) % 360;

        // Find the slice that the pointer is hovering over
        for (int i = 0; i < angles.Count; i++)
        {
            if (angle < -angles[i])
            {
                
                hoveredSelectedSliceIndex = i - 1;
                selectedSlice = slices[i - 1];
                break;
            }
        }
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        selectedSlice = null;
        hoveredSelectedSliceIndex = -1;

        tooltip.HideTooltip();
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (pieChartType == PieChartType.HoverToSelect) return;
        if(hoveredSelectedSliceIndex == SelectedSliceIndex)
        {
            hoveredSelectedSliceIndex = -1;
            SelectedSliceIndex = -1;
            return;
        }
        int temp = SelectedSliceIndex;
        SelectedSliceIndex = hoveredSelectedSliceIndex;
        if (temp != -1)
        {
            AnimateTransform(slices[temp], Vector3.one);
        }
    }
    public void AnimateTransform(Transform transform, Vector3 targetScale)
    {
        if (SelectedSliceIndex != -1 && slices[SelectedSliceIndex] == transform) return;
       float speed = 2.3f;
        // Check if there's already a running coroutine for this transform
        if (activeCoroutines.TryGetValue(transform, out Coroutine runningCoroutine))
        {
            if (runningCoroutine == null)
            {
                activeCoroutines.Remove(transform);
                return;
            }
            StopCoroutine(runningCoroutine);
            activeCoroutines.Remove(transform);
        }

        Coroutine newCoroutine = StartCoroutine(AnimateSlice(transform, targetScale, speed));
        activeCoroutines[transform] = newCoroutine;
    }

    IEnumerator AnimateSlice(Transform transform, Vector3 targetScale, float speed)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
            yield return null;
        }

        transform.localScale = targetScale;
        activeCoroutines.Remove(transform); 
    }

}
