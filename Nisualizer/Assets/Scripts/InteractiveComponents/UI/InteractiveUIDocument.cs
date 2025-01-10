using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace InteractiveComponents.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InteractiveUIDocument : MonoBehaviour
    {
        private string _uxmlPath, _ussPath;
        
        [ReadOnly] [SerializeField] private UIDocument _document;
        
        private void Reset()
        {
            _document = GetComponent<UIDocument>();
        }

        public void SetUXML(string path)
        {
            
        }
    }
}