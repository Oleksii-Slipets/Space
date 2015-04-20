using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapPanel : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData data)
	{
		MinimapManager.GetInstance().OnClickMinimap(data.position);
	}
}
