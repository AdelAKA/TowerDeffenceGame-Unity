using UnityEngine;
using UnityEngine.UI;

public class WavesUI : MonoBehaviour
{
    public Text[] wave;
    void Update()
    {
        // The Right world canvas Text for current wave
        wave[0].text = "W\nA\nV\nE\n" + PlayerStatus.Rounds.ToString();
        // The On screen status Canvas text for current wave
        wave[1].text = PlayerStatus.Rounds.ToString();
    }
}
