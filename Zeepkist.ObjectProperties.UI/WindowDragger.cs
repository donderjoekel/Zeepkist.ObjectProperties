using UnityEngine;
using UnityEngine.EventSystems;

namespace TNRD.Zeepkist.ObjectProperties.UI
{
    public class WindowDragger : MonoBehaviour, IDragHandler
    {
        [SerializeField] private Transform transformToMove;
    
        /// <inheritdoc />
        public void OnDrag(PointerEventData eventData)
        {
            transformToMove.position += (Vector3)eventData.delta;
        }
    }
}
