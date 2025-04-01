using UnityEngine;

public class DragController : MonoBehaviour
{
    bool isDragActive = false;
    Vector3 _screenPosition;
    Vector3 _worldPosition;
    public Draggable _lastDragged;
    public static DragController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
   
    private void Update()
    {
        if (isDragActive && (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Drop();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _screenPosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            _screenPosition = Input.GetTouch(0).position;
        }
        else { return; }

        // Lanzar un rayo desde la cámara al mundo 3D
        Ray ray = Camera.main.ScreenPointToRay(_screenPosition);
        RaycastHit hit;

        if (isDragActive)
        {
            Drag(ray);
        }
        else
        {
            if (Physics.Raycast(ray, out hit))
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag()
    {
        isDragActive = true;
        _lastDragged.gameObject.GetComponent<Draggable>().OnDrag();
        //_lastDragged.transform.parent = null;
        //_lastDragged.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    void Drop()
    {
        _lastDragged.transform.gameObject.GetComponent<Draggable>().OnDrop();
        try
        {
            _lastDragged.transform.gameObject.GetComponent<EnergyManager>().DropInCard();
        }
        catch { }
        isDragActive = false;
    }

    void Drag(Ray ray)
    {
        Plane plane = new Plane(Vector3.up, _lastDragged.transform.position); // Plano donde se moverá el objeto
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            _worldPosition = ray.GetPoint(distance);
            _lastDragged.transform.position = _worldPosition;
        }
    }
   
}
