using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musica;
    public Slider sliderVolume;

    void Start()
    {
        // Garante que o valor do slider reflete o volume atual
        sliderVolume.value = musica.volume;

        // Toda vez que o valor mudar, chama a função de ajuste
        sliderVolume.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        musica.volume = volume;
    }
}
