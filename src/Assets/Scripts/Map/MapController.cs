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

    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        _playerRepresentation = _root.Q<VisualElement>("Player");
    }
    private void LateUpdate()
    {
        float multiplyer = IsMapOpen ? fullMultiplyer : miniMultiplyer;
        // SIIN ON KALA. Järgmine rida töötab, aga tegelase liigutamine kaardil kaotab ta kaardilt.
        // Probleem pule muutujates: ka fikseeritud suurusega pole seda näha. Kolmanda muutuja suurendamine või kahandamine (-1000 kuni 1000) ei teinud midagi
        // Kui seadsin väärtused madalaks, siis töötas veidi. LIIGUB! NEGATIIVSED KOORDINAADID OLI PROBLEEM?
        // On raske ratast ja kaarti kattuma saada. Näib, nagu oleks ikka suurte muutujate küsimus. Saab noole kaardile, kui multiplyer on väga väike (alla nulli)
        // Samas usun, et  abiks võiks olla see, kui ratta koordinaadid võimalikult väiksed oleks.
       _playerRepresentation.style.translate = new Translate(Player.transform.position.x * multiplyer,
            Player.transform.position.z * -multiplyer, 0);
        _playerRepresentation.style.rotate = new Rotate(
            new Angle(Player.transform.rotation.eulerAngles.y));
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
