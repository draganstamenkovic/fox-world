using Configs;
using UnityEngine;
using VContainer;

public class SpinnerController : MonoBehaviour
{
    [Inject] private GuiConfig _guiConfig;
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _guiConfig.loadingRotateSpeed));
    }
}
