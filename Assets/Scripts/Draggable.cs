using System.Collections;
using UnityEngine;

public class Draggable:MonoBehaviour
{
    bool isDragging = false;
    public void OnDrag() => isDragging = true;
    public void OnDrop() => isDragging = false;
    [field:SerializeField] protected CharacterUIManager targetCharacter;
    private void Update()
    {
        if (!isDragging) return;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, color: Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) 
        {
            if (hit.collider.transform.parent.GetComponent<CharacterUIManager>() != null)
            {
                // Debug.Log(hit.collider.gameObject.name);
                targetCharacter = hit.collider.transform.parent.GetComponent<CharacterUIManager>();
            }
            
        }
        else
        {
            targetCharacter = null;
        }
    }

    public virtual void DropInCard()
    {
        StartCoroutine(DestroyDraggable());
    }

    IEnumerator DestroyDraggable()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
