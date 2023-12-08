using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class MapController : MonoBehaviour
{
    private VisualElement _root;
    private bool IsMapOpen => _root.ClassListContains("root-container-full");

    public GameObject Player;
    [Range(1, 15)]
    public float miniMultiplyer = 5.3f;
    [Range(0, 15)]
    public float fullMultiplyer = 7f;
    private VisualElement _playerRepresentation;

    private VisualElement _mapContainer;
    private VisualElement _mapImage;

    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        _playerRepresentation = _root.Q<VisualElement>("Player");
        _mapImage = _root.Q<VisualElement>("Image");
        _mapContainer = _root.Q<VisualElement>("Map");
    }
    private void LateUpdate()
    {
        float multiplyer = IsMapOpen ? fullMultiplyer : miniMultiplyer;
        // SIIN ON KALA. J�rgmine rida t��tab, aga tegelase liigutamine kaardil kaotab ta kaardilt.
        // Probleem pule muutujates: ka fikseeritud suurusega pole seda n�ha. Kolmanda muutuja suurendamine v�i kahandamine (-1000 kuni 1000) ei teinud midagi
        // Kui seadsin v��rtused madalaks, siis t��tas veidi. LIIGUB! NEGATIIVSED KOORDINAADID OLI PROBLEEM?
        // On raske ratast ja kaarti kattuma saada. N�ib, nagu oleks ikka suurte muutujate k�simus. Saab noole kaardile, kui multiplyer on v�ga v�ike (alla nulli)
        // Samas usun, et  abiks v�iks olla see, kui ratta koordinaadid v�imalikult v�iksed oleks.
        float zMultiplier = multiplyer * 0.75f;
       _playerRepresentation.style.translate = new Translate(Player.transform.position.x * multiplyer,
            Player.transform.position.z * -zMultiplier, 0);
        _playerRepresentation.style.rotate = new Rotate(
            new Angle(Player.transform.rotation.eulerAngles.y));


        // Mini-map. Kuvab hetkel pilti vigaselt
        // Liiga kiire. K�rguse ja laiuse jagamistehe kaotada? Hoopis toplet?

        if (!IsMapOpen)
        {
            var clampWidth = _mapImage.worldBound.width / 2 -
                _mapContainer.worldBound.width / 2;
            var clampHeight = _mapImage.worldBound.height / 2 -
                _mapContainer.worldBound.height / 2;

            var xPos = Mathf.Clamp(Player.transform.position.x * -multiplyer,
                -clampWidth, clampWidth);
            var yPos = Mathf.Clamp(Player.transform.position.z * zMultiplier,
                -clampHeight, clampHeight);

            _mapImage.style.translate = new Translate(xPos, yPos, 0);
        }
        else
        {
            _mapImage.style.translate = new Translate(0, 0, 0);
        }
        // Alternate solution
        //_mapImage.style.translate = new Translate(Player.transform.position.x * -multiplyer, Player.transform.position.z * zMultiplier, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap(!IsMapOpen);
        }
    }

    private void ToggleMap(bool on)
    {
        _root.EnableInClassList("root-container-mini", !on);
        _root.EnableInClassList("root-container-full", on);
    }
}
