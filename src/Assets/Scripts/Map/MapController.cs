using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Rigidbody _playerRigidbody;

    private bool _mapFaded;
    public bool MapFaded
    {
        get => _mapFaded;

        set
        {
            if (_mapFaded == value)
            {
                return;
            }

            Color end = !_mapFaded ? Color.white.WithAlpha(.5f) : Color.white;

            _mapImage.experimental.animation.Start(
                _mapImage.style.unityBackgroundImageTintColor.value, end, 500,
                (elm, val) => { elm.style.unityBackgroundImageTintColor = val; });

            _mapFaded = value;
        }
    }

    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        _playerRepresentation = _root.Q<VisualElement>("Player");
        _mapImage = _root.Q<VisualElement>("Image");
        _mapContainer = _root.Q<VisualElement>("Map");

        _playerRigidbody = Player.GetComponent<Rigidbody>();
    }
    private void LateUpdate()
    {
        float multiplyer = IsMapOpen ? fullMultiplyer : miniMultiplyer;
   
        // On raske ratast ja kaarti kattuma saada. Näib, nagu oleks ikka suurte muutujate küsimus. Saab noole kaardile, kui multiplyer on väga väike (alla nulli)

        float zMultiplier = multiplyer * 0.75f;
       _playerRepresentation.style.translate = new Translate(Player.transform.position.x * multiplyer,
            Player.transform.position.z * -zMultiplier, 0);
        _playerRepresentation.style.rotate = new Rotate(
            new Angle(Player.transform.rotation.eulerAngles.y));


        // Mini-map. Kuvab hetkel pilti vigaselt
        // Liiga kiire. Kõrguse ja laiuse jagamistehe kaotada? Hoopis toplet?
        // HETKEL SIIN KALA.

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

        MapFaded = IsMapOpen && _playerRigidbody.velocity.sqrMagnitude > 1;
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
