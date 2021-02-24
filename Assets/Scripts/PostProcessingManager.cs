using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PostProcessingManager : MonoBehaviour
{
    public Volume volume;
    Bloom bloom;
    DepthOfField dof;

    // Start is called before the first frame update
    void Start()
    {
        if(volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.threshold.value = 0.55f;
            bloom.intensity.value = 19.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                bloom.threshold.value = 0.55f;
                bloom.intensity.value = 19.0f;
            }
            else if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
            {
                bloom.threshold.value = 1.0f;
                bloom.intensity.value = 4.75f;
                bloom.scatter.value = 0.05f;
            }
        }

        if(volume.profile.TryGet<DepthOfField>(out dof))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                dof.mode.value = DepthOfFieldMode.Off;
            }
            else if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
            {
                dof.mode.value = DepthOfFieldMode.Bokeh;
                dof.focusDistance.value = 12.0f;
                dof.focalLength.value = 300.0f;
                dof.aperture.value = 15.5f;
            }
        }
    }
}
