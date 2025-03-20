using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationChange : MonoBehaviour
{
    public TMP_Dropdown locationDropdown;
    public GameObject Rotatables;
    private string currentLocation;
    // Start is called before the first frame update
    void Start()
    {
        currentLocation = "initial";
        ChangeLocation();
        locationDropdown.onValueChanged.AddListener(delegate { ChangeLocation(); });
    }

    private string newLocation;
    // Update is called once per frame
    void ChangeLocation()
    {
        newLocation = locationDropdown.captionText.text;
        if (!currentLocation.Equals(newLocation))
        {
            //getting values
            double oldLatitude = GetLatitude(currentLocation);
            double newLatitude = GetLatitude(newLocation);
            double oldLongitude = GetLongitude(currentLocation);
            double newLongitude = GetLongitude(newLocation);
            currentLocation = newLocation;

            //rotate sphere back to standard (which is lat,long = 0,0)
            Rotatables.transform.Rotate(0, 0, -(float)oldLatitude, Space.World);
            Rotatables.transform.Rotate(-(float)oldLongitude, 0, 0, Space.World);

            //rotate to new lat, long
            Rotatables.transform.Rotate((float)newLongitude, 0, 0, Space.World);
            Rotatables.transform.Rotate(0, 0, (float)newLatitude, Space.World);
        }
    }

    double GetLatitude(string location)
    {
        switch (location)
        {
            case "initial": return 0.0;
            case "Athens, GA, USA": return 33.957409;
            case "New York City, USA": return 40.712776;
            case "Los Angeles, USA": return 34.052235;
            case "Juneau, AK, USA": return 58.301945;
            case "Iqaluit, Canada": return 63.746693;
            case "Mexico City, Mexico": return 19.432608;
            case "Bogota, Colombia": return 4.710989;
            case "Rio de Janeiro, Brazil": return -22.906847;
            case "Stanley, Falkland Islands": return -51.700981;
            case "Reykjavík, Iceland": return 64.126518;
            case "London, UK": return 51.507351;
            case "Moscow, Russia": return 55.755825;
            case "Cairo, Egypt": return 30.044420;
            case "Casablanca, Morocco": return 33.573109;
            case "Kinshasa, DRC": return -4.441931;
            case "Cape Town, South Africa": return -33.924870;
            case "Karachi, Pakistan": return 24.860735;
            case "Chennai, India": return 13.082680;
            case "Kashgar, China": return 39.467686;
            case "Chengdu, China": return 30.572815;
            case "Seoul, South Korea": return 37.566536;
            case "Norilsk, Russia": return 69.330399;
            case "Anadyr, Russia": return 64.732857;
            case "Jakarta, Indonesia": return -6.175110;
            case "Perth, Australia": return -31.950527;
            case "Sydney, Australia": return -33.868820;
            case "Palikir, FSM": return 6.916644;
            case "Suva, Fiji": return -18.124809;
            case "Honolulu, USA": return 21.306944;
            case "Papeete, French Polynesia": return -17.535000;
            case "South Pole, Antarctica": return 90.0;
            case "North Pole, Arctic Circle": return -90.0;
        }
        return 0.0;
    }

    double GetLongitude(string location)
    {
        switch (location)
        {
            case "initial": return 0.0;
            case "Athens, GA, USA": return -83.376801;
            case "New York City, USA": return -74.005974;
            case "Los Angeles, USA": return -118.243683;
            case "Juneau, AK, USA": return -134.419724;
            case "Iqaluit, Canada": return -68.516968;
            case "Mexico City, Mexico": return -99.133209;
            case "Bogota, Colombia": return -74.072090;
            case "Rio de Janeiro, Brazil": return -43.172897;
            case "Stanley, Falkland Islands": return -57.849190;
            case "Reykjavík, Iceland": return -21.817438;
            case "London, UK": return -0.127758;
            case "Moscow, Russia": return 37.617298;
            case "Cairo, Egypt": return 31.235712;
            case "Casablanca, Morocco": return -7.589843;
            case "Kinshasa, DRC": return 15.266293;
            case "Cape Town, South Africa": return 18.424055;
            case "Karachi, Pakistan": return 67.001137;
            case "Chennai, India": return 80.270721;
            case "Kashgar, China": return 75.993790;
            case "Chengdu, China": return 104.066803;
            case "Seoul, South Korea": return 126.977966;
            case "Norilsk, Russia": return 88.220497;
            case "Anadyr, Russia": return 177.507812;
            case "Jakarta, Indonesia": return 106.865036;
            case "Perth, Australia": return 115.860458;
            case "Sydney, Australia": return 151.209290;
            case "Palikir, FSM": return 158.149974;
            case "Suva, Fiji": return 178.450073;
            case "Honolulu, USA": return -157.858337;
            case "Papeete, French Polynesia": return -149.569595;
            case "South Pole, Antarctica": return 0.0;
            case "North Pole, Arctic Circle": return 0.0;
        }
        return 0.0;
    }
}
