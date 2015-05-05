using UnityEngine;
using UnityEngine.EventSystems;

namespace MapNamespace
{
	public class MinimapPanel : MonoBehaviour, IPointerClickHandler
	{
		public void OnPointerClick(PointerEventData data)
		{
			MinimapManager.GetInstance().OnClickMinimap(data.position);
		}
	}
}
