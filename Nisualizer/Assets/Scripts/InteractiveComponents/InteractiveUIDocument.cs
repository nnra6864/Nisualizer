using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractiveComponents
{
    [RequireComponent(typeof(UIDocument))]
    public class InteractiveUIDocument : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private UIDocument _uiDocument;
        private string _vtaName;
        private string _styleName;
        
        private void Reset()
        {
            _uiDocument = GetComponent<UIDocument>();
        }
        
    }
}