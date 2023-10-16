using UnityEngine;

namespace GMEngine.UI
{
    [DisallowMultipleComponent]
    /// <summary>
    /// Provides the transform of the UI need to be located
    /// </summary>
    public class UILocator : MonoBehaviour
    {
        public Vector2 offset;
        public Transform locatingGO;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            SetUIPosition();
        }

        public void SetUIPosition()
        {
            transform.position = TargetPosition();
            Debug.Log($"Setting {name} Position, Position is {TargetPosition()}");
        }

        private Vector3 TargetPosition()
        {
            Vector3 parentSPP = Camera.main.WorldToScreenPoint(locatingGO.position);
            return parentSPP + new Vector3 (offset.x, offset.y, 0);
        }



        #region Editor
#if UNITY_EDITOR
        [Header("Editor")]
        public GameObject UIPrefab;
        [HideInInspector]
        public GameObject UISceneGO;
#endif
        #endregion
    }

}

